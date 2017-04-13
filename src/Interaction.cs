// Interaction.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  public class Interaction {
    public int limiter;

    public Interaction(int limiter) {
      this.limiter = limiter;
    }

    // Trigger reactions and observations to this interaction
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

    public virtual IEnumerable<Entity> GetObservers(
      Entity host, ICollection<Entity> targets
    ) {
      return Universe.root.entities;
    }
  }
}
