// ClassComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class ClassComponent : MonoBehaviour {

  public Class reference;

  // Display
  public List<string> attributes = new List<string>();
  public List<string> effects = new List<string>();
  public List<string> interactions = new List<string>();

  void Update() {
    if (reference == null) {
      Debug.LogWarning("ClassComponent: Reference is null!");
      return;
    }

    // Display

    attributes.Clear();
    foreach (Attribute attribute in reference.GetAttributes())
      attributes.Add(attribute.ToString());

    effects.Clear();
    foreach (Effect effect in reference.effects) {
      effects.Add(
        effect is ILabeled ? (effect as ILabeled).ToString() : "Unlabeled"
      );
    }

    interactions.Clear();
    foreach (Interaction interaction in reference.interactions) {
      interactions.Add(
        interaction is ILabeled ? (interaction as ILabeled).ToString() : "Unlabeled"
      );
    }
  }
}
