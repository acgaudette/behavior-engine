using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public abstract class Person : Entity {

    public string Name {
      get; private set;
    }

    Brain oracle;
    BrainRepository repo;

    public Person(string name, BrainRepository repo) : base() {
      Name = name;
      oracle = new Brain();
      this.repo = repo;
    }

    protected override IList<Effect> Reaction(
      Interaction interaction, IEntity host
    ) {
      // Black box
      return oracle.GetEffectsFromInteraction(
        interaction as InfluencedInteraction, repo
      );
    }

    protected override IList<Effect> Observation(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      return null; // ! (placeholder)
    }
  }
}
