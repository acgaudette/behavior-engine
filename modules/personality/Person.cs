using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public abstract class Person : Entity {

    public string Name {
      get; private set;
    }

    private Brain oracle;
    private PersonalityFactorClass fiveFactors;
    private PersonalityPropertyClass personalityProperties;

    public Person(
      string name,
      Dictionary<string, Attribute<float>.InitializeState>
        initFactors,
      Dictionary<string, BehaviorEngine.Attribute<float>.InitializeState>
        initProperties
    ) : base() {
      Name = name;
      fiveFactors = new PersonalityFactorClass(initFactors);
      personalityProperties = new PersonalityPropertyClass(initProperties);
      oracle = new Brain(fiveFactors, personalityProperties);
    }

    private Dictionary<
      Interaction, List<Dictionary<string, PersonalityFactor>>
    > dict;

    protected override IList<Effect> GetReaction(
      Interaction interaction, Entity host
    ) {
      return oracle.GetEffectsFromInteraction(interaction as PersonalityInteraction);
    }

    protected override IList<Effect> GetObservation(
      Interaction interaction, Entity host, ICollection<Entity> targets
    ) {
      return null; // ! (placeholder)
    }
  }
}
