// InfluencedInteraction.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class InfluencedInteraction : Interaction {

    public Dictionary<Factor, Trait> strongTraitInfluences;
    public Dictionary<string, State> strongStateInfluences;

    public string actionID;

    public InfluencedInteraction(
      int limiter,
      IEnumerable<Trait> strongTraitInfluences,
      IEnumerable<State> strongStateInfluences,
      ICharacterAction action
    ) : base(limiter) {
      foreach (Trait trait in strongTraitInfluences)
        this.strongTraitInfluences[trait.type] = trait;

      foreach (State state in strongStateInfluences)
        this.strongStateInfluences[state.name] = state;

      actionID = action.ID;
    }

    public InfluencedInteraction(int limiter) : base(limiter) {
      strongTraitInfluences = new Dictionary<Factor, Trait>();
      strongStateInfluences = new Dictionary<string, State>();
    }
  }
}
