// Trait.cs
// Created by Daniel W. Zhang on 23.04.17
// General structure for a personality factor

using System;

namespace BehaviorEngine.Personality {

  public enum Factor {
    OPENNESS,
    CONSCIENTIOUSNESS,
    EXTRAVERSION,
    AGREEABLENESS,
    NEUROTICISM
  }

  public class Trait : Float.NormalizedAttribute {

    // Register all (const) factors in a provided repository
    public static void RegisterFactors(
      BrainRepository repo, Initializer initializer = null
    ) {
      foreach (Factor factor in Enum.GetValues(typeof(Factor)))
        repo.RegisterTrait(new Trait(factor, initializer));
    }

    public Factor type;

    public Trait(
      Factor type, Initializer defaultInitializer = null
    ) : base(defaultInitializer == null ? () => 0 : defaultInitializer) {
      this.type = type;
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = type.ToString();
    }
  }
}
