// InfluencedInteraction.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class InfluencedInteraction : Interaction {

    public HashSet<Property> strongPropertyInfluences;
    public HashSet<State> strongStateInfluences;

    public InfluencedInteraction(
      int limiter,
      HashSet<Property> properties,
      HashSet<State> states
    ) : base(limiter) {
      strongPropertyInfluences = properties;
      if (strongPropertyInfluences == null) {
        strongPropertyInfluences = new HashSet<Property>();
      }

      strongStateInfluences = states;
      if (strongStateInfluences == null) {
        strongStateInfluences = new HashSet<State>();
      }
    }
  }
}
