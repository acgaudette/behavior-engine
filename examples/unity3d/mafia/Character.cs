// Placeholder
// TODO: Inherit Person

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;

public class Character : ConsoleCharacter {

  private Brain brain;
  private PersonalityFactorClass fiveFactors;
  private PersonalityPropertyClass personalityProperties;

  public Character(
    string name, 
    Dictionary<string,
    Attribute<float>.InitializeState> initFactorsl,
    Dictionary<string,
    BehaviorEngine.Attribute<float>.InitializeState> initProperties) : 
  base(name) { 
    fiveFactors = new PersonalityFactorClass();
    personalityProperties = new PersonalityPropertyClass();
    brain = new Brain(fiveFactors, personalityProperties);
  }

  protected override IList<Effect> GetReaction(Interaction interaction, Entity host)
  {
    brain.GetEffectsFromInteraction(interaction);
  }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    return Random.Range(0, 1f);
  }
}
