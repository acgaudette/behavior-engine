// EntityComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;

public class EntityComponent : MonoBehaviour {

  public bool debug = false;
  public UnityEntity reference;

  [System.Serializable]
  public class NormalizedAttributeRenderer {
    [HideInInspector] public string label;
    [Range(0, 1)] public float state;

    public NormalizedAttributeRenderer(string label, float state) {
      this.label = label;
      this.state = state;
    }
  }

  // Display (fallback, non-editable)
  public List<NormalizedAttributeRenderer> attributes
    = new List<NormalizedAttributeRenderer>();

  // Internal (allows for modification of Attributes in the editor)
  public List<NormalizedAttribute.Instance> instances
    = new List<NormalizedAttribute.Instance>();

  ICollection<IAttributeInstance> lastInstances;
  int lastCount = 0;

  void Update() {
    if (reference == null) return;

    reference.print = debug;
    attributes.Clear();

    // Display
    foreach (IAttributeInstance instance in reference.GetAttributes()) {
      NormalizedAttribute.Instance i = instance as NormalizedAttribute.Instance;

      attributes.Add(new NormalizedAttributeRenderer(
        i == null ? "?" : i.GetLabel(),
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
      NormalizedAttribute.Instance i = instance as NormalizedAttribute.Instance;
      instances.Add(i == null ? null : i); // Synchronize with attribute list
    }
  }
}
