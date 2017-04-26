// EntityTemplate.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using BehaviorEngine;

// If you extend from UnityEntity, you'll get labeling and
// automatic debugging within the Unity editor (go take a look at that class)
public class EntityTemplate : Entity {

  // Get the possible targets to interact with
  protected override ICollection<IEntity> Targets(Interaction interaction) {
    // By default, target everything in the root Universe
    return Universe.root == null ? null : Universe.root.entities;
  }

  // Return an IList (one or more) of effects on reaction
  // to an Interaction with a host Entity
  protected override IList<Effect> Reaction(
    Interaction interaction, IEntity host
  ) {
    // Is this an interaction with the self?
    if (host == this) {
      return null;
    }

    // Compare a subset of the host's Attributes to a subset of this Entity's

    return null; // Does nothing by default
  }

  // Return an IList (one or more) of effects on observation
  // of an Interaction with a host Entity and target Entities
  protected override IList<Effect> Observation(
    Interaction interaction, IEntity host, ICollection<IEntity> targets
  ) {
    return null; // Does nothing by default
  }

  // Given an interaction and its target(s), return a value;
  // higher values are more likely to be performed
  protected override float Score(
    Interaction interaction, ICollection<IEntity> targets
  ) {

    // Do something with metadata from the interaction (and targets)
    // The base Interaction class only has 'limiter'
    // UnityInteraction has a Label, but that's supposed to be for debugging
    // Normalize your output if you can,
    // running it through curves is recommended

    return 0;
  }
}
