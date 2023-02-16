using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIceOneGraph
{
    internal class Edge
    {
        public Vertex From { get; set; }
        public Vertex To { get; set; }
        public int Weight { get; set; } 
        public Edge(Vertex from, Vertex to) 
        {
            From = from;
            To = to;
            Weight = 0;
        }

        public void addWeight(int weight)
        {
            Weight = weight;
        }
    }
}
