using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Htw.Cave.Utilites;

namespace Htw.Cave.Joycons
{
	[CustomEditor(typeof(JoyconBinding))]
	public class JoyconBindingEditor : Editor
	{
		private JoyconBinding me;
		private SerializedProperty schemesProperty;
		private ReorderableList schemesList;
		private int size;

		public void OnEnable()
		{
			this.me = (JoyconBinding)base.target;
			this.schemesProperty = base.serializedObject.FindProperty("schemes");
			this.schemesList = new ReorderableList(serializedObject, this.schemesProperty, true, true, true, true);
			this.schemesList.drawHeaderCallback = (Rect rect) => {
				Rect column = rect;
				column.width *= 0.3333f;

				EditorGUI.LabelField(column, "Name");

				column.x += column.width;

				EditorGUI.LabelField(column, "Type");

				column.x += column.width;

				EditorGUI.LabelField(column, "Button");
			};
			this.schemesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				SerializedProperty element = this.schemesList.serializedProperty.GetArrayElementAtIndex(index);
				SerializedProperty name = element.FindPropertyRelative("name");
				SerializedProperty axis = element.FindPropertyRelative("axis");
				SerializedProperty button = element.FindPropertyRelative("button");

				Rect column = rect;
				column.height = rect.height * 0.75f;
				column.width *= 0.3333f;

				EditorGUI.PropertyField(column, name, new GUIContent());

				column.x += column.width;

				EditorGUI.PropertyField(column, axis, new GUIContent());

				column.x += column.width;

				using(new EditorGUI.DisabledScope(axis.intValue > 0))
					EditorGUI.PropertyField(column, button, new GUIContent());
			};

			this.size = this.schemesProperty.arraySize;
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			serializedObject.Update();

			EditorLayoutUtility.ScriptField(base.serializedObject);

			this.schemesList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
			EditorGUI.EndChangeCheck();
		}
	}
}
