// RepoComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class RepoComponent : MonoBehaviour {

  public IRepository reference;

  // Display
  public List<string> attributes = new List<string>();
  public List<string> interactions = new List<string>();

  void Update() {
    if (reference == null) return;

    attributes.Clear();
    IEnumerable<IAttribute> a = reference.AttributePrototypes;
    if (a != null) {
      foreach (IAttribute attribute in a)
        attributes.Add(attribute.GetDebugLabel());
    }

    interactions.Clear();
    ICollection<Interaction> i = reference.Interactions;
    if (i != null) {
      foreach (Interaction interaction in i)
        interactions.Add(interaction.GetDebugLabel());
    }

    UpdateDisplay();
  }

  // Override in derived classes
  protected virtual void UpdateDisplay() { }
}
