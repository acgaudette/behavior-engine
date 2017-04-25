// PersonalityFactor.cs
// Created by Daniel W. Zhang on 23.04.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {

  public class PersonalityFactor : Float.NormalizedAttribute {

    public string Name {
      get; private set;
    }

    public PersonalityFactor(string name, InitializeState i) : base(i) {
      Name = name;
    }
  }
}
