// Person.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Person : Entity {

    public string name;

    public BrainRepository BrainRepo {
      get { return Repository as BrainRepository; }
    }

    Brain oracle;

    public Person(string name) : base() {
      this.name = name;
      oracle = new Brain();
    }

    public bool PerformAction(string key) {
      if (!BrainRepo.actions.ContainsKey(key))
        return false;

      BrainRepo.actions[key].Perform();

      return true;
    }

    protected override void PrePoll() {
      // State evaluation
      oracle.EvaluateState(GetAttributeInstances());

      // Goals etc. go here
    }

    protected override IList<Effect> Reaction(
      Interaction interaction, IEntity host
    ) {
      // Black box
      return oracle.ReactionEffects(
        interaction as InfluencedInteraction,
        host as Person, BrainRepo
      );
    }

    protected override IList<Effect> Observation(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      // Black box
      return oracle.ObservationEffects(
        interaction as InfluencedInteraction,
        host as Person, targets, BrainRepo
      );
    }

    protected override float Score(
      Interaction interaction, ICollection<IEntity> targets
    ) {
      // Black box
      return oracle.ComboScore(
        // Pass in evaluation from PrePoll() here
        interaction as InfluencedInteraction,
        targets, BrainRepo
      );
    }

    // Debug
    protected override void AssignDebugLabel(ref string label) {
      label = name;
    }
  }
}
