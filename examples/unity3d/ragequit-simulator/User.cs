// User.cs
// Created by Aaron C Gaudette on 10.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;

public partial class User : Entity, IDestroyable {

  public bool Destroy { get; set; }

  public User(string label) : base() {
    SetLabel(label);
    HookRenderer();
  }

  // Helper function
  public float GetAttributeState(IAttribute attribute) {
    return (GetAttribute(attribute) as NormalizedAttribute.Instance).State;
  }

  protected override IList<Effect> Reaction(
    Interaction interaction, IEntity host
  ) {
    if (
      interaction != Forum.quit
      && (Destroy || (host as IDestroyable).Destroy)
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

  protected override IList<Effect> Observation(
    Interaction interaction, IEntity host, ICollection<IEntity> targets
  ) {
    if (
      interaction != Forum.quit
      && (Destroy || (host as IDestroyable).Destroy)
    ) {
      return null;
    }

    if (interaction == Forum.start) {
      if ((host as User).GetAttributeState(Forum.anger) > .5f)
        return new List<Effect>(1){ Forum.incite };

      else if ((host as User).GetAttributeState(Forum.anger) > .25f)
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
    Interaction interaction, ICollection<IEntity> targets
  ) {
    if (Destroy) return 0;

    if (targets != null) {
      foreach (IEntity target in targets) {
        if ((target as IDestroyable).Destroy)
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
