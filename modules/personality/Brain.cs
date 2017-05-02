// Brain.cs

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Brain {

    static Random random = new Random();

    public IEnumerable<State> EvaluateState(
      ICollection<IAttributeInstance> instances
    ) {

      var top = new LinkedList<State>(); // Dequeue
      float highscore = 0;

      foreach (IAttributeInstance instance in instances) {
        if (!(instance.Prototype is State)) continue;
        var i = instance as State.TransformedInstance;

        float s = i.TransformedState;
        if (s >= highscore) {
          top.AddLast(i.Prototype as State);
          highscore = s;
        }
      }

      while (top.Count > 2) // Top 2
        top.RemoveFirst();

      return top;
    }

    public IList<Effect> ReactionEffects(
      InfluencedInteraction i,
      Character host, // Unused
      BrainRepository repo
    ) {
      var effects = getStronglyInfluencedEffects(i);
      //TODO: Change selection via host/other methods
      return effects;
    }

    public IList<Effect> ObservationEffects(
      InfluencedInteraction interaction, Character host,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      var effects = getStronglyInfluencedEffects();
      //
      return effects;
    }

    public float ComboScore(
      IEnumerable<State> state,
      InfluencedInteraction interaction,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      return Float.Distributions.Uniform()(); // Placeholder
    }

    private void getStronglyInfluencedEffects(InfluencedInteraction i) {
      // Return value
      List<Effect> effects = new List<Effect>();

      var allEffects = new List<InfluencedEffect>(repo.Effects);
      // Guarantee that default Effect will be different every time
      Shuffle(allEffects);

      int position = 0, total = allEffects.Count;
      foreach (InfluencedEffect e in allEffects) {
        // Difference in influence (zero = the same)
        int differential = 0;
        foreach (Factor factor in i.strongTraitInfluences.Keys) {
          if (!e.strongTraitInfluences.ContainsKey(factor)) {
            differential++;
          }
        }

        foreach (string name in i.strongStateInfluences.Keys) {
          if (!e.strongStateInfluences.ContainsKey(name)) {
            differential++;
          }
        }

        if (differential == 0) {
          effects.Add(e);
        } else {
          double toTheEnd = (total - position) / (total * differential);
          double selectChance = random.NextDouble() / random.NextDouble();

          // the values are always guaranteed to be non-negative
          if (selectChance > Math.Min(toTheEnd, effects.Count)) {
            effects.Add(e);
          }
        }

        position++;
      }
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
