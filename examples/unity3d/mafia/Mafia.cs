// Mafia.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;
using BehaviorEngine.Float;

public class Mafia : MonoBehaviour {

  const string DATAPATH
    = "./Assets/behavior-engine/examples/unity3d/mafia";
  const string FILENAME
    = "actions.txt";

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

    // Entity repositories
    foreach (Character character in characters)
      character.Repository = repo;

    // Actions
    foreach (Character character in characters) {
      ConsoleActionReader.LoadFile(
        DATAPATH + "/" + FILENAME, repo.actions
      );
    }

    /* Attributes */

    /* Effects */

    repo.effects.Add(
      new InfluencedEffect(
        "example",
        new List<Trait>() { new Trait(TraitType.CONSCIENTIOUSNESS) },
        new List<State>() { new State("angry") },
        new List<FloatModifier>() {
          new FloatModifier(new State("confused"), .3f)
        },
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
