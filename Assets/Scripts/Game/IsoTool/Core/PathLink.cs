namespace ISO
{
	public class PathLink {

		public PathNode node = null ;
		public float cost = 0;

		public PathLink( PathNode node ,float cost){
			this.node = node;
			this.cost = cost;
		}
	}
}