// Entity.cs
// Created by Aaron C Gaudette on 09.04.17

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BehaviorEngine {

  public abstract class Entity {

    public List<Interaction> interactions;
    List<Attribute> attributes;

    public Entity() {
      interactions = new List<Interaction>();
      attributes = new List<Attribute>();
    }

    // Input can be an instance or archetype
    public Attribute GetAttribute(Attribute attribute) {
      return attributes.Find(a => a.ID == attribute.ID);
    }

    public ICollection<Attribute> GetAttributes() {
      return attributes;
    }

    // Input must be an archetype, and attributes are unique
    public bool AddAttribute(Attribute attribute) {
      if (attribute.Instance || GetAttribute(attribute) != null)
        return false;

      attributes.Add(new Attribute(attribute)); // Create instance

      return true;
    }

    // Input can be an instance or archetype
    public bool RemoveAttribute(Attribute attribute) {
      Attribute a = GetAttribute(attribute);
      if (a == null) return false;

      attributes.Remove(a);
      return true;
    }

    public void Subscribe(Class family) {
      attributes.Clear();
      for (ulong i = 0; i < family.AttributeCount; ++i)
        AddAttribute(family[i]);

      interactions = family.interactions;
    }

    public void React(Interaction interaction, Entity host) {
      List<Effect> effects = GetReaction(interaction, host);

      OnReact(interaction, host, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    public void Observe(Interaction interaction, Entity host, List<Entity> targets) {
      List<Effect> effects = GetObservation(interaction, host, targets);

      OnObserve(interaction, host, targets, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    public void Poll() {
      if (interactions.Count == 0) return;

      Interaction choice = null;
      List<Entity> targets = new List<Entity>();
      float highscore = 0;

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
          foreach (Entity target in GetTargets(i)) {
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
          continue; // Stub
        }
      }

      OnPoll(choice, new ReadOnlyCollection<Entity>(targets), highscore);

      if (choice == null) return; // Should never happen

      Interact(choice, targets);
    }

    public virtual Universe GetUniverse() {
      return Universe.root;
    }

    public virtual Class GetClass() {
      return Class.root;
    }

    public virtual ReadOnlyCollection<Entity> GetTargets(Interaction interaction) {
      return GetUniverse().GetEntities(this); // By default, target everything in the root
    }

    protected virtual void OnReact(Interaction interaction, Entity host, List<Effect> effects) { }

    protected virtual void OnObserve(
      Interaction interaction, Entity host, List<Entity> targets, List<Effect> effects
    ) { }

    protected virtual void OnPoll(
      Interaction choice, ReadOnlyCollection<Entity> targets, float highscore
    ) { }

    protected virtual List<Effect> GetReaction(Interaction interaction, Entity host) {
      if (host == this) {
        return null; // React to self
      }

      // Given an interaction, and host attributes
      // Compare a subset of these attributes to a subset of yours
      // Then, select an effect from the class

      return GetClass().effects; // By default, applies every effect
    }

    protected virtual List<Effect> GetObservation(
      Interaction interaction, Entity host, List<Entity> targets
    ) {
      return GetClass().effects;
    }

    protected abstract float Score(Interaction interaction, List<Entity> targets = null);

    void Interact(Interaction interaction, List<Entity> targets) {
      interaction.Perform(this, targets);
    }
  }
}
