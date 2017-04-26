// Character.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;
using BehaviorEngine.Personality;

public class Character : Person, IUnityEntity {

  public CharacterUnit<ConsoleAction> unit;

  public Character(string name) : base(name) {
    unit = new CharacterUnit<ConsoleAction>();
  }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    return Random.Range(0, 1f); // ! (placeholder)
  }

  /* UnityEntity */

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

  // Helper function
  public float GetAttributeState(IAttribute attribute) {
    return (GetAttribute(attribute) as NormalizedAttribute.Instance).State;
  }

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
