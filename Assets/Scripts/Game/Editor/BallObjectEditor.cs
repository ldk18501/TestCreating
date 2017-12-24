﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using ISO;

[CustomEditor(typeof(Ball))]
class BallObjectEditor:IsoObjectEditor
{
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"), true);
		serializedObject.ApplyModifiedProperties();

		base.OnInspectorGUI ();
	}
}
