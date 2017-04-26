// Brain.cs

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Brain {

    static Random random = new Random();

    // Called from GetReaction()
    public IList<Effect> GetEffectsFromInteraction(
      InfluencedInteraction i, BrainRepository repo
    ) {
      // Return value
      List<Effect> effects = new List<Effect>();

      var allEffects = new List<InfluencedEffect>(repo.effects);
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
