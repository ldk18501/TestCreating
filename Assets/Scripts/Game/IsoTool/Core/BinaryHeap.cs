using System.Collections.Generic;
using System.Collections;

namespace ISO
{
	public delegate bool JustMinFun(PathNode x,PathNode y);

	public class BinaryHeap
	{

		public List<PathNode> a = new List<PathNode>();

		private JustMinFun m_CalFun = null;

		public BinaryHeap(JustMinFun justMinFun)
		{
			this.m_CalFun = justMinFun;
		}

		public void Ins(PathNode value) {
			int p = a.Count;
			a.Add(value);
			int pp = p >> 1;
			while (p > 1 && m_CalFun(a[p], a[pp])){
				PathNode temp = a[p];
				a[p] = a[pp];
				a[pp] = temp;
				p = pp;
				pp = p >> 1;
			}
		}

		public PathNode Pop() 
		{
			PathNode min = a[1];
			a[1] = a[a.Count - 1];
			a.RemoveAt(a.Count-1);
			int p = 1;
			int l = a.Count;
			int sp1 = p << 1;
			int sp2 = sp1 + 1;
			while (sp1 < l){
				int minp = sp1;
				if (sp2 < l){
					minp = m_CalFun(a[sp2], a[sp1]) ? sp2 : sp1;
				}
				if (m_CalFun(a[minp], a[p])){
					PathNode temp = a[p];
					a[p] = a[minp];
					a[minp] = temp;
					p = minp;
					sp1 = p << 1;
					sp2 = sp1 + 1;
				} else {
					break;
				}
			}
			return min;
		}

		public void Clear(){
			a.Clear();
		}
	}
}