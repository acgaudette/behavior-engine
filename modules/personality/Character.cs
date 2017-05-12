// Character.cs

using System.Collections.Generic;

using CharacterState = System.Collections.Generic.IEnumerable<
  BehaviorEngine.Personality.State
>;

namespace BehaviorEngine.Personality {

  public class Character : Entity {

    const float RELATIONSHIP_OFFSET = .2f;

    public string name;

    public BrainRepository BrainRepo {
      get { return Repository as BrainRepository; }
    }

    Brain oracle;
    CharacterState state;
    public Dictionary<string, Relationship> relationships;

    public Relationship.Affinities trustAffinities;
    public Relationship.Affinities agreementAffinities;

    // Action link
    EntityEvents.OnPollEventHandler TriggerAction = (
      object sender,
      Interaction choice, ICollection<IEntity> targets, float highscore
    ) => {
      if (choice == null) return;

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

      agreementAffinities = new Relationship.Affinities();
      trustAffinities = new Relationship.Affinities();
    }

    /* Relationships */

    // Access a relationship with another character, if it exists
    public Relationship GetRelationship(Character target) {
      if(relationships.ContainsKey(target.name))
        return relationships[target.name];
      return null;
    }

    // Create first-time relationship
    Relationship CreateRelationship(
      Character target,
      InfluencedInteraction interaction
    ) {
      Relationship r = GetRelationship(target);
      if (r != null) return null;

      var states = interaction.strongStateInfluences;
      float trust = 0, agreement = 0;

      foreach (string name in states.Keys) {
        agreement = agreementAffinities.Match(name) > 0 ?
          agreement + 1 : agreement - 1;
        trust = trustAffinities.Match(name) > 0 ?
          trust + 1 : trust - 1;
      }

      // Calculate initial axes

      if (trust == 0)
        trust = .5f;
      else if (trust < 0)
        trust = 1.0f / (-(trust - 2));
      else
        trust = .5f + (.05f * trust);

      if (agreement == 0)
        agreement = .5f;
      else if (agreement < 0)
        agreement = 1.0f / (-(agreement - 2));
      else agreement = .5f + (.05f * agreement);

      // Create relationship

      r = new Relationship(target);

      r.trust.Offset(trust * 2 - 1); // Scale
      r.agreement.Offset(agreement * 2 - 1); // Scale

      relationships[target.name] = r;

      return r;
    }

    // Event

    public delegate void OnUpdateRelationshipEventHandler(
      object sender,
      Character target,
      float trustOffset, float agreementOffset,
      Relationship relationship
    );

    public event OnUpdateRelationshipEventHandler OnUpdateRelationship;

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
      Character hostCharacter = host as Character;
      InfluencedInteraction influencedInteraction
        = interaction as InfluencedInteraction;
      // Black box
      var effects =  oracle.ReactionEffects(
        influencedInteraction,
        hostCharacter, BrainRepo
      );
      UpdateRelationship(hostCharacter, influencedInteraction, effects);
      return effects;
    }

    protected override IList<Effect> Observation(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    ) {
      Character character = host as Character;

      if (character == null) return null;

      InfluencedInteraction influencedInteraction
        = interaction as InfluencedInteraction;

      // Black box
      var effects =  oracle.ObservationEffects(
        influencedInteraction,
        character, targets, BrainRepo
      );

      /* Relationship (with host) */
      UpdateRelationship(character, influencedInteraction, effects);

      return effects;
    }

    protected void UpdateRelationship(
      Character character,
      InfluencedInteraction influencedInteraction,
      IList<Effect> effects
    ) {
      Relationship withHost = GetRelationship(character);
      if (withHost == null) {
        withHost = CreateRelationship(
          character, influencedInteraction
        );
      }

      float trustOffset = 0, agreementOffset = 0;

      foreach (Effect effect in effects) {
        var e = effect as InfluencedEffect;
        if (e == null) continue;

        // Check for matches
        foreach (string name in e.strongStateInfluences.Keys) {
          var matchTrust = trustAffinities.Match(name);
          var matchAgreement = agreementAffinities.Match(name);

          float trust = matchTrust > 0 ? RELATIONSHIP_OFFSET
            : matchTrust < 0 ? -RELATIONSHIP_OFFSET : 0;
          float agreement = matchAgreement > 0 ? RELATIONSHIP_OFFSET
            : matchAgreement < 0 ? -RELATIONSHIP_OFFSET : 0;

          withHost.trust.Offset(trust);
          withHost.agreement.Offset(agreement);

          trustOffset += trust;
          agreementOffset += agreement;
        }
      }

      // Event
      OnUpdateRelationshipEventHandler handler = OnUpdateRelationship;
      if (handler != null)
        handler(this, character, trustOffset, agreementOffset, withHost);
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
