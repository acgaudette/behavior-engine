// User.cs
// Created by Aaron C Gaudette on 10.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public partial class User : UnityEntity {

  public User(string label) : base(label) { }

  protected override List<Effect> GetReaction(Interaction interaction, Entity host) {
    if ((destroy || (host as UnityEntity).destroy) && interaction != Forum.quit)
      return null;

    if (host == this)
      return null;

    if (interaction == Forum.flame) {
      if (GetAttributeState(Forum.trollFactor) > .5f)
        return new List<Effect>(){ Forum.annoy };
      else
        return new List<Effect>(){ Forum.incite };
    }

    return null;
  }

  protected override List<Effect> GetObservation(
    Interaction interaction, Entity host, List<Entity> targets
  ) {
    if ((destroy || (host as UnityEntity).destroy) && interaction != Forum.quit)
      return null;

    if (interaction == Forum.start) {
      if ((host as UnityEntity).GetAttributeState(Forum.anger) > .5f)
        return new List<Effect>(){ Forum.incite };

      else if ((host as UnityEntity).GetAttributeState(Forum.anger) > .25f)
        return new List<Effect>(){ Forum.annoy };

      else
        return new List<Effect>(){ Forum.calm };
    }

    else if (interaction == Forum.flame) {
      return null;
    }

    else if (interaction == Forum.quit) {
      if (GetAttributeState(Forum.trollFactor) > .5f)
        return new List<Effect>(){ Forum.calm };
      else
        return new List<Effect>(){ Forum.annoy };
    }

    return null;
  }

  protected override float Score(Interaction interaction, List<Entity> targets) {
    if (destroy || targets != null && (targets[0] as UnityEntity).destroy)
      return 0;

    if (interaction == Forum.quit)
      return GetAttributeState(Forum.anger) > .95f ? 1 : 0;

    float flameChance = .5f * (
      GetAttributeState(Forum.trollFactor) + GetAttributeState(Forum.anger)
    );

    float chance = interaction == Forum.flame ? flameChance : 1 - flameChance;

    return Random.Range(0, 1f) < chance ? 1 : interaction == Forum.start ? .2f : .1f;
  }
}
