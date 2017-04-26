// Property.cs
// Created by Daniel W. Zhang on 23.04.17

using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class Property : NormalizedAttribute {

    public string Name {
      get; private set;
    }

    public Property(string name, Initializer defaultInitializer = null)
      : base(defaultInitializer == null ? () => 0 : defaultInitializer) {
      Name = name;
    }
  }
}
