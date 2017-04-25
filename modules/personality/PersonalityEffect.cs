using System.Collections.Generic;

using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {
  public class PersonalityEffect : Effect {

    public HashSet<PersonalityFactor> strongFactorInfluences;
    public HashSet<PersonalityProperty> strongPropertyInfluences;

    public PersonalityEffect(
        HashSet<PersonalityFactor> factors = null,
        HashSet<PersonalityProperty> properties = null,
        Dictionary<PersonalityProperty,float> targets = null) : base() {
      strongFactorInfluences = factors;
      if(factors == null) {
        strongFactorInfluences = new HashSet<PersonalityFactor>();
      }
      strongPropertyInfluences = properties;
      if(properties == null) {
        strongPropertyInfluences = new HashSet<PersonalityProperty>();
      }
      if(targets == null) {
        return;
      }
      foreach(var entry in targets) {
        var property = entry.Key;
        var offset = entry.Value;
        modifiers.Add(new FloatModifier(property, offset));
      }
    }
  }
}
