using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class PersonalityInteraction : Interaction {

    public HashSet<PersonalityFactor> strongFactorInfluences;
    public HashSet<PersonalityProperty> strongPropertyInfluences;

    public PersonalityInteraction(
      int limiter,
      HashSet<PersonalityFactor> factors = null,
      HashSet<PersonalityProperty> properties = null
    ) : base(limiter) {
      strongFactorInfluences = factors;

      if(factors == null) {
        factors = new HashSet<PersonalityFactor>();
      }

      strongPropertyInfluences = properties;

      if (properties == null) {
        strongPropertyInfluences = new HashSet<PersonalityProperty>();
      }
    }
  }
}
