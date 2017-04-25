// Mafia.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;

public class Mafia : MonoBehaviour {

  const string DATA_PATH
    = "./Assets/behavior-engine/examples/unity3d/mafia";

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    Class.root = new Class();

    // Characters
    Character[] characters = {
      new Character("Julian", null, null),
      new Character("Andy", null, null),
      new Character("Eugene", null, null)
    };

    // Actions
    foreach (Character character in characters) {
      ConsoleActionReader.LoadFile(
        DATA_PATH + "/actions.txt",
        // There should be a global/subscription CU,
        // not an instance for every Character
        character.unit
      );
    }

    // Attributes are defined in the Personality module

    /* Effects */

    Dictionary<FactorEnum, Factor> inputFactors = 
      new Dictionary<FactorEnum, Factor>();
    inputFactors[FactorEnum.CONSCIENTIOUSNESS] =
      new Factor(
        // Should probably reference instances in PersonalityPropertyClass instead?
        // An enum would also be useful here
        FactorEnum.CONSCIENTIOUSNESS, // Conscientiousness
        () => .2f // Shouldn't have state
      );

    Dictionary<string, Property> inputProperties = 
      new Dictionary<string, Property>();
    inputProperties[SharedData.PersonalityPropertyNames[0]] =
      new Property(
        SharedData.PersonalityPropertyNames[0], // angry
        () => .5f
      );

    Dictionary<Property, float> inputTargets = 
      new Dictionary<Property, float>();
    inputTargets[
      new Property(
        SharedData.PersonalityPropertyNames[3], // confused
        () => .2f
      )
    ] = .3f; // Offset

    Brain.CentralBrainRepository.registerEffect(
      new InfluencedEffect(
        "example",
        inputFactors, inputProperties, inputTargets,
        null
      )
    );

    // Generate Interactions

    // Attribution
    foreach (Character character in characters) {
      character.Subscribe(Class.root);
      Universe.root.entities.Add(character);
    }

    // Unity hooks
    ComponentManager hook = GetComponent<ComponentManager>();
    hook.Hook("Universe.root", Universe.root);
    hook.Hook("Class.root", Class.root);
  }
}
