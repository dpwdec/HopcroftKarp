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
                    layers.Add(nextLayer);
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
                .ToList();
                
            }
            // otherwise just return an empty list, this nullifies any path that was store so far on this search branch
            // so that a lot of empty lists are flattened leading to only the correct path remaining
            else
            {
                return new List<Node>();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
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

        public Matching MergeAugmentingPath(List<Node> path)
        {
            return new Matching();
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
            var takenElements = list.GetRange(0, range);
            list.RemoveRange(0, range);
            return takenElements;
        }
    }
}
