using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ISO
{
	[CustomEditor(typeof(IsoObject))]
	public class IsoObjectEditor : Editor {

		private IsoObject _isoObj;

		private int _nodeX,_nodeZ;
		private int _spanX,_spanZ;
		private bool _rotateX;

		void OnEnable(){
			_isoObj = target as IsoObject;
			_nodeX = _isoObj.nodeX;
			_nodeZ = _isoObj.nodeZ;
			_spanX = _isoObj.spanX;
			_spanZ = _isoObj.spanZ;
			_rotateX = serializedObject.FindProperty("m_isRotated").boolValue;
		}

		public override void OnInspectorGUI ()
		{
			if(_isoObj.transform.parent!=null && _isoObj.world==null)
			{
				_isoObj.world = _isoObj.transform.GetComponentInParent<IsoWorld>();
			}

			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("showGrid"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosColor"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_nodeX"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_nodeZ"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("spanX"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("spanZ"), true);
			serializedObject.ApplyModifiedProperties();

			if(!Application.isPlaying)
			{
				if(_isoObj.nodeX!=_nodeX || _isoObj.nodeZ!=_nodeZ){
					_nodeX = _isoObj.nodeX;
					_nodeZ = _isoObj.nodeZ;
					_isoObj.SetNodePosition(_nodeX,_nodeZ);
				}

				if(_isoObj.spanX!=_spanX || _isoObj.spanZ!=_spanZ){
					_spanX = _isoObj.spanX;
					_spanZ = _isoObj.spanZ;
					HandleUtility.Repaint();
				}
			}


			EditorGUILayout.Space();
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_isRotated"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("world"), true);
			serializedObject.ApplyModifiedProperties();

			if(serializedObject.FindProperty("m_isRotated").boolValue!=_rotateX){
				_rotateX = serializedObject.FindProperty("m_isRotated").boolValue; 
				_isoObj.RotateX(_rotateX);
			}

			if(!_isPrefab()){
				EditorGUILayout.Space();
				if(GUILayout.Button("Align To Grid",GUILayout.Height(20))){
					if(_isoObj.world){
						Vector2 nodePos = _isoObj.world.LocalPosToGridPos(_isoObj.transform.localPosition.x,_isoObj.transform.localPosition.y);
						_nodeX = (int)nodePos.x;
						_nodeZ = (int)nodePos.y;
						_isoObj.SetNodePosition(_nodeX,_nodeZ);
						if (!Application.isPlaying && !_isPrefab()){
							serializedObject.ApplyModifiedProperties();
							EditorUtility.SetDirty(_isoObj);
							EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
						}
					}
				}
			}
		}

		private bool _isPrefab(){
			return PrefabUtility.GetPrefabParent(_isoObj.gameObject) == null 
				&& PrefabUtility.GetPrefabObject(_isoObj.gameObject) != null;
		}
	}
}