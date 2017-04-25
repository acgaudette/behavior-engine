using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Person : Entity, ILabeled {

    public string Label { set; get; }
    private Brain oracle;
    private PersonalityFactorClass fiveFactors;
    private PersonalityPropertyClass personalityProperties;

    public Person(string label, 
        Dictionary<string,
            Attribute<float>.InitializeState> initFactors = null,
        Dictionary<string,
            BehaviorEngine.Attribute<float>.InitializeState> initProperties
        = null
        ) : base() {
      Label = label;
      fiveFactors = new PersonalityFactorClass(initFactors);
      personalityProperties = new PersonalityPropertyClass(initProperties);
      oracle = new Brain(fiveFactors, personalityProperties);
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
