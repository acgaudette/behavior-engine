// BrainRepositoryComponent.cs
// Created by Aaron C Gaudette on 26.04.17

using System.Collections.Generic;
using BehaviorEngine;
using BehaviorEngine.Personality;

public class BrainRepoComponent : RepoComponent {

  public List<string> effects = new List<string>();
  public List<string> actions = new List<string>();

  protected override void UpdateDisplay() {
    effects.Clear();
    foreach (Effect effect in (reference as BrainRepository).Effects)
      effects.Add(effect.GetDebugLabel());

    actions.Clear();
    foreach (string key in (reference as BrainRepository).actions.Keys)
      actions.Add(key);
  }
}
