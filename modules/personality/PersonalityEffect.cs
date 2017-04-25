using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class PersonalityEffect : Effect {

    HashSet<PersonalityFactor> strongFactorInfluences;
    HashSet<PersonalityProperty> strongPropertyInfluences;
    PersonalityProperty targetProperty;

    ICharacterAction action;

    public PersonalityEffect(
      HashSet<PersonalityFactor> factors = null,
      HashSet<PersonalityProperty> properties = null,
      PersonalityProperty target = null,
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

      this.targetProperty = target;

      this.action = action;
    }

    protected override void OnTrigger(Entity target, bool effective) {
      if (action !=null)
        action.Perform();
    }
  }
}
