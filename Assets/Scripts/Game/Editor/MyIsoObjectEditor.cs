using UnityEngine;
using System.Collections;
using UnityEditor;
using ISO;

[CustomEditor(typeof(MyIsoObject))]
class MyIsoObjectEditor:IsoObjectEditor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
	}
}
