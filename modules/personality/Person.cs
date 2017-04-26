using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public abstract class Person : Entity {

    public string Name {
      get; private set;
    }

    private Brain oracle;

    public Person(string name) : base() {
      Name = name;
      oracle = new Brain();
    }

    protected override IList<Effect> GetReaction(
      Interaction interaction, Entity host
    ) {
      // Black box
      return oracle.GetEffectsFromInteraction(interaction as InfluencedInteraction);
    }

    protected override IList<Effect> GetObservation(
      Interaction interaction, Entity host, ICollection<Entity> targets
    ) {
      return null; // ! (placeholder)
    }
  }
}
