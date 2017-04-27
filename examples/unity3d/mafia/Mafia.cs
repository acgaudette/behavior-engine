// Mafia.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;
using BehaviorEngine.Float;

public static class MafiaExtensions {

   // Macro
   public static IModifier MOD(
    this BrainRepository repo, string stateName, float offset
  ) {
    return new FloatModifier(repo.GetState(stateName), offset)
      as IModifier;
  }
}

public class Mafia : MonoBehaviour {

  // Macro
  static List<T> LS<T>(params T[] p) {
    List<T> list = new List<T>();
    foreach (T t in p) list.Add(t);
    return list;
  }

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

    // Link each character to the central repository
    foreach (Character character in characters)
      character.Repository = repo;

    /* Actions */

    ConsoleActionReader.LoadFile(
      DATAPATH + "/" + FILENAME, repo.actions
    );

    List<string> actionIDs = new List<string>(repo.actions.Keys);

    /* Attributes (Traits, States) */

    Trait.RegisterFactors(repo, Distributions.Normal());

    repo.RegisterState(
      new State(
        "anger",
        Distributions.Uniform(), Transformations.Linear()
      )
    );

    repo.RegisterState(
      new State(
        "confusion",
        Distributions.Uniform(), Transformations.Linear()
      )
    );

    /* Effects */

    repo.Effects.Add(
      new InfluencedEffect(
        "example",

        LS( repo.GetTrait(Factor.CONSCIENTIOUSNESS) ),
        LS( repo.GetState("anger") ),
        LS( repo.MOD("confusion", .3f) ),

        // Random Action
        repo.actions[actionIDs[Random.Range(0, actionIDs.Count)]]
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
