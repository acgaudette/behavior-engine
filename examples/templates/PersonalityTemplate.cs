// PersonalityTemplate.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;
using BehaviorEngine.Float;

public class PersonalityTemplate : MonoBehaviour {

  // Macro
  static List<T> LS<T>(params T[] p) {
    List<T> list = new List<T>();
    foreach (T t in p) list.Add(t);
    return list;
  }

  const string DATAPATH
    = "./Assets/behavior-engine/examples/templates";
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

    /*
    ConsoleActionReader.LoadFile(
      DATAPATH + "/" + FILENAME, repo
    );
    */

    List<string> actionIDs = new List<string>(repo.GetActionIDs());

    /* Attributes (Traits, States) */

    Trait.RegisterFactors(repo, Distributions.Normal());

    repo.RegisterState(
      new State(
        "anger",
        Distributions.Uniform(),
        Transformations.EaseSquared()
      )
    );

    repo.RegisterState(
      new State(
        "confusion",
        Distributions.Uniform(),
        Transformations.InvertedSquared()
      )
    );

    repo.RegisterState(
      new State(
        "energy", () => 1,
        Transformations.Linear()
      )
    );

    /* Effects */

    repo.Effects.Add(
      new InfluencedEffect(
        "effect-example",
        LS( repo.GetTrait(Factor.CONSCIENTIOUSNESS) ),
        LS( repo.GetState("anger") ),
        LS( repo.MOD("confusion", .3f) )
      )
    );

    /* Interactions */

    repo.RegisterInteraction(
      new InfluencedInteraction(
        0,
        LS( repo.GetTrait(Factor.AGREEABLENESS) ),
        LS( repo.GetState("confusion") ),
        repo.GetAction(actionIDs[Random.Range(0, actionIDs.Count)])
      )
    );

    repo.RegisterInteraction(
      new InfluencedInteraction(
        1,
        LS( repo.GetTrait(Factor.NEUROTICISM) ),
        LS( repo.GetState("energy") ),
        repo.GetAction(actionIDs[Random.Range(0, actionIDs.Count)])
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
