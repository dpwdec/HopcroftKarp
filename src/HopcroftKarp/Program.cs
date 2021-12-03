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
                    .Where((node) => !matching.Contains(node))
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
            return Pairs.Any((pair) => pair.Item1 == node || pair.Item2 == node);
        }

        public bool HasPair(Node x, Node y)
        {

            return Pairs.Where((pair) => pair.Contains(x) && pair.Contains(y)).ToList().Count > 0;
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
    }
}
