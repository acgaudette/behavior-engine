namespace BehaviorEngine.Personality {

  public class PersonalityFactor : NormalizedAttribute, ILabeled {

    public float value;

    public string Label { get; set; }

    public PersonalityFactor(string Name, InitializeState i) : base(i) {
      Label = Name;
    }
  }
}
