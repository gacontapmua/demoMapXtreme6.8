using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Graph
    {
        public class Node
        {
            public double X, Y;
            public int ID;
            public List<int> ListAdjNode;
            public double G;
            public double H;
            public double F;
            public bool daxet;

            public Node(int id, double cordX, double cordY, double g, double h, double f, bool dx)
            {
                ID = id;
                X = cordX; Y = cordY;
                ListAdjNode = new List<int>();
                G = g;
                H = h;
                F = f;
                daxet = dx;

            }
        }
        //-----------------------------------------------------
        //-----------------------------------------------------
        //-----------------------------------------------------
        public List<Node> ListNodes;
        public Graph()
        {
            ListNodes = new List<Node>();
        }
    }

}