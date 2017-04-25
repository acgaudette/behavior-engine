using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class PersonalityEffect : Effect {

    public HashSet<PersonalityFactor> strongFactorInfluences;
    public HashSet<PersonalityProperty> strongPropertyInfluences;

    ICharacterAction action;

    public PersonalityEffect(
      HashSet<PersonalityFactor> factors,
      HashSet<PersonalityProperty> properties,
      Dictionary<PersonalityProperty,float> targets,
      ICharacterAction action
    ) : base() {
      strongFactorInfluences = factors;

      if (factors == null) {
        strongFactorInfluences = new HashSet<PersonalityFactor>();
      }

      strongPropertyInfluences = properties;

      if (properties == null) {
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

      this.action = action;
    }

    protected override void OnTrigger(Entity target, bool effective) {
      if (action !=null)
        action.Perform();
    }
  }
}
