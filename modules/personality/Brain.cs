using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Brain {

    public static class CentralBrainRepository {

      public static Dictionary<FactorEnum, Factor> factorPrototypes =
        new Dictionary<FactorEnum, Factor>();

      public static HashSet<InfluencedEffect> allEffects =
        new HashSet<InfluencedEffect>();

      public static void registerFactor(Factor f) {
        factorPrototypes[f.factorType] = f;
      }

      public static void registerEffect(InfluencedEffect e) {
        allEffects.Add(e);
      }

      public static List<InfluencedEffect> getAllEffects() {
        return new List<InfluencedEffect>(allEffects);
      }
    }

    FactorClass fiveFactors; // Never used
    PersonalityPropertyClass properties; // Never used
    Random r;

    /** Make sure BrainInteractionFactory is loaded with interactions and
     * properties before instatiating any instances of this class
     */
    public Brain(
      FactorClass fiveFactors,
      PersonalityPropertyClass properties
    ) {
      this.fiveFactors = fiveFactors;
      this.properties = properties;
      r = new Random();
    }

    // Called from GetReaction()
    public IList<Effect> GetEffectsFromInteraction(
      InfluencedInteraction i
    ) {
      // Return value
      List<Effect> effects = new List<Effect>();

      var allEffects = new List<InfluencedEffect>(
        CentralBrainRepository.getAllEffects()
      );
      // Guarantee that default Effect will be different every time
      Shuffle(allEffects);

      int position = 0, total = allEffects.Count;
      foreach (InfluencedEffect e in allEffects) {
        // Difference in influence (zero = the same)
        int differential = 0;
        foreach (var factor in i.strongFactorInfluences) {
          if (!e.strongFactorInfluences.ContainsKey(factor.factorType)) {
            differential++;
          }
        }

        foreach (var prop in i.strongPropertyInfluences) {
          if (!e.strongPropertyInfluences.ContainsKey(prop.Name)) {
            differential++;
          }
        }

        if (differential == 0) {
          effects.Add(e);
        } else {
          double top = r.NextDouble() * differential;
          double currentCount = effects.Count;
          double s = r.NextDouble() * differential;
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

    private void Shuffle(IList<InfluencedEffect> list) {
      int n = list.Count;
      while (n > 1) {
        int k = (r.Next(0, n) % n);
        n--;
        var value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }
}
