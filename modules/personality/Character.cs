// Character.cs

using System.Collections.Generic;

using CharacterState = System.Collections.Generic.IEnumerable<
  BehaviorEngine.Personality.State
>;

namespace BehaviorEngine.Personality {

  public class Character : Entity {

    public string name;

    public BrainRepository BrainRepo {
      get { return Repository as BrainRepository; }
    }

    Brain oracle;
    CharacterState state;

    // Action link
    EntityEvents.OnPollEventHandler TriggerAction = (
      object sender,
      Interaction choice, ICollection<IEntity> targets, float highscore
    ) => {
      (sender as Character).PerformAction(
        (choice as InfluencedInteraction).actionID, targets
      );
    };

    public Character(string name) : base() {
      this.name = name;
      oracle = new Brain();

      // Perform Action on Interaction trigger
      OnPoll += TriggerAction;
    }

    public bool PerformAction(string id, ICollection<IEntity> targets) {
      ICharacterAction action = BrainRepo.GetAction(id);
      if (action == null)
        return false;

      action.Perform(GetActionInfo(targets));
      return true;
    }

    protected virtual CharacterActionInfo GetActionInfo(
      ICollection<IEntity> targets
    ) {
      return new CharacterActionInfo(this, targets);
    }

    protected override void PrePoll() {
      // State evaluation
      state = oracle.EvaluateState(GetAttributeInstances());

      // Goals etc. go here
    }

    protected override IList<Effect> Reaction(
      Interaction interaction, IEntity host
    ) {
      // Black box
      return oracle.ReactionEffects(
        interaction as InfluencedInteraction,
        host as Character, BrainRepo
      );
    }

    protected override IList<Effect> Observation(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      // Black box
      return oracle.ObservationEffects(
        interaction as InfluencedInteraction,
        host as Character, targets, BrainRepo
      );
    }

    protected override float Score(
      Interaction interaction, ICollection<IEntity> targets
    ) {
      // Black box
      return oracle.ComboScore(
        state, this,
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
