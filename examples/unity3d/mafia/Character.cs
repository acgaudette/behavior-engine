// Character.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;

public class Character : Person {

  public CharacterUnit<ConsoleAction> unit;

  public Character(
    string name,
    Dictionary<string, Attribute<float>.InitializeState>
      initFactors,
    Dictionary<string, BehaviorEngine.Attribute<float>.InitializeState>
      initProperties
  ) : base(name, initFactors, initProperties) {
    unit = new CharacterUnit<ConsoleAction>();
  }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    return Random.Range(0, 1f); // ! (placeholder)
  }
}
