// Entity.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Linq;

namespace BehaviorEngine {

  public abstract partial class Entity : Debug.Labeled, IEntity {

    public IRepository Repository { get; set; }
    Dictionary<IAttribute, IAttributeInstance> attributes;

    public Entity() {
      attributes = new Dictionary<IAttribute, IAttributeInstance>();
    }

    /* Attributes */

    public IAttributeInstance this[IAttribute prototype] {
      get {
        if (!attributes.ContainsKey(prototype))
          return null;
        return attributes[prototype];
      }
    }

    public ICollection<IAttributeInstance> GetAttributeInstances() {
      // ReadOnlyDictionary not available in .NET 3.5
      return attributes.Values;
    }

    public bool AddAttribute(IAttribute prototype) {
      if (this[prototype] != null)
        return false;

      attributes[prototype] = prototype.NewInstance();
      return true;
    }

    public bool RemoveAttribute(IAttribute prototype) {
      if (this[prototype] == null)
        return false;

      attributes.Remove(prototype);
      return true;
    }

    public virtual void Subscribe() {
      attributes.Clear(); // Replace, not add

      // Iterate through prototypes and create instances
      foreach (IAttribute prototype in Repository.AttributePrototypes)
        AddAttribute(prototype);
    }

    /* Reactions, observations, scoring (called externally) */

    // Given the possible Interactions and target Entities,
    // perform the highest-scoring Interaction/target(s) combo
    public void Poll() {
      PrePoll();

      if (
        // System.Linq is useful for this one case
        Repository.Interactions == null || !Repository.Interactions.Any()
      ) return;

      Interaction choice = null;
      List<IEntity> targets = new List<IEntity>();
      float highscore = float.MinValue;

      foreach (Interaction i in Repository.Interactions) {
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
          ICollection<IEntity> options = Targets(i);
          if (options == null) continue;

          foreach (IEntity target in options) {
            if (target == this) continue; // Skip self

            score = Score(i, new List<IEntity>(1){ target });

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

      EntityEvents.OnPollEventHandler handler = OnPoll;
      if (handler != null)
        handler(this, choice, targets, highscore);

      if (choice == null) return;

      choice.Perform(this, targets);
    }

    // React (as a host or target) to an Interaction
    public void React(Interaction interaction, IEntity host) {
      IList<Effect> effects = Reaction(interaction, host);

      EntityEvents.OnReactEventHandler handler = OnReact;
      if (handler != null)
        handler(this, interaction, host, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    // Observe an Interaction this Entity is not a part of
    public void Observe(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      IList<Effect> effects = Observation(interaction, host, targets);

      EntityEvents.OnObserveEventHandler handler = OnObserve;
      if (handler != null)
        handler(this, interaction, host, targets, effects);

      if (effects == null) return;

      foreach (Effect e in effects)
        e.Trigger(this);
    }

    /* Reactions, observations, scoring (called internally, override these) */

    protected virtual void PrePoll() { }

    // Determine the targets of a particular Interaction
    protected virtual ICollection<IEntity> Targets(
      Interaction interaction
    ) {
      // By default, target everything in the root Universe
      return Universe.root == null ? null : Universe.root.entities;
    }

    // Return an IList (one or more) of Effects on reaction
    // to an Interaction with a host Entity
    protected virtual IList<Effect> Reaction(
      Interaction interaction, IEntity host
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
    protected virtual IList<Effect> Observation(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      return null; // Does nothing by default
    }

    // Given an Interaction and its target(s), return a value;
    // higher values are more likely to be performed
    protected abstract float Score(
      Interaction interaction, ICollection<IEntity> targets = null
    );

    /* Events */

    public event EntityEvents.OnReactEventHandler OnReact;
    public event EntityEvents.OnObserveEventHandler OnObserve;
    public event EntityEvents.OnPollEventHandler OnPoll;
  }
}
