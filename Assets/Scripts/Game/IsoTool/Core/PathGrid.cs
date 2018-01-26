using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ISO
{
    public class PathGrid
    {

        private int m_gridX = 0;
        private int m_gridZ = 0;

        private int m_type = 0;

        private readonly float m_straightCost = 1f;
        private readonly float m_diagCost = Mathf.Sqrt(2f);

        private List<List<PathNode>> m_nodes = null; //all road point
        private List<PathNode> m_walkableNodes = null;//walkable road point


        public int type
        {
            get { return m_type; }
        }
        public int gridX
        {
            get { return m_gridX; }
        }
        public int gridZ
        {
            get { return m_gridZ; }
        }


        public PathGrid() { }

        public PathGrid(int gridX, int gridZ)
        {
            m_nodes = new List<List<PathNode>>();
            this.m_gridX = gridX;
            this.m_gridZ = gridZ;
            for (int i = 0; i < gridX; ++i)
            {
                List<PathNode> line = new List<PathNode>();
                for (int j = 0; j < gridZ; ++j)
                {
                    PathNode node = new PathNode(i, j);
                    line.Add(node);
                }
                m_nodes.Add(line);
            }
        }

        public void SetAllWalkable(bool value)
        {
            for (int i = 0; i < m_gridX; ++i)
            {
                for (int j = 0; j < m_gridZ; ++j)
                {
                    m_nodes[i][j].walkable = value;
                }
            }
        }

        public void ChangeSize(int gridX, int gridZ)
        {
            if (m_gridX != gridX || m_gridZ != gridZ)
            {
                List<List<PathNode>> nodes = new List<List<PathNode>>();
                for (int i = 0; i < gridX; i++)
                {
                    List<PathNode> v = new List<PathNode>();
                    for (int j = 0; j < gridZ; j++)
                    {
                        if (i < m_gridX && j < m_gridZ)
                        {
                            v[j] = m_nodes[i][j];
                        }
                        else
                        {
                            PathNode node = new PathNode(i, j);
                            v[j] = node;
                        }
                    }
                    nodes.Add(v);
                }
                //clear
                for (int i = 0; i < m_gridX; i++)
                {
                    for (int j = 0; j < m_gridZ; j++)
                    {
                        if (i >= gridX || j >= gridZ)
                        {
                            m_nodes[i][j] = null;
                        }
                    }
                }
                m_nodes.Clear();
                m_gridX = gridX;
                m_gridZ = gridZ;
                m_nodes = nodes;
            }
        }

        public List<PathNode> GetNodesByWalkable(bool walkable)
        {
            m_walkableNodes = new List<PathNode>();
            for (int i = 0; i < m_gridX; i++)
            {
                for (int j = 0; j < m_gridZ; j++)
                {
                    if (m_nodes[i][j].walkable == walkable)
                    {
                        m_walkableNodes.Add(m_nodes[i][j]);
                    }
                }
            }
            return m_walkableNodes;
        }

        /**
		 *
		 * @param	node
		 * @param	type	0 = 8director,  1 = 4direction,  2 = jump chess
		 */
        private void InitNodeLink(PathNode node, int type)
        {
            int startX = Mathf.Max(0, node.x - 1);
            int endX = Mathf.Min(m_gridX - 1, node.x + 1);
            int startZ = Mathf.Max(0, node.z - 1);
            int endZ = Mathf.Min(m_gridZ - 1, node.z + 1);

            if (node.links != null)
            {
                node.links.Clear();
            }
            else
            {
                node.links = new List<PathLink>();
            }

            for (int i = startX; i <= endX; ++i)
            {
                for (int j = startZ; j <= endZ; ++j)
                {
                    PathNode test = GetNode(i, j);
                    if (test == node || !test.walkable)
                    {
                        continue;
                    }
                    if (type != 2 && i != node.x && j != node.z)
                    {
                        PathNode test2 = GetNode(node.x, j);
                        if (!test2.walkable)
                        {
                            continue;
                        }
                        test2 = GetNode(i, node.z);
                        if (!test2.walkable)
                        {
                            continue;
                        }
                    }
                    float cost = m_straightCost;
                    if (!((node.x == test.x) || (node.z == test.z)))
                    {
                        if (type == 1)
                        {
                            continue;
                        }
                        if (type == 2 && (node.x - test.x) * (node.z - test.z) == 1)
                        {
                            continue;
                        }
                        if (type == 2)
                        {
                            cost = m_straightCost;
                        }
                        else
                        {
                            cost = m_diagCost;
                        }
                    }
                    node.links.Add(new PathLink(test, cost));
                }
            }
        }

        public bool CheckInGrid(int nodeX, int nodeZ)
        {
            if (nodeX < 0 || nodeZ < 0 || nodeX >= m_gridX || nodeZ >= m_gridZ) return false;
            return true;
        }

        public PathNode GetNode(int nodeX, int nodeZ)
        {
            return m_nodes[nodeX][nodeZ];
        }

        public void SetWalkable(int nodeX, int nodeZ, bool value)
        {
            m_nodes[nodeX][nodeZ].walkable = value;
        }

        /**
		 *
		 * @param	type	0 = 8director,  1 = 4direction,  2 = jump chess
		 */
        public void CalculateLinks(int type = 0)
        {
            m_type = type;
            for (int i = 0; i < m_gridX; ++i)
            {
                for (int j = 0; j < m_gridZ; ++j)
                {
                    InitNodeLink(m_nodes[i][j], m_type);
                }
            }
        }


        public PathGrid Clone()
        {
            PathGrid gird = new PathGrid(this.m_gridX, this.m_gridZ);
            for (int i = 0; i < m_gridX; ++i)
            {
                for (int j = 0; j < m_gridZ; ++j)
                {
                    gird.GetNode(i, j).walkable = this.GetNode(i, j).walkable;
                }
            }
            return gird;
        }

        public void Destroy()
        {
            if (m_nodes != null)
            {
                m_nodes.Clear();
            }
            if (m_walkableNodes != null)
            {
                m_walkableNodes.Clear();
            }
        }
    }
}