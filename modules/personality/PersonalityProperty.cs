namespace BehaviorEngine.Personality {
  public class PersonalityProperty : NormalizedAttribute, ILabeled {
    public string Label { get; set; }

    public PersonalityProperty(string Name, InitializeState i):base(i) {
      Label = Name;
    }
  }
}