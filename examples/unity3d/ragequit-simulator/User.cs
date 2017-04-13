// User.cs
// Created by Aaron C Gaudette on 10.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public partial class User : UnityEntity {

  public User(string label) : base(label) { }

  protected override IList<Effect> GetReaction(
    Interaction interaction, Entity host
  ) {
    if (
      interaction != Forum.quit
      && (destroy || (host as UnityEntity).destroy)
    ) {
      return null;
    }

    if (host == this)
      return null;

    if (interaction == Forum.flame) {
      if (GetAttributeState(Forum.trollFactor) > .5f)
        return new List<Effect>(1){ Forum.annoy };
      else
        return new List<Effect>(1){ Forum.incite };
    }

    return null;
  }

  protected override IList<Effect> GetObservation(
    Interaction interaction, Entity host, ICollection<Entity> targets
  ) {
    if (
      interaction != Forum.quit
      && (destroy || (host as UnityEntity).destroy)
    ) {
      return null;
    }

    if (interaction == Forum.start) {
      if ((host as UnityEntity).GetAttributeState(Forum.anger) > .5f)
        return new List<Effect>(1){ Forum.incite };

      else if ((host as UnityEntity).GetAttributeState(Forum.anger) > .25f)
        return new List<Effect>(1){ Forum.annoy };

      else
        return new List<Effect>(1){ Forum.calm };
    }

    else if (interaction == Forum.flame) {
      return null;
    }

    else if (interaction == Forum.quit) {
      if (GetAttributeState(Forum.trollFactor) > .5f)
        return new List<Effect>(1){ Forum.calm };
      else
        return new List<Effect>(1){ Forum.annoy };
    }

    return null;
  }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    if (destroy) return 0;

    if (targets != null) {
      foreach (Entity target in targets) {
        if ((target as UnityEntity).destroy)
          return 0;
      }
    }

    if (interaction == Forum.quit)
      return GetAttributeState(Forum.anger) > .95f ? 1 : 0;

    float flameChance = .5f * (
      GetAttributeState(Forum.trollFactor) + GetAttributeState(Forum.anger)
    );

    float chance = interaction == Forum.flame ? flameChance : 1 - flameChance;

    return Random.Range(0, 1f) < chance ?
      1 : interaction == Forum.start ? .2f : .1f;
  }
}
