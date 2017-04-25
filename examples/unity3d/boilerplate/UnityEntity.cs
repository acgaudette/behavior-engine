// UnityEntity.cs
// Created by Aaron C Gaudette on 04.09.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;

public abstract class UnityEntity : Entity {

  public bool destroy = false;

  public float GetAttributeState(IAttribute attribute) {
    return (GetAttribute(attribute) as NormalizedAttribute.Instance).State;
  }

  /* Debug */

  public bool print = false;

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
