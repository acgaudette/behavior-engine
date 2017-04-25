// PersonalityFactor.cs
// Created by Daniel W. Zhang on 04.23.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {

  public class PersonalityFactor : Float.NormalizedAttribute {

    public PersonalityFactor(string name, InitializeState i) : base(i) {
      SetLabel(name); // SetLabel() not sustainable!
    }
  }
}
