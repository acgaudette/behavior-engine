// Trait.cs
// Created by Daniel W. Zhang on 23.04.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {

  public enum TraitType {
    OPENNESS,
    CONSCIENTIOUSNESS,
    EXTRAVERSION,
    AGREEABLENESS,
    NEUROTICISM
  }

  public class Trait : Float.NormalizedAttribute {

    public TraitType type;

    public Trait(
      TraitType type, Initializer defaultInitializer = null
    ) : base(defaultInitializer == null ? () => 0 : defaultInitializer) {
      this.type = type;
    }
  }
}
