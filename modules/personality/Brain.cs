using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {
  public class Brain {

    public static class CentralBrainRepository {

      public static HashSet<PersonalityEffect> allEffects =
          new HashSet<PersonalityEffect>();

      public static void registerEffect(PersonalityEffect e) {
        allEffects.Add(e);
      }

      public static List<PersonalityEffect> getAllEffects() {
        return new List<PersonalityEffect>(allEffects);
      }
    }

    PersonalityFactorClass fiveFactors;
    PersonalityPropertyClass properties;
    Random r;

    /** Make sure BrainInteractionFactory is loaded with interactions and
     * properties before instatiating any instances of this class
     */
    public Brain(PersonalityFactorClass fiveFactors,
        PersonalityPropertyClass properties) {
      this.fiveFactors = fiveFactors;
      this.properties = properties;
      r = new Random();
    }

    public List<PersonalityEffect> GetEffectsFromInteraction(
        PersonalityInteraction i) {
      List<PersonalityEffect> effects = new List<PersonalityEffect>();
      var allEffects = new List<PersonalityEffect>(
          CentralBrainRepository.getAllEffects());
      Shuffle(allEffects);
      int position = 0, total = allEffects.Count;
      foreach(PersonalityEffect e in CentralBrainRepository.getAllEffects()) {
        int differential = 0;
        foreach(var factor in i.strongFactorInfluences) {
          if(!e.strongFactorInfluences.Contains(factor)) {
            differential++;
          }
        }

        foreach(var prop in i.strongPropertyInfluences) {
          if(!e.strongPropertyInfluences.Contains(prop)) {
            differential++;
          }
        }

        if(differential == 0) {
          effects.Add(e);
        } else {
          double top = r.NextDouble() * differential;
          double currentCount = effects.Count;
          double s = r.NextDouble() * differential;
          double toTheEnd = (total - position) / total;
          if(s / top > Math.Min(toTheEnd, currentCount)) {
            effects.Add(e);
          }
        }
        position++;
      }
      return effects;
    }

    private void Shuffle(IList<PersonalityEffect> list) {
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
