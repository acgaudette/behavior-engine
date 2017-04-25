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

    HashSet<PersonalityFactor> inputFactors = new HashSet<PersonalityFactor>();
    inputFactors.Add(
      // Should be unique
      new PersonalityFactor(
        SharedData.PersonalityFactorNames[1], // Conscientiousness
        () => .2f // Shouldn't be state
      )
    );

    HashSet<PersonalityProperty> inputProperties = 
      new HashSet<PersonalityProperty>();
    inputProperties.Add(
      new PersonalityProperty(
        SharedData.PersonalityPropertyNames[0], // angry
        () => .5f
      )
    );

    Dictionary<PersonalityProperty, float> inputTargets = 
      new Dictionary<PersonalityProperty, float>();
    inputTargets[
      new PersonalityProperty(
        SharedData.PersonalityPropertyNames[3], // confused
        () => .2f
      )
    ] = .3f; // Offset

    Brain.CentralBrainRepository.registerEffect(
      new PersonalityEffect(inputFactors, inputProperties, inputTargets, null)
    );

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
        character.unit
      );
    }

    /* See Forum.cs for examples */

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
