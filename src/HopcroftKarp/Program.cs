using System;
using System.Collections.Generic;
using System.Linq;

namespace HopcroftKarp
{
    public class HopcroftKarpMatching
    {
        public static List<HashSet<Node>> Bfs(BipartiteGraph graph, Matching matching)
        {
            // initialize layers of the output graph
            var layers = new List<HashSet<Node>>();

            // initialize the first layer starting on the left
            layers.Add(
                graph
                    .Left
                    .Where(node => !matching.Contains(node))
                    .ToHashSet()
            );

            // bfs loop
            while(true)
            {
                var currentLayer = layers.Last();
                var nextLayer = new HashSet<Node>();

                foreach (var node in currentLayer)
                {
                    // iterate through neighbours that have NOT already been visisted
                    foreach (var neighbor in node.Connections.Where(neighbour => !layers.Contains(neighbour)))
                    {
                        // if node is on the left side of the graph
                        if (graph.Left.Contains(node))
                        {
                            if (!matching.Contains(node) || !matching.HasPair(node, neighbor))
                            {
                                nextLayer.Add(neighbor);
                            }
                        }
                        else // if its on the right side of the graph
                        {
                            // add it to the layer if that IS a mathing path between them
                            if (matching.HasPair(node, neighbor))
                            {
                                nextLayer.Add(neighbor);
                            }
                        }
                    }
                }

                // no nodes were added this iteration, then bfs is finished and fully explored
                if (nextLayer.Count == 0) { break; }

                // if a node was encountered in this new layer that is unmatched we reach the maximum depth
                if (nextLayer.Any(node => !matching.Contains(node)))
                {
                    layers.Add(
                        // remove any matched nodes from this final layer before returning
                        nextLayer
                        .Where(node => !matching.Contains(node))
                        .ToHashSet()
                    );
                    break;
                }

                // append next layers to layers to be retrieved in next iteration
                layers.Add(nextLayer);
            }

            return layers;
        }

        public static List<Node> Dfs(
            List<HashSet<Node>> layers, 
            Node node, 
            int layerIndex, 
            Matching matching, 
            List<Node> path)
        {
            // if we reach the bottom layer of our layered graph AND we find an unmatched node
            // this is because only the bottom and top layers of the graph should contain unmatched nodes
            if (!matching.Contains(node) && layerIndex == 0)
            {
                return new List<Node> { node }.Merge(path);
            }

            // filter node neighbors by whether they are in the next layer
            // providing we aren't at the bottom layer
            var neighbors = node
                .Connections
                .Where(neighbor => layerIndex > 0 && layers[layerIndex - 1].Contains(neighbor))
                .ToList();

            // if there are at least one valid neighbor
            if (neighbors.Count != 0)
            {
                // recursively call Dfs for all neighbors producing an array of lists
                return neighbors.Select(neighbor => Dfs(
                        layers,
                        neighbor,
                        layerIndex - 1,
                        matching,
                        new List<Node> { node }.Merge(path) // update the path so far with this node
                    )
                )
                // flatten lists
                .SelectMany(n => n)
                // ensure the returned path is only the same length as the number of layers
                .Take(layers.Count)
                .ToList();
                
            }
            // otherwise just return an empty list, this nullifies any path that was store so far on this search branch
            // so that a lot of empty lists are flattened leading to only the correct path remaining
            else
            {
                return new List<Node>();
            }
        }

