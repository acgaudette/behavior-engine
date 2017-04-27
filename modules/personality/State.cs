// State.cs
// Created by Daniel W. Zhang on 23.04.17

using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class State : NormalizedAttribute {

    public string name;

    public State(string name, Initializer defaultInitializer)
      : base(defaultInitializer) {
      this.name = name;
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = name;
    }
  }
}
