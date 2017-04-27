// Brain.cs

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Brain {

    static Random random = new Random();

    public void EvaluateState(
      ICollection<IAttributeInstance> instances
    ) {

      foreach (IAttributeInstance instance in instances) {
        var i = instance as State.TransformedInstance;
        if (i == null) continue;

        // Do something with i.Transformed
      }

      return; // Placeholder--return something useful!
    }

    public IList<Effect> ReactionEffects(
      InfluencedInteraction i,
      Person host, // Unused
      BrainRepository repo
    ) {
      // Return value
      List<Effect> effects = new List<Effect>();

      var allEffects = new List<InfluencedEffect>(repo.Effects);
      // Guarantee that default Effect will be different every time
      Shuffle(allEffects);

      int position = 0, total = allEffects.Count;
      foreach (InfluencedEffect e in allEffects) {
        // Difference in influence (zero = the same)
        int differential = 0;
        foreach (var trait in i.strongTraitInfluences) {
          if (!e.strongTraitInfluences.ContainsKey(trait.type)) {
            differential++;
          }
        }

        foreach (var state in i.strongStateInfluences) {
          if (!e.strongStateInfluences.ContainsKey(state.name)) {
            differential++;
          }
        }

        if (differential == 0) {
          effects.Add(e);
        } else {
          double top = random.NextDouble() * differential;
          double currentCount = effects.Count;
          double s = random.NextDouble() * differential;
          double toTheEnd = (total - position) / (total * differential);

          // s / top is always guaranteed to be positive
          if (s / top > Math.Min(toTheEnd, currentCount)) {
            effects.Add(e);
          }
        }

        position++;
      }

      return effects;
    }

    public IList<Effect> ObservationEffects(
      InfluencedInteraction interaction, Person host,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      return null; // Placeholder
    }

    public float ComboScore(
      InfluencedInteraction interaction,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      return 0; // Placeholder
    }

    void Shuffle(IList<InfluencedEffect> list) {
      int n = list.Count;
      while (n > 1) {
        int k = (random.Next(0, n) % n);
        n--;
        var value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }
}
