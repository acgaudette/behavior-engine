using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class PersonalityEffect : Effect {

    public HashSet<PersonalityFactor> strongFactorInfluences;
    public HashSet<PersonalityProperty> strongPropertyInfluences;

    ICharacterAction action;

    public PersonalityEffect(
      HashSet<PersonalityFactor> strongFactorInfluences,
      HashSet<PersonalityProperty> strongPropertyInfluences,
      Dictionary<PersonalityProperty,float> targets,
      ICharacterAction action
    ) : base() {
      this.strongFactorInfluences = strongFactorInfluences;

      if (strongFactorInfluences == null) {
        this.strongFactorInfluences = new HashSet<PersonalityFactor>();
      }

      this.strongPropertyInfluences = strongPropertyInfluences;

      if (strongPropertyInfluences == null) {
        this.strongPropertyInfluences = new HashSet<PersonalityProperty>();
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
