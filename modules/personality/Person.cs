using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public abstract class Person : Entity {

    public string Name {
      get; private set;
    }

    public BrainRepository BrainRepo {
      get { return Repository as BrainRepository; }
    }

    Brain oracle;

    public Person(string name) : base() {
      Name = name;
      oracle = new Brain();
    }

    public bool PerformAction(string key) {
      if (!BrainRepo.actions.ContainsKey(key))
        return false;

      BrainRepo.actions[key].Perform();

      return true;
    }

    protected override IList<Effect> Reaction(
      Interaction interaction, IEntity host
    ) {
      // Black box
      return oracle.GetEffectsFromInteraction(
        interaction as InfluencedInteraction, BrainRepo
      );
    }

    protected override IList<Effect> Observation(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      return null; // ! (placeholder)
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = Name;
    }
  }
}
