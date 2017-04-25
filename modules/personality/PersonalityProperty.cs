using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class PersonalityProperty : NormalizedAttribute {

    public string Name {
      get; private set;
    }

    public PersonalityProperty(string name, InitializeState i):base(i) {
      Name = name;
    }
  }
}
