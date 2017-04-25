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
    if (reference == null) return;

    attributes.Clear();
    foreach (IAttribute attribute in reference.attributes)
      attributes.Add(attribute.GetLabel());

    effects.Clear();
    foreach (Effect effect in reference.effects)
      effects.Add(effect.GetLabel());

    interactions.Clear();
    foreach (Interaction interaction in reference.interactions)
      interactions.Add(interaction.GetLabel());
  }
}
