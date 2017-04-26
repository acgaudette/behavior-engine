// SimpleRepoComponent.cs
// Created by Aaron C Gaudette on 26.04.17

using System.Collections.Generic;
using BehaviorEngine;

public class SimpleRepoComponent : RepoComponent {

  // Display
  public List<string> effects = new List<string>();

  protected override void UpdateDisplay() {
    effects.Clear();
    foreach (Effect effect in (reference as SimpleRepo).effects)
      effects.Add(effect.GetDebugLabel());
  }
}
