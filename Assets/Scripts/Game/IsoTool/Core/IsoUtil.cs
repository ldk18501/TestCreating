using UnityEngine;
using System.Collections;

namespace ISO
{
	public class IsoUtil {

		public static readonly float Y_CORRECT = Mathf.Cos(-Mathf.PI / 6f) * Mathf.Sqrt(2f);

		//w/h
		public static float W_H_RATE = 0.75f ;


		public static Vector2 IsoPosToLocalPos(float px,float py ,float pz){
			float screenX = px - pz;
			float screenY = py * Y_CORRECT + (px + pz) * W_H_RATE;
			return new Vector2(screenX*0.01f, -screenY*0.01f);
		}

//		public static Vector2 LocalPosToIsoPos(float px,float py )
//		{
//			px*=100f;
//			py*=100f;
//			float zpos=(-py-px*W_H_RATE)/(2*W_H_RATE);
//			float xpos = px+zpos;
//			float ypos = 0;
//			return new Vector2(xpos,ypos);
//		}

		public static Vector2 LocalPosToIsoGrid(float size,float px ,float py)
		{
			px*=100f;
			py*=100f;
			float zpos = ( -py-px*W_H_RATE )/(2*W_H_RATE);
			float xpos = px + zpos;

			int col = Mathf.FloorToInt ( xpos / size );
			int row = Mathf.FloorToInt ( zpos / size );
			return new Vector2(col,row);
		}
	}
}