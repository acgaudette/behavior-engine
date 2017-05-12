// Brain.cs

using System;
using System.Collections.Generic;

using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class Brain {

    private const float LOW_STABILITY = .3f;
    private const float HI_STABILITY = .7f;
    private const float MIDPOINT = 0.5f;

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
      var effects = GetStronglyInfluencedEffects(repo, i, host);
      //TODO: Change selection via host/other methods; with the addtion of the
      //relationship module, this seems more reasonable
      return effects;
    }

    public IList<Effect> ObservationEffects(
      InfluencedInteraction interaction, Character host,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      var effects = GetStronglyInfluencedEffects(repo, interaction, host);
      //TODO: Change selection via host/other methods; with the addtion of the
      //relationship module, this seems more reasonable
      return effects;
    }

    public float ComboScore(
      IEnumerable<State> state, Character host,
      InfluencedInteraction interaction,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      float score = 0f;
      int count = 0;
      foreach(State s in interaction.strongStateInfluences.Values) {
        State.TransformedInstance currState
          = host[s] as State.TransformedInstance;
        if(currState == null) {
          continue;
        }
        score = currState.TransformedState;

        foreach(State t in state) {
          if(t.Equals(s)) {
            score += .25f;
          }
        }
        count++;
      }
      if(count != 0) {
        score /= count;
      }
      //BehaviorEngine.Debug.Logger.Log("Calculated score");
      //score *= StabilityScore(state, host, interaction, targets, repo);

      return score;
    }

    private float StabilityScore(
      IEnumerable<State> state, Character host,
      InfluencedInteraction interaction,
      ICollection<IEntity> targets,
      BrainRepository repo
    ) {
      float ownScore = 0f;
      int count = 0;
      foreach(State s in state) {
        State.TransformedInstance currState 
        = host[s] as State.TransformedInstance;
        ownScore += currState.TransformedState;
        count++;
      }
      ownScore /= count;
      float ease = .3f;
      if(ownScore < LOW_STABILITY) {
        ease = .5f;
      } else {
        if(ownScore > HI_STABILITY) {
          ease = .1f;
        }
      }

      var func = Transformations.EaseSquaredAtValue(ease);

      var effects = GetStronglyInfluencedEffects(repo, interaction, host);
      //List<FloatModifier> modifiers = new List<FloatModifier>();
      float modifyVal = 0f;
      foreach(Effect e in effects) {
        float currMod = 0f;
        foreach(IModifier m in e.Modifiers) {
          FloatModifier floatMod = m as FloatModifier;
          currMod = Math.Abs(floatMod.offset);
        }
        if (e.Modifiers.Count != 0) {
          currMod /= e.Modifiers.Count;
        }
        modifyVal += currMod;
      }
      if (effects.Count != 0) {
        modifyVal /= effects.Count;
      }

      ownScore = func(modifyVal);

      if(targets == null) {
        //BehaviorEngine.Debug.Logger.Log("Score: " + ownScore);
        return ownScore;
      }

      float otherScore = 0f;
      foreach(IEntity target in targets) {
        var attrs = target.GetAttributeInstances();
        float stabilityValue = 0f;
        float weight = 1f;
        foreach(IAttributeInstance a in attrs) {
          NormalizedAttribute.Instance attr = a as NormalizedAttribute.Instance;
          stabilityValue += attr.State;
        }
        stabilityValue /= attrs.Count;
        weight = .3f;
        if(stabilityValue <= LOW_STABILITY) {
          weight = .5f;
        } else {
          weight = .1f;
        }
        var f = Transformations.EaseSquaredAtValue(weight);
        otherScore += f(modifyVal);
      }
      otherScore /= targets.Count;
      float score = (ownScore + otherScore) / 2;
      //BehaviorEngine.Debug.Logger.Log("Score: " + score);
      return score;
    }

    private IList<Effect> GetStronglyInfluencedEffects(
      BrainRepository repo,
      InfluencedInteraction i,
      Character host
    ) {
      // Return value
      List<Effect> effects = new List<Effect>();
      List<InfluencedEffect> unused = new List<InfluencedEffect>();
      List<int> differentials = new List<int>();

      var allEffects = new List<InfluencedEffect>(repo.Effects);
      // Guarantee that default Effect will be different every time
      Shuffle(allEffects);

      int position = 0, total = allEffects.Count;
      foreach (InfluencedEffect e in allEffects) {
        // Difference in influence (zero = the same)
        int differential = 0;
        List<float> traitValues = new List<float>();
        List<float> stateValues = new List<float>();
        foreach (var pair in i.strongTraitInfluences) {
          if (!e.strongTraitInfluences.ContainsKey(pair.Key)) {
            differential++;
          } else {
            foreach (var instance in host.GetAttributeInstances()) {
              if (instance.Prototype.Equals(pair.Value)) {
                var val = instance as Trait.TransformedInstance;
                traitValues.Add(val.TransformedState);
              }
            }
          }
        }

        foreach (var pair in i.strongStateInfluences) {
          if (!e.strongStateInfluences.ContainsKey(pair.Key)) {
            differential++;
          } else {
            foreach (var instance in host.GetAttributeInstances()) {
              if (instance.Prototype.Equals(pair.Value)) {
                var val = instance as State.TransformedInstance;
                stateValues.Add(val.TransformedState);
              }
            }
          }
        }

        if (differential == 0) {
          bool chosen = false;
          float accumulatorVal = 0f;
          float modTotal = 0f;
          int numTraits = 0;
          foreach(var m in e.Modifiers) {
            var f = m as FloatModifier;
            float valCalc = 0f;
            foreach (var traitVal in traitValues) {
              foreach (var stateVal in stateValues) {
                valCalc += stateVal;
              }
              numTraits++;
            }
            valCalc /= numTraits;
            accumulatorVal += valCalc;
            modTotal += f.offset;
          }
          accumulatorVal /= e.Modifiers.Count;
          modTotal /= e.Modifiers.Count;

          chosen
            = (accumulatorVal < LOW_STABILITY &&
          modTotal > 0 && modTotal < .2f) ||
          (accumulatorVal > LOW_STABILITY && accumulatorVal < MIDPOINT &&
          modTotal >= .2f) ||
          (accumulatorVal > MIDPOINT && accumulatorVal < HI_STABILITY &&
          modTotal < 0f && modTotal > -.2f) ||
          (accumulatorVal > HI_STABILITY &&
          modTotal <= -.2f);
          if(chosen) {
            effects.Add(e);
          } else {
            //TODO: Effect is not chosen; perhaps could be a useful point.
          }
        } else {
          unused.Add(e);
          differentials.Add(differential);
        }

        position++;
      }
      /*
      while(effects.Count == 0) {
        Shuffle(unused);
        int index = 0;
        double chanceTotal = (differentials[index] * total);
        double toTheEnd = ((double)(total - position)) / chanceTotal;
        double selectChance = random.NextDouble();

        // the values are always guaranteed to be non-negative
        if (selectChance > Math.Min(toTheEnd, effects.Count)) {
          effects.Add(unused[index]);
        }
        index++;
      }
      */
      if(effects.Count != 1) {
        //TODO: Debug statement in case no effects or more than one
        //effect is chosen
        /*BehaviorEngine.Debug.Logger.Log(
        "Number of effects chosen: " + effects.Count
        );
        */
      }

      /*
      if(effects.Count == 0) {
        effects.Add(unused[0]);
      }
      */

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
