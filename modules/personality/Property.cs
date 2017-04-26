// Property.cs
// Created by Daniel W. Zhang on 23.04.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {

  public enum PropertyType {
    OPENNESS,
    CONSCIENTIOUSNESS,
    EXTRAVERSION,
    AGREEABLENESS,
    NEUROTICISM
  }

  public class Property : Float.NormalizedAttribute {

    public PropertyType type;

    public Property(
      PropertyType type, Initializer defaultInitializer = null
    ) : base(defaultInitializer == null ? () => 0 : defaultInitializer) {
      this.type = type;
    }
  }
}
