// Interaction.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  public class Interaction : Root {
    public int limiter;

    public Interaction(int limiter) {
      this.limiter = limiter;
    }

    // Trigger reactions and observations to this Interaction
    // Expects a target list with count <= the limiter
    public bool Perform(Entity host, ICollection<Entity> targets) {
      if (host == null || targets == null || targets.Count > limiter)
        return false; // Invalid input

      host.React(this, host); // React to self

      foreach (Entity target in targets) // Target reactions
        target.React(this, host);

      IEnumerable<Entity> observers = GetObservers(host, targets);

      if (observers != null) {
        foreach (Entity target in observers) { // Observers
          // Skip host and targets
          if (target == host || targets.Contains(target))
            continue;

          target.Observe(this, host, targets);
        }
      }

      return true;
    }

    // Determine the observers of this Interaction given a host and  target(s)
    public virtual IEnumerable<Entity> GetObservers(
      Entity host, ICollection<Entity> targets
    ) {
      // By default, target everything in the root Universe
      return Universe.root == null ? null : Universe.root.entities;
    }
  }
}
