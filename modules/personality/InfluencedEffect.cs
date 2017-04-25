using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class InfluencedEffect : Effect {

    public string name;

    public Dictionary<FactorEnum, Factor> strongFactorInfluences;
    public Dictionary<string, Property> strongPropertyInfluences;

    ICharacterAction action;

    public InfluencedEffect(
      string name,
      Dictionary<FactorEnum, Factor> strongFactorInfluences,
      Dictionary<string, Property> strongPropertyInfluences,
      Dictionary<Property,float> targets,
      ICharacterAction action
    ) : base() {
      this.name = name;

      this.strongFactorInfluences = strongFactorInfluences;

      if (strongFactorInfluences == null) {
        this.strongFactorInfluences = new Dictionary<FactorEnum, Factor>();
      }

      this.strongPropertyInfluences = strongPropertyInfluences;

      if (strongPropertyInfluences == null) {
        this.strongPropertyInfluences = new Dictionary<string, Property>();
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
