// InfluencedInteraction.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class InfluencedInteraction : Interaction {

    public HashSet<Trait> strongTraitInfluences;
    public HashSet<State> strongStateInfluences;

    public InfluencedInteraction(
      int limiter,
      HashSet<Trait> traits,
      HashSet<State> states
    ) : base(limiter) {
      strongTraitInfluences = traits;
      if (strongTraitInfluences == null) {
        strongTraitInfluences = new HashSet<Trait>();
      }

      strongStateInfluences = states;
      if (strongStateInfluences == null) {
        strongStateInfluences = new HashSet<State>();
      }
    }
  }
}
