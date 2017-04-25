using System.Collections.Generic;

namespace BehaviorEngine.Personality {
  public class PersonalityEffect : Effect, ILabeled {

    public string Label { get; set; }

    HashSet<PersonalityFactor> strongFactorInfluences;
    HashSet<PersonalityProperty> strongPropertyInfluences;
    PersonalityProperty targetProperty;

    public PersonalityEffect(
        HashSet<PersonalityFactor> factors = null,
        HashSet<PersonalityProperty> properties = null,
        PersonalityProperty target = null) : base() {
      strongFactorInfluences = factors;
      if(factors == null) {
        strongFactorInfluences = new HashSet<PersonalityFactor>();
      }
      strongPropertyInfluences = properties;
      if(properties == null) {
        strongPropertyInfluences = new HashSet<PersonalityProperty>();
      }
      this.targetProperty = target;
    }
  }
}