// Mafia.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;
using BehaviorEngine.Float;

public class Mafia : MonoBehaviour {

  const string DATA_PATH
    = "./Assets/behavior-engine/examples/unity3d/mafia";

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    BrainRepository repo = new BrainRepository();

    // Characters
    Character[] characters = {
      new Character("Julian", repo),
      new Character("Andy", repo),
      new Character("Eugene", repo)
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
      new Factor(FactorEnum.CONSCIENTIOUSNESS);

    Dictionary<string, Property> inputProperties = 
      new Dictionary<string, Property>();
    inputProperties["angry"] = new Property("angry");

    Dictionary<Property, float> inputTargets = 
      new Dictionary<Property, float>();
    inputTargets[new Property("confused")] = .3f; // Offset

    repo.effects.Add(
      new InfluencedEffect(
        "example",
        inputFactors, inputProperties, inputTargets,
        null
      )
    );

    // Generate Interactions

    // Attribution
    foreach (Character character in characters) {
      character.Subscribe(repo);
      Universe.root.entities.Add(character);
    }

    // Unity hooks
    ComponentManager hook = GetComponent<ComponentManager>();
    hook.Hook("Universe.root", Universe.root);
    hook.Hook("repo", repo);
  }
}
