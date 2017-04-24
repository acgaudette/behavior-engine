using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Person : Entity, ILabeled {

    public string Label { set; get; }
    private Brain oracle;
    private PersonalityClass fiveFactors;

    public Person(string label, 
        Dictionary<string, BehaviorEngine.Attribute<float>.InitializeState> init
        = null
        ) : base() {
      Label = label;
      fiveFactors = new PersonalityClass(init);
      oracle = new Brain(fiveFactors);
    }

    private Dictionary<Interaction, 
        List<Dictionary<string, PersonalityFactor>>> dict;

    protected override IList<Effect> GetReaction(
      Interaction interaction, Entity host
    ) {
      return new List<Effect>();
    }

    protected override IList<Effect> GetObservation(
      Interaction interaction, Entity host, ICollection<Entity> targets
    ) {
      return new List<Effect>();
    }

    protected override float Score(
      Interaction interaction, ICollection<Entity> targets
    ) {
      return 1;
    }
      
  }
}
