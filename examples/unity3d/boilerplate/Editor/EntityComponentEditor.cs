// EntityComponentEditor.cs
// Created by Aaron C Gaudette on 12.04.17

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityComponent))]
public class EntityComponentEditor : Editor {

  SerializedProperty debug, attributes;

  void OnEnable() {
    debug = serializedObject.FindProperty("debug");
    attributes = serializedObject.FindProperty("attributes");
  }

  public override void OnInspectorGUI() {
    serializedObject.Update();

    EditorGUILayout.PropertyField(debug);

    GUIStyle bold = new GUIStyle(EditorStyles.boldLabel);
    EditorGUILayout.LabelField(
      "Attributes (" + attributes.arraySize + ")", bold
    );

    EditorGUI.indentLevel++;
    EditorGUIUtility.labelWidth = 64;

    EntityComponent component = serializedObject.targetObject
      as EntityComponent;

    for (int i = 0; i < attributes.arraySize; ++i) {
      SerializedProperty label
        = attributes.GetArrayElementAtIndex(i).FindPropertyRelative("label");
      SerializedProperty state
        = attributes.GetArrayElementAtIndex(i).FindPropertyRelative("state");

      EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label.stringValue);
        component.instances[i].State
          = EditorGUILayout.Slider(state.floatValue, 0, 1);
      EditorGUILayout.EndHorizontal();
    }

    serializedObject.ApplyModifiedProperties();
  }
}
