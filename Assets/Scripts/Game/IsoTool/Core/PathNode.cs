using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ISO
{
    public class PathNode
    {

        //A*
        public int x = 0, z = 0;
        public float f = 0f, g = 0f, h = 0f;

        //
        public bool walkable = false;

        //
        public PathNode parent;


        public List<PathLink> links = null;

        //find path type
        internal int version = 1;

        public PathNode() { }


        public PathNode(int nodeX, int nodeY)
        {
            this.x = nodeX;
            this.z = nodeY;
        }

        public PathNode Clone()
        {
            var node = new PathNode(x, z);
            return node;
        }
    }

}