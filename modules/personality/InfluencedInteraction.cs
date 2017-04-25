using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class InfluencedInteraction : Interaction {

    public HashSet<Factor> strongFactorInfluences;
    public HashSet<Property> strongPropertyInfluences;

    public InfluencedInteraction(
      int limiter,
      HashSet<Factor> factors = null,
      HashSet<Property> properties = null
    ) : base(limiter) {
      strongFactorInfluences = factors;

      if(factors == null) {
        factors = new HashSet<Factor>();
      }

      strongPropertyInfluences = properties;

      if (properties == null) {
        strongPropertyInfluences = new HashSet<Property>();
      }
    }
  }
}
