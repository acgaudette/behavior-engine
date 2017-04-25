// PersonalityFactor.cs
// Created by Daniel W. Zhang on 04.23.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {
  public class PersonalityFactor : NormalizedAttribute, ILabeled {

    public float value;

    public string Label { get; set; }

    public PersonalityFactor(string Name, InitializeState i) : base(i) {
      Label = Name;
    }
  }
}