        public static Matching Run(BipartiteGraph graph)
        {
            var matching = new Matching();

            // run bfs
            do
            {
                // if all nodes in either side are matched then a maximum matching has been found
                if (graph.Left.All(node => matching.Contains(node)) || 
                    graph.Right.All(node => matching.Contains(node)))
                {
                    break;
                }

                var layers = Bfs(graph, matching);

                // if bfs produces a path which is uneven then no more augmenting paths can be generated
                if (layers.Count % 2 == 1) { break; }

                // run dfs to isolate augmenting paths in layers and update matching
                do
                {
                    // if all the node have been processed in the layers then break
                    if (layers.Last().Count == 0) { break; }

                    // grab a unmatched node from the last layers
                    var node = layers.Last().First();

                    // find an augmenting path
                    var augmentingPath = Dfs(layers, node, layers.Count -1, matching, new List<Node>());

                    // if there are no paths to return then break
                    if (augmentingPath.Count == 0) { break; }

                    // remove the nodes in the augmenting paths from the layers
                    // TODO: perhaps could make a better solution using zip?
                    foreach (var layer in layers)
                    {
                        layer.RemoveWhere(layerNode => augmentingPath.Contains(layerNode));
                    }

                    // update the matching
                    matching.MergeAugmentingPath(augmentingPath);

                } while(true);
            } while(true);

            return matching;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 3, 4 } },
                    { 1, new List<int> { 3, 4, 5 } },
                    { 2, new List<int> { 4 } }
                }
            );

            HopcroftKarpMatching.Run(graph);
        }
    }

    public class BipartiteGraph
    {
        public List<Node> Left { get; set; }
        public List<Node> Right { get; set; }

        // Does not allow for orphan nodes on the right side
        public BipartiteGraph(Dictionary<int, List<int>> left)
        {
            // initialize lists
            Left = new List<Node>();
            Right = new List<Node>();

            foreach (var association in left)
            {
                var leftNode = new Node($"{association.Key}");
                Left.Add(leftNode);

                // link up the left and right nodes
                foreach (var rightNodeKey in association.Value)
                {
                    var rightNode = Right.All(node => node.Content != $"{rightNodeKey}") ?
                        new Node($"{rightNodeKey}") :
                        Right
                            .Where(node => node.Content == $"{rightNodeKey}")
                            .First();

                    leftNode.Connections.Add(rightNode);
                    rightNode.Connections.Add(leftNode);

                    Right.Add(rightNode);
                    Right = Right.Distinct().ToList();
                }
            }
        }

        public BipartiteGraph()
        {

        }
    }

    public class Node
    {
        public Node(string content)
        {
            Connections = new List<Node>();
            Content = content;
        }

        public List<Node> Connections { get; set; }

        public string Content { get; set; }
    }

    public class Matching
    {
        public Matching()
        {
            Pairs = new List<(Node, Node)>();
        }

        public List<(Node, Node)> Pairs { get; set; }

        public bool Contains(Node node)
        {
            return Pairs.Any(pair => pair.Item1 == node || pair.Item2 == node);
        }

        public bool HasPair(Node x, Node y)
        {

            return Pairs.Where(pair => pair.Contains(x) && pair.Contains(y)).ToList().Count > 0;
        }

        public void RemovePair(Node x, Node y)
        {
            Pairs.RemoveAll(pair => pair.Contains(x) && pair.Contains(y));
        }

        public void MergeAugmentingPath(List<Node> path)
        {
            // add augmenting pairs into the matching
            foreach (var augmentingPair in path.IntervalGroup(2))
            {
                Pairs.Add((augmentingPair[0], augmentingPair[1]));
            }

            // if this isn't a single depth path
            if (path.Count > 2)
            {
                // top and tail the list
                path.RemoveAt(0);
                path.RemoveAt(path.Count - 1);

                // remove all the redundant pairs
                foreach (var redundantPair in path.IntervalGroup(2))
                {
                    RemovePair(redundantPair[0], redundantPair[1]);
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool Contains<T>(this (T, T) tuple, T cmp) where T : class
        {
            return (tuple.Item1 == cmp || tuple.Item2 == cmp);
        }

        public static bool Contains<T>(this List<HashSet<T>> self, T cmp) where T: class
        {
            return self.Any(set => set.Contains(cmp));
        }

        public static List<T> Merge<T>(this List<T> list, List<T> other)
        {
            list.AddRange(other);
            return list;
        }

        // takes X number of elements from a list and removes them
        public static List<T> Take<T>(this List<T> list, int range)
        {
            // if the range is larger than the remaining list then just return whatever is left
            range = range > list.Count ? range - (range - list.Count) : range;

            var takenElements = list.GetRange(0, range);
            list.RemoveRange(0, range);
            return takenElements;
        }

        // implementation of interval group for deep clones
        // public static List<List<T>> IntervalGroup<T>(this List<T> list, int interval) where T: ICloneable
        // {
        //     var intervals = new List<List<T>>();

        //     List<T> clone = list.Select(element => (T) element.Clone()).ToList();

        //     while (clone.Count > 0)
        //     {
        //         intervals.Add(clone.Take(interval).ToList());
        //     }

        //     return intervals;
        // }

        public static List<List<T>> IntervalGroup<T>(this List<T> list, int interval)
        {
            var intervals = new List<List<T>>();

            List<T> clone = list.ToList();

            while (clone.Count > 0)
            {
                intervals.Add(clone.Take(interval).ToList());
            }

            return intervals;
        }
    }
}
