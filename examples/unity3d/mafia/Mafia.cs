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
    MafiaCharacter[] characters = {
      new MafiaCharacter("Jurgen"),
      new MafiaCharacter("Francis"),
      new MafiaCharacter("Eugene")
    };

    // Link each character to the central repository
    foreach (Character character in characters)
      character.Repository = repo;

    /* Actions */

    ConsoleActionReader.LoadFile(
      DATAPATH + "/" + FILENAME, repo
    );

    List<string> actionIDs = new List<string>(repo.GetActionIDs());

    /* Attributes (Traits, States) */

    Trait.RegisterFactors(repo, Distributions.Normal());

    // The higher the anger, the rasher the actions
    repo.RegisterState(
      new State(
        "anger",
        Distributions.Uniform(),
        Transformations.EaseSquared()
      )
    );

    // The higher the confusion, the more likely energy will needlessly go down
    repo.RegisterState(
      new State(
        "confusion",
        Distributions.Uniform(),
        Transformations.InvertedSquared()
      )
    );

    // Need energy to perform certain actions
    repo.RegisterState(
      new State(
        "energy", () => 1,
        Transformations.Linear()
      )
    );

    // The higher the stress, the more likely people will be suspicious of
    // each other
    repo.RegisterState(
      new State(
        "stress", Distributions.Uniform(),
        Transformations.InvertedSquared()
      )
    );

    /* Effects */

    repo.Effects.Add(
      new InfluencedEffect(
        "accuse",
        LS(
          repo.GetTrait(Factor.AGREEABLENESS),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("anger"),
          repo.GetState("stress")
        ),
        LS(
          repo.MOD("anger", .1f),
          repo.MOD("confusion", -.3f)
        )
      )
    );

    repo.Effects.Add(
      new InfluencedEffect(
        "despair",
        LS(
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("stress")
        ),
        LS(
          repo.MOD("energy", -.2f),
          repo.MOD("anger", .2f),
          repo.MOD("stress", .2f)
        )
      )
    );

    repo.Effects.Add(
      new InfluencedEffect(
        "sleep",
        LS(
          repo.GetTrait(Factor.CONSCIENTIOUSNESS)
        ),
        LS(
          repo.GetState("energy")
        ),
        LS(
          repo.MOD("energy", .3f),
          repo.MOD("confusion", -.1f)
        )
      )
    );

    repo.Effects.Add(
      new InfluencedEffect(
        "investigate",
        LS(
          repo.GetTrait(Factor.EXTRAVERSION),
          repo.GetTrait(Factor.OPENNESS)
        ),
        LS(
          repo.GetState("energy")
        ),
        LS(
          repo.MOD("energy", -.3f),
          repo.MOD("confusion", -.2f),
          repo.MOD("stress", .1f)
        )
      )
    );

    repo.Effects.Add(
      new InfluencedEffect(
        "attack",
        LS(
          repo.GetTrait(Factor.AGREEABLENESS),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("energy")
        ),
        LS(
          repo.MOD("energy", -.2f),
          repo.MOD("anger", -.2f),
          repo.MOD("stress", .3f)
        )
      )
    );

    repo.Effects.Add(
      new InfluencedEffect(
        "hallucinate",
        LS(
          repo.GetTrait(Factor.EXTRAVERSION),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("energy"),
          repo.GetState("confusion"),
          repo.GetState("stress")
        ),
        LS(
          repo.MOD("energy", -.2f),
          repo.MOD("anger", .2f),
          repo.MOD("stress", .3f),
          repo.MOD("confusion", .2f)
        )
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
