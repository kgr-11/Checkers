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
        public int Number { get; init; }
        public List<Node> neighbors;
        public int Value { get; set; }
        public Node(int value, int number)
        {
            Number = number;
            Value = value;
        }
    }
}
