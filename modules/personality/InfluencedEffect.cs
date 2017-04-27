﻿// InfluencedEffect.cs

using System.Collections.Generic;
using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class InfluencedEffect : Effect {

    public string name;

    public Dictionary<Factor, Trait> strongTraitInfluences;
    public Dictionary<string, State> strongStateInfluences;

    /* Action link */

    ICharacterAction action;

    EffectEvents.OnTriggerEventHandler TriggerAction = (
      object sender, IEntity target, bool effective
    ) => {
      InfluencedEffect e = sender as InfluencedEffect;
      if (e.action != null)
        e.action.Perform();
    };

    public InfluencedEffect(
      string name,
      IEnumerable<Trait> strongTraitInfluences,
      IEnumerable<State> strongStateInfluences,
      List<IModifier> modifiers,
      ICharacterAction action
    ) : this(name, action) {
      foreach (Trait trait in strongTraitInfluences)
        this.strongTraitInfluences[trait.type] = trait;

      foreach (State state in strongStateInfluences)
        this.strongStateInfluences[state.name] = state;

      Modifiers = modifiers;
    }

    public InfluencedEffect(
      string name, ICharacterAction action
    ) : base() {
      this.name = name;

      strongTraitInfluences = new Dictionary<Factor, Trait>();
      strongStateInfluences = new Dictionary<string, State>();

      this.action = action;
      // Perform Action on Effect trigger
      OnTrigger += TriggerAction;
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = name;
    }
  }
}
