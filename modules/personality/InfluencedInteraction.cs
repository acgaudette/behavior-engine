// InfluencedInteraction.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class InfluencedInteraction : Interaction {

    public HashSet<Factor> strongFactorInfluences;
    public HashSet<Property> strongPropertyInfluences;

    public InfluencedInteraction(
      int limiter,
      HashSet<Factor> factors,
      HashSet<Property> properties
    ) : base(limiter) {
      strongFactorInfluences = factors;

      if(strongFactorInfluences == null) {
        strongFactorInfluences = new HashSet<Factor>();
      }

      strongPropertyInfluences = properties;
      if (strongPropertyInfluences == null) {
        strongPropertyInfluences = new HashSet<Property>();
      }
    }
  }
}
