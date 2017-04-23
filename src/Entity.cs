// Entity.cs
// Created by Aaron C Gaudette on 09.04.17

using System;
using System.Collections.Generic;

namespace BehaviorEngine {

  public abstract class Entity {

    public HashSet<Interaction> interactions;
    Dictionary<IAttribute, IAttributeInstance> attributes;

    public Entity() {
      interactions = new HashSet<Interaction>();
      attributes = new Dictionary<IAttribute, IAttributeInstance>();
    }

    /* Interactions and Attributes */

    public IAttributeInstance GetAttribute(IAttribute prototype) {
      if (!attributes.ContainsKey(prototype))
        return null;
      return attributes[prototype];
    }

    public ICollection<IAttributeInstance> GetAttributes() {
      return attributes.Values; // ReadOnlyDictionary not available in .NET 3.5
    }

    public bool AddAttribute(IAttribute prototype) {
      if (GetAttribute(prototype) != null)
        return false;

      attributes[prototype] = prototype.GetNewInstance();
      return true;
    }

    public bool RemoveAttribute(IAttribute prototype) {
      if (GetAttribute(prototype) == null)
        return false;

      attributes.Remove(prototype);
      return true;
    }

    // Replace Attributes and Interactions with the ones from a provided Class
    public void Subscribe(Class family) {
      attributes.Clear();

      foreach (IAttribute attribute in family.attributes)
        AddAttribute(attribute);

      interactions = family.interactions;
    }

    /* Reactions, observations, scoring (called externally) */

    // Given the possible Interactions and target Entities,
    // perform the highest-scoring Interaction/target(s) combo
    public void Poll() {
      if (interactions.Count == 0) return;

      Interaction choice = null;
      List<Entity> targets = new List<Entity>();
      float highscore = float.MinValue;

      foreach (Interaction i in interactions) {
        float score = 0;

        if (i.limiter == 0) {
          score = Score(i);

          if (score >= highscore) {
            choice = i;
            targets.Clear();
            highscore = score;
          }

          continue;
        }

        else if (i.limiter == 1) {
          ICollection<Entity> options = GetTargets(i);
          if (options == null) continue;

          foreach (Entity target in options) {
            if (target == this) continue; // Skip self

            score = Score(i, new List<Entity>(1){ target });

            if (score >= highscore) {
              choice = i;
              targets.Clear();
              targets.Add(target);
              highscore = score;
            }
          }
        }

        else {
          continue; // Stub (not yet implemented)
        }
      }

      OnPoll(choice, targets, highscore);

      if (choice == null) return;

      choice.Perform(this, targets);
    }

    // React (as a host or target) to an Interaction
    internal void React(Interaction interaction, Entity host) {
      IList<Effect> effects = GetReaction(interaction, host);

      OnReact(interaction, host, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    // Observe an Interaction this Entity is not a part of
    internal void Observe(
      Interaction interaction, Entity host, ICollection<Entity> targets
    ) {
      IList<Effect> effects = GetObservation(interaction, host, targets);

      OnObserve(interaction, host, targets, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    /* Reactions, observations, scoring (called internally, override these) */

    // Determine the targets of a particular Interaction
    public virtual ICollection<Entity> GetTargets(Interaction interaction) {
      // By default, target everything in the root Universe
      return Universe.root == null ? null : Universe.root.entities;
    }

    // Return an IList (one or more) of Effects on reaction
    // to an Interaction with a host Entity
    protected virtual IList<Effect> GetReaction(
      Interaction interaction, Entity host
    ) {
      // Is this an interaction with the self?
      if (host == this) {
        return null;
      }

      // Compare a subset of the host's Attributes to a subset of this Entity's

      return null; // Does nothing by default
    }

    // Return an IList (one or more) of Effects on observation
    // of an Interaction with a host Entity and target Entities
    protected virtual IList<Effect> GetObservation(
      Interaction interaction, Entity host, ICollection<Entity> targets
    ) {
      return null; // Does nothing by default
    }

    // Given an Interaction and its target(s), return a value;
    // higher values are more likely to be performed
    protected abstract float Score(
      Interaction interaction, ICollection<Entity> targets = null
    );

    /* Events */

    protected virtual void OnReact(
      Interaction interaction, Entity host, IList<Effect> effects
    ) { }

    protected virtual void OnObserve(
      Interaction interaction, Entity host,
      ICollection<Entity> targets, IList<Effect> effects
    ) { }

    protected virtual void OnPoll(
      Interaction choice, ICollection<Entity> targets, float highscore
    ) { }
  }
}
