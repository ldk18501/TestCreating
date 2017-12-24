using UnityEngine;
using System.Collections;
using ISO;

public class MyIsoObject : IsoObject {

	public override void RotateX (bool value)
	{
		base.RotateX (value);
		Vector3 rot = value? new Vector3(0,180,0):Vector3.zero;
		transform.localEulerAngles = rot;
	}
}