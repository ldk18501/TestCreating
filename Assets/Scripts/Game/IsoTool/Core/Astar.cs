using System.Collections;
using System.Collections.Generic;
using System;

namespace ISO
{

	public class Astar
	{
		public delegate float Func(PathNode node); 


		private BinaryHeap m_open;
		private PathGrid m_grid;
		private PathNode m_endNode;
		private PathNode m_startNode;
		private List<PathNode> m_path;
		private float m_straightCost = 1.0f;
		private float m_diagCost = (float)Math.Sqrt(2f);
		private int nowversion = 1;

		public Func heuristic;
		private readonly float TwoOneTwoZero = (float)(2f * Math.Cos(Math.PI / 3f));


		public List<PathNode> path{
			get{ return m_path;}
		}

		public Astar(PathGrid grid){
			this.m_grid = grid;
			heuristic = Euclidian2;
			m_open = new BinaryHeap(JustMin);
		}

		private bool JustMin(PathNode x, PathNode y) {
			return x.f < y.f;
		}

		public bool FindPath(int startNodeX,int startNodeZ,int endNodeX,int endNodeZ) {
			++nowversion;
			m_startNode = m_grid.GetNode(startNodeX,startNodeZ);
			m_endNode = m_grid.GetNode(endNodeX,endNodeZ);
			m_startNode.g = 0;
			m_open.Clear();
			return Search();
		}

		private bool Search() {
			var node = m_startNode;
			node.version = nowversion;
			while (node != m_endNode){
				var len = node.links.Count;
				for (int i = 0; i < len; ++i){
					var test = node.links[i].node;
					var cost = node.links[i].cost;
					var g = node.g + cost;
					var h = heuristic(test);
					var f = g + h;
					if (test.version == nowversion){
						if (test.f > f){
							test.f = f;
							test.g = g;
							test.h = h;
							test.parent = node;
						}
					} else {
						test.f = f;
						test.g = g;
						test.h = h;
						test.parent = node;
						m_open.Ins(test);
						test.version = nowversion;
					}

				}
				if (m_open.a.Count <= 1){
					return false;
				}
				node = m_open.Pop();
			}
			BuildPath();
			return true;
		}

		private void BuildPath() {
			if(m_path==null){
				m_path = new List<PathNode>();
			}else{
				m_path.Clear();
			}
			PathNode node = m_endNode;
			m_path.Add(node);
			while (node != m_startNode){
				node = node.parent;
				m_path.Insert(0,node);
			}
		}

		public float Manhattan(PathNode node) {
			return Math.Abs(node.x - m_endNode.x) + Math.Abs(node.z - m_endNode.z);
		}

		public float Manhattan2(PathNode node) {
			float dx = Math.Abs(node.x - m_endNode.x);
			float dy = Math.Abs(node.z - m_endNode.z);
			return dx + dy + Math.Abs(dx - dy) *0.001f ;
		}

		public float Euclidian(PathNode node) {
			float dx = node.x - m_endNode.x;
			float dy = node.z - m_endNode.z;
			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

		public float ChineseCheckersEuclidian2(PathNode node) {
			int y = (int)(node.z / TwoOneTwoZero);
			int x = (int)(node.x + node.z * 0.5f)  ;
			var dx = x - m_endNode.x -m_endNode.z * 0.5f ;
			var dy = y - m_endNode.z / TwoOneTwoZero;
			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

		public float Euclidian2(PathNode node) {
			float dx = node.x - m_endNode.x;
			float dy = node.z - m_endNode.z;
			return dx * dx + dy * dy;
		}

		public float Diagonal(PathNode node) {
			float dx = Math.Abs(node.x - m_endNode.x);
			float dy = Math.Abs(node.z - m_endNode.z);
			float diag = Math.Min(dx, dy);
			float straight = dx + dy;
			return m_diagCost * diag + m_straightCost * (straight - 2f * diag);
		}
	}
}