// EntityComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public class EntityComponent : MonoBehaviour {

  public bool debug = true;
  public UnityEntity reference;

  [System.Serializable]
  public class AttributeRenderer {
    [HideInInspector] public string label;
    [Range(0, 1)] public float state;

    public AttributeRenderer(string label, float state) {
      this.label = label;
      this.state = state;
    }
  }

  // Display
  public List<AttributeRenderer> attributes = new List<AttributeRenderer>();

  void Update() {
    reference.Print = debug;

    if (reference == null) {
      Debug.LogWarning("EntityComponent: Reference is null!");
      return;
    }

    attributes.Clear();

    // Display
    foreach (Attribute attribute in reference.GetAttributes()) {
      attributes.Add(new AttributeRenderer(
        attribute.GetArchetype().ToString(), attribute.GetState()
      ));
    }
  }
}
