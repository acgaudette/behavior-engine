// InfluencedEffect.cs

using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class InfluencedEffect : Effect {

    public string name;

    public Dictionary<FactorEnum, Factor> strongFactorInfluences;
    public Dictionary<string, Property> strongPropertyInfluences;

    /* Action link */

    ICharacterAction action;

    OnTriggerEventHandler TriggerAction = (
      object sender, IEntity target, bool effective
    ) => {
      InfluencedEffect e = sender as InfluencedEffect;
      if (e.action != null)
        e.action.Perform();
    };

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

      if (targets != null) {
        foreach(var entry in targets) {
          var property = entry.Key;
          var offset = entry.Value;
          modifiers.Add(new FloatModifier(property, offset));
        }
      }

      this.action = action;
      OnTrigger += TriggerAction; // Perform Action on Effect trigger
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = name;
    }
  }
}
