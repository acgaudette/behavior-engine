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
    public Dictionary<string, Relationship> relationships;

    //TODO: As mentioned inside Relationship.cs, may want to replace with a
    //dictionary or some kind of data structure to make it more maintainable.
    List<State> positiveTrustStates;
    List<State> negativeTrustStates;
    List<State> positiveAgreeStates;
    List<State> negativeAgreeStates;

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
      relationships = new Dictionary<string, Relationship>();
      // Perform Action on Interaction trigger
      OnPoll += TriggerAction;

      positiveAgreeStates = new List<State>();
      negativeAgreeStates = new List<State>();
      positiveTrustStates = new List<State>();
      negativeTrustStates = new List<State>();
    }

    public void registerPositiveAgree(List<State> relevantStates) {
      positiveAgreeStates.AddRange(relevantStates);
    }

    public void registerNegativeAgree(List<State> relevantStates) {
      negativeAgreeStates.AddRange(relevantStates);
    }

    public void registerPositiveTrust(List<State> relevantStates) {
      positiveTrustStates.AddRange(relevantStates);
    }

    public void registerNegativeTrust(List<State> relevantStates) {
      negativeTrustStates.AddRange(relevantStates);
    }

    /* Relationships */

    // Create first-time relationship
    private void SetupRelationship(
      Character c,
      InfluencedInteraction interaction
    ) {
      var states = interaction.strongStateInfluences;
      int trust = 0;
      int agree = 0;
      foreach(string name in states.Keys) {
        if(positiveAgreeStates.Find(x => x.name.Equals(name)) != null) {
          agree++;
        }
        if(negativeAgreeStates.Find(x => x.name.Equals(name)) != null) {
          agree--;
        }
        if(positiveTrustStates.Find(x => x.name.Equals(name)) != null) {
          trust++;
        }
        if(negativeTrustStates.Find(x => x.name.Equals(name)) != null) {
          trust--;
        }
      }
      float trustworthiness = trust;
      if(trustworthiness == 0) {
        trustworthiness = .5f;
      } else {
        if(trustworthiness < 0) {
          trustworthiness = 1.0f / (-(trustworthiness - 2));
        } else {
          trustworthiness = .5f + (.05f * trustworthiness);
        }
      }

      float agreeability = agree;
      if(agreeability == 0) {
        agreeability = .5f;
      } else {
        if(agreeability < 0) {
          agreeability = 1.0f / (-(agreeability - 2));
        } else {
          agreeability = .5f + (.05f * agreeability);
        }
      }

      RegisterRelationship(c, agreeability, trustworthiness);
    }

    // Update an existing relationship
    public void RegisterRelationship(
      Character target, float agreeability, float trustworthiness
    ) {
      Relationship r = new Relationship(
        target, agreeability, trustworthiness
      );

      relationships[target.name] =  r;

      OnRegisterRelationshipEventHandler handler = OnRegisterRelationship;
      if (handler != null)
        handler(this, target, r);
    }

    // Access a relationship with another character, if it exists
    public Relationship GetRelationship(Character character) {
      if(relationships.ContainsKey(character.name))
        return relationships[character.name];
      return null;
    }

    public delegate void OnRegisterRelationshipEventHandler(
      object sender,
      Character target, Relationship relationship
    );

    public event OnRegisterRelationshipEventHandler OnRegisterRelationship;

    /* Actions */

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

    /* Overrides */

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
      InfluencedInteraction influencedInteraction
        = interaction as InfluencedInteraction;
      // Black box
      var effects =  oracle.ObservationEffects(
        influencedInteraction,
        host as Character, targets, BrainRepo
      );

      Relationship withHost = GetRelationship(host as Character);
      if(withHost == null) {
        SetupRelationship(host as Character, influencedInteraction);
        withHost = GetRelationship(host as Character);
      }

      foreach(Effect e in effects) {
        var effect = e as InfluencedEffect;
        if(effect == null) {
          continue;
        }
        var influences = effect.strongStateInfluences;
        foreach(State s in influences.Values) {
          var name = s.name;
          if(positiveAgreeStates.Find(x => x.name.Equals(name)) != null) {
            withHost.agreeability += .05f;
          }
          if(negativeAgreeStates.Find(x => x.name.Equals(name)) != null) {
            withHost.agreeability -= .05f;
          }
          if(positiveTrustStates.Find(x => x.name.Equals(name)) != null) {
            withHost.trustworthiness += .05f;
          }
          if(negativeTrustStates.Find(x => x.name.Equals(name)) != null) {
            withHost.trustworthiness -= .05f;
          }
        }
      }
      return effects;
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

    /* Debug */

    protected override void AssignDebugLabel(ref string label) {
      label = name;
    }
  }
}
