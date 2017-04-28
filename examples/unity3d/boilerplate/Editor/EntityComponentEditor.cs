// EntityComponentEditor.cs
// Created by Aaron C Gaudette on 12.04.17

using UnityEngine;
using UnityEditor;
using BehaviorEngine.Float;

[CustomEditor(typeof(EntityComponent)), CanEditMultipleObjects]
public class EntityComponentEditor : Editor {

  SerializedProperty debug, attributes;

  void OnEnable() {
    debug = serializedObject.FindProperty("debug");
    attributes = serializedObject.FindProperty("attributes");
  }

  public override void OnInspectorGUI() {
    serializedObject.Update();

    EditorGUILayout.PropertyField(debug);

    if (!serializedObject.isEditingMultipleObjects) {
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

        NormalizedAttribute prototype = component.instances[i].Prototype
          as NormalizedAttribute;

        if (prototype.transform != Transformations.Linear()) {
          EditorGUILayout.BeginHorizontal();
          EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.LabelField("  (Transformed)");
            NormalizedAttribute.TransformedInstance instance
              = component.instances[i]
              as NormalizedAttribute.TransformedInstance;
            EditorGUILayout.Slider(instance.TransformedState, 0, 1);

          EditorGUI.EndDisabledGroup();
          EditorGUILayout.EndHorizontal();
        }
      }
    }

    serializedObject.ApplyModifiedProperties();
  }
}
