// Interaction.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BehaviorEngine {

  public class Interaction {
    public int limiter;

    public Interaction(int limiter) {
      this.limiter = limiter;
    }

    // Trigger reactions and observations to this interaction
    // Expects a target list with count <= the limiter
    public bool Perform(Entity host, List<Entity> targets) {
      if (host == null || targets == null || targets.Count > limiter)
        return false; // Invalid input

      host.React(this, host); // React to self

      foreach (Entity target in targets) // Target reactions
        target.React(this, host);

      ReadOnlyCollection<Entity> observers = GetObservers(host, targets);

      if (observers != null) {
        foreach (Entity target in GetObservers(host, targets)) { // Observers
          if (target == host || targets.Contains(target)) // Skip host and targets
            continue;

          target.Observe(this, host, targets);
        }
      }

      return true;
    }

    public virtual ReadOnlyCollection<Entity> GetObservers(Entity host, List<Entity> targets) {
      return Universe.root.GetEntities();
    }
  }
}
