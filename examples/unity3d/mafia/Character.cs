// Character.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;
using BehaviorEngine.Personality;

public class Character : Person, IUnityEntity {

  public CharacterUnit<ConsoleAction> unit;

  public bool Destroy {
    get { return destroy; }
    set { destroy = value; }
  }
  bool destroy = false;

  public bool Print {
    get { return print; }
    set { print = value; }
  }
  bool print = true;

  public Character(
    string name,
    Dictionary<string, Attribute<float>.InitializeState>
      initFactors,
    Dictionary<string, BehaviorEngine.Attribute<float>.InitializeState>
      initProperties
  ) : base(name, initFactors, initProperties) {
    unit = new CharacterUnit<ConsoleAction>();
  }

  // Helper function
  public float GetAttributeState(IAttribute attribute) {
    return (GetAttribute(attribute) as NormalizedAttribute.Instance).State;
  }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    return Random.Range(0, 1f); // ! (placeholder)
  }

  /* Debug */

  public override void LogReaction(
    Interaction interaction, Entity host, IList<Effect> effects
  ) {
    if (print) base.LogReaction(interaction, host, effects);
  }

  public override void LogObservation(
    Interaction interaction, Entity host,
    ICollection<Entity> targets, IList<Effect> effects
  ) {
    if (print) base.LogObservation(interaction, host, targets, effects);
  }

  public override void LogPoll(
    Interaction choice, ICollection<Entity> targets, float highscore
  ) {
    if (print) base.LogPoll(choice, targets, highscore);
  }
}
