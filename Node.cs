using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Node
    {
        public List<Node> neighbors;
        public int Value { get; set; }
        public Node(int value)
        {
            Value = value;
            neighbors = new List<Node>();
        }
    }
}
