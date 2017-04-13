// Entity.cs
// Created by Aaron C Gaudette on 09.04.17

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BehaviorEngine {

  public abstract class Entity {

    public List<Interaction> interactions;
    List<IAttributeInstance> attributes;

    public Entity() {
      interactions = new List<Interaction>();
      attributes = new List<IAttributeInstance>();
    }

    /* Interactions and Attributes */

    public IAttributeInstance GetAttribute(IAttribute attribute) {
      return attributes.Find(a => a.Prototype == attribute);
    }

    // Read-only collection
    public ICollection<IAttributeInstance> GetAttributes() {
      return attributes;
    }

    public bool AddAttribute(IAttribute attribute) {
      if (GetAttribute(attribute) != null) // No duplicates
        return false;

      attributes.Add(attribute.GetNewInstance());

      return true;
    }

    public bool RemoveAttribute(IAttribute attribute) {
      IAttributeInstance instance = GetAttribute(attribute);
      if (instance == null) return false;

      attributes.Remove(instance);
      return true;
    }

    // Replace Attributes and Interactions with the ones from a provided Class
    public void Subscribe(Class family) {
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
          ReadOnlyCollection<Entity> options = GetTargets(i);
          if (options == null) continue;

          foreach (Entity target in options) {
            if (target == this) continue; // Skip self

            score = Score(i, new List<Entity>(){ target });

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

      OnPoll(choice, new ReadOnlyCollection<Entity>(targets), highscore);

      if (choice == null) return;

      choice.Perform(this, targets);
    }

    // React (as a host or target) to an Interaction
    internal void React(Interaction interaction, Entity host) {
      List<Effect> effects = GetReaction(interaction, host);

      OnReact(interaction, host, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    // Observe an Interaction this Entity is not a part of
    internal void Observe(Interaction interaction, Entity host, List<Entity> targets) {
      List<Effect> effects = GetObservation(interaction, host, targets);

      OnObserve(interaction, host, targets, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    /* Reactions, observations, scoring (called internally, override these) */

    // Get the possible targets to interact with
    public virtual ReadOnlyCollection<Entity> GetTargets(Interaction interaction) {
      return Universe.root == null ? // By default, target everything in the root Universe
        null : Universe.root.GetEntities();
    }

    // Return a list (one or more) of effects on reaction to an Interaction with a host Entity
    protected virtual List<Effect> GetReaction(Interaction interaction, Entity host) {
      // Is this an interaction with the self?
      if (host == this) {
        return null;
      }

      // Compare a subset of the host's Attributes to a subset of this Entity's

      return null; // Does nothing by default
    }

    // Return a list (one or more) of effects on observation
    // of an Interaction with a host Entity and target Entities
    protected virtual List<Effect> GetObservation(
      Interaction interaction, Entity host, List<Entity> targets
    ) {
      return null; // Does nothing by default
    }

    // Given an interaction and its target(s), return a value--
    // higher values are more likely to be performed
    protected abstract float Score(Interaction interaction, List<Entity> targets = null);

    /* Events */

    protected virtual void OnReact(
      Interaction interaction, Entity host, List<Effect> effects
    ) { }

    protected virtual void OnObserve(
      Interaction interaction, Entity host, List<Entity> targets, List<Effect> effects
    ) { }

    protected virtual void OnPoll(
      Interaction choice, ReadOnlyCollection<Entity> targets, float highscore
    ) { }
  }
}
