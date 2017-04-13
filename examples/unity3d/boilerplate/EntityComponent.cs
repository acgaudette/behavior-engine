// EntityComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class EntityComponent : MonoBehaviour {

  public bool debug = true;
  public UnityEntity reference;

  [System.Serializable]
  public class UnityAttributeRenderer {
    [HideInInspector] public string label;
    [Range(0, 1)] public float state;

    public UnityAttributeRenderer(string label, float state) {
      this.label = label;
      this.state = state;
    }
  }

  // Display
  public List<UnityAttributeRenderer> attributes = new List<UnityAttributeRenderer>();

  void Update() {
    if (reference == null) return;

    reference.Print = debug;
    attributes.Clear();

    // Display
    foreach (IAttributeInstance instance in reference.GetAttributes()) {
      UnityAttribute.Instance i = instance as UnityAttribute.Instance;

      attributes.Add(new UnityAttributeRenderer(
        i == null ? "?" : i.Prototype.ToString(),
        i == null ? 0 : i.State
      ));
    }
  }
}
