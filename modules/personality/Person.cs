﻿// Person.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Person : Entity {

    public string name;

    public BrainRepository BrainRepo {
      get { return Repository as BrainRepository; }
    }

    Brain oracle;

    // Action link
    EntityEvents.OnPollEventHandler TriggerAction = (
      object sender,
      Interaction choice, ICollection<IEntity> targets, float highscore
    ) => {
      Person person = sender as Person;
      InfluencedInteraction i = choice as InfluencedInteraction;
      ICharacterAction action = person.BrainRepo.GetAction(i.actionID);

      if (action == null) return;
      action.Perform();
    };

    public Person(string name) : base() {
      this.name = name;
      oracle = new Brain();

      // Perform Action on Interaction trigger
      OnPoll += TriggerAction;
    }

    public bool PerformAction(string id) {
      ICharacterAction action = BrainRepo.GetAction(id);
      if (action == null)
        return false;

      action.Perform();
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
