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
      new Character("Jurgen"),
      new Character("Francis"),
      new Character("Eugene")
    };

    foreach (Character character in characters)
      character.Repository = repo;

    // Actions
    foreach (Character character in characters) {
      ConsoleActionReader.LoadFile(
        DATA_PATH + "/actions.txt", repo.actions
      );
    }

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

    // Attribution
    foreach (Character character in characters) {
      character.Subscribe();
      Universe.root.entities.Add(character);
    }

    // Unity hooks
    ComponentManager hook = GetComponent<ComponentManager>();
    hook.Hook("Universe.root", Universe.root);
    hook.Hook<BrainRepoComponent>("brain-repo", repo);
  }
}
