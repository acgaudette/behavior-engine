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

  // Display (fallback, non-editable)
  public List<UnityAttributeRenderer> attributes
    = new List<UnityAttributeRenderer>();

  // Internal (allows for modification of Attributes in the editor)
  public List<UnityAttribute.Instance> instances
    = new List<UnityAttribute.Instance>();

  ICollection<IAttributeInstance> lastInstances;
  int lastCount = 0;

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

    ICollection<IAttributeInstance> current = reference.GetAttributes();

    if (lastInstances != current || lastCount != current.Count) {
      GenerateInstances();
      lastInstances = current;
      lastCount = current.Count;
    }
  }

  void GenerateInstances() {
    instances.Clear();

    foreach (IAttributeInstance instance in reference.GetAttributes()) {
      UnityAttribute.Instance i = instance as UnityAttribute.Instance;
      instances.Add(i == null ? null : i); // Synchronize with attribute list
    }
  }
}
