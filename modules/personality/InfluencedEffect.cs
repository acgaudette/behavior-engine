// InfluencedEffect.cs

using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class InfluencedEffect : Effect {

    public string name;

    public Dictionary<Factor, Trait> strongTraitInfluences;
    public Dictionary<string, State> strongStateInfluences;

    public InfluencedEffect(
      string name,
      IEnumerable<Trait> strongTraitInfluences,
      IEnumerable<State> strongStateInfluences,
      List<IModifier> modifiers
    ) : this(name) {
      foreach (Trait trait in strongTraitInfluences)
        this.strongTraitInfluences[trait.type] = trait;

      foreach (State state in strongStateInfluences)
        this.strongStateInfluences[state.name] = state;

      Modifiers = modifiers;
    }

    public InfluencedEffect(string name) : base() {
      this.name = name;

      strongTraitInfluences = new Dictionary<Factor, Trait>();
      strongStateInfluences = new Dictionary<string, State>();
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = name;
    }
  }
}
