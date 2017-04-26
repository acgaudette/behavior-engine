// Factor.cs
// Created by Daniel W. Zhang on 23.04.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {

  public enum FactorEnum {
    OPENNESS,
    CONSCIENTIOUSNESS,
    EXTRAVERSION,
    AGREEABLENESS,
    NEUROTICISM
  }

  public class Factor : Float.NormalizedAttribute {

    public FactorEnum factorType;

    public Factor(
      FactorEnum factorType, Initializer defaultInitializer = null
    ) : base(defaultInitializer == null ? () => 0 : defaultInitializer) {
      this.factorType = factorType;
    }
  }
}
