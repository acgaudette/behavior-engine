// InfluencedEffect.cs

using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class InfluencedEffect : Effect {

    public string name;

    public Dictionary<TraitType, Trait> strongTraitInfluences;
    public Dictionary<string, State> strongStateInfluences;

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
      Dictionary<TraitType, Trait> strongTraitInfluences,
      Dictionary<string, State> strongStateInfluences,
      Dictionary<State, float> targets, // Or IAttribute/NormalizedAttribute
      ICharacterAction action
    ) : base() {
      this.name = name;

      this.strongTraitInfluences = strongTraitInfluences;
      if (strongTraitInfluences == null) {
        this.strongTraitInfluences = new Dictionary<TraitType, Trait>();
      }

      this.strongStateInfluences = strongStateInfluences;
      if (strongStateInfluences == null) {
        this.strongStateInfluences = new Dictionary<string, State>();
      }

      if (targets != null) {
        foreach(var entry in targets) {
          var state = entry.Key;
          var offset = entry.Value;
          modifiers.Add(new FloatModifier(state, offset));
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
