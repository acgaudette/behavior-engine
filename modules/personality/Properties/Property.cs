using BehaviorEngine.Float;

namespace BehaviorEngine.Personality {

  public class Property : NormalizedAttribute {

    public string Name {
      get; private set;
    }

    public Property(string name, InitializeState i):base(i) {
      Name = name;
    }
  }
}
