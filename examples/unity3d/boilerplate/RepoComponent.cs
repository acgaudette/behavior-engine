// RepoComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class RepoComponent : MonoBehaviour {

  public IRepository reference;

  // Display
  public List<string> attributes = new List<string>();
  //public List<string> effects = new List<string>();
  public List<string> interactions = new List<string>();

  void Update() {
    if (reference == null) return;

    attributes.Clear();
    if (reference.AttributePrototypes != null) {
      foreach (IAttribute attribute in reference.AttributePrototypes)
        attributes.Add(attribute.GetLabel());
    }

    /*
    effects.Clear();
    foreach (Effect effect in reference.effects)
      effects.Add(effect.GetLabel());
    */

    interactions.Clear();
    if (reference.Interactions != null ) {
      foreach (Interaction interaction in reference.Interactions)
        interactions.Add(interaction.GetLabel());
    }
  }
}
