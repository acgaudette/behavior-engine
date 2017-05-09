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
    Crewmember[] characters = {
      new Crewmember("Jurgen", "Engineer", Crewmember.M),
      new Crewmember("Francis", "First Mate", Crewmember.M),
      new Crewmember("Eugene", "Swabbie", Crewmember.M),
      new Crewmember("Peter", "Security", Crewmember.M),
      new Crewmember("Yasmin", "Navigator", Crewmember.F),
      new Crewmember("Leonie", "Pilot", Crewmember.F),
      new Crewmember("Micaela", "Quartermaster", Crewmember.F),
      new Crewmember("Nora", "Captain", Crewmember.F)
    };

    // Link each character to the central repository
    foreach (Character character in characters)
      character.Repository = repo;

    /* Actions */

    ConsoleReader.Node root;
    ConsoleReader.LoadFile(
      DATAPATH + "/" + FILENAME, out root
    );

    Debug.Log(root);

    foreach (ConsoleReader.Node action in root.children) {
      if (action.children.Length == 0) {
        repo.RegisterAction(
          new LogEntry(
            action.data,
            new LogEntry.Phrase[] { new LogEntry.Phrase(action.data) }
          )
        );
      }

      else {
        LogEntry.Phrase[] phrases = new LogEntry.Phrase[
          action.children.Length
        ];

        for (int i = 0; i < action.children.Length; ++i) {
          phrases[i] = new LogEntry.Phrase(action.children[i].data);
        }

        repo.RegisterAction(new LogEntry(action.data, phrases));
      }
    }

    //List<string> actionIDs = new List<string>(repo.GetActionIDs());

    /* Attributes (Traits, States) */

    Trait.RegisterFactors(repo, Distributions.Normal());

    // The higher the anger, the rasher the actions
    repo.RegisterState(
      new State(
        "anger", () => Random.Range(0, .25f),
        Transformations.EaseSquared()
      )
    );

    // Need energy to perform certain actions
    repo.RegisterState(
      new State(
        "energy", () => 1,
        Transformations.Linear()
      )
    );

    // The higher the confusion, the more likely energy will needlessly go down
    repo.RegisterState(
      new State(
        "confusion", () => Random.Range(0, .25f),
        Transformations.InvertedSquared()
      )
    );

    // The higher the stress, the more likely people will be suspicious of
    // each other
    repo.RegisterState(
      new State(
        "stress", () => Random.Range(0, .25f),
        Transformations.InvertedSquared()
      )
    );

    /* Effects */

    foreach (IAttribute prototype in repo.AttributePrototypes) {
      if (prototype is State) {
        for (float o = -.3f; o < .4f; o += .15f) {
          if (o == 0) continue;

          State state = prototype as State;

          repo.Effects.Add(
            new InfluencedEffect(
              (o > 0 ? "raise-" : "lower-")
                + state.name + "-"
                + (Mathf.Abs(o) > .2f ? "strong" : "weak"),
              TraitLinksFromState(state, repo),
              LS(state),
              LS(repo.MOD(state.name, o))
            )
          );
        }
      }
    }


    /* Interactions */

    // Accuse
    var accuse = 
      new InfluencedInteraction(
        1,
        LS(
          repo.GetTrait(Factor.AGREEABLENESS),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("anger"),
          repo.GetState("stress")
        ),
        repo.GetAction("accuse")
      );
    accuse.SetDebugLabel("accuse");

    // Investigate
    var investigate =
      new InfluencedInteraction(
        1,
        LS(
          repo.GetTrait(Factor.EXTRAVERSION),
          repo.GetTrait(Factor.OPENNESS)
        ),
        LS(
          repo.GetState("energy")
        ),
        repo.GetAction("investigate")
      );
    investigate.SetDebugLabel("investigate");

    // Despair
    InfluencedInteraction despair =
      new InfluencedInteraction(
        0,
        LS(
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("stress")
        ),
        repo.GetAction("despair")
      );
    despair.SetDebugLabel("despair");

    // Hallucinate
    var hallucinate =
      new InfluencedInteraction(
        0,
        LS(
          repo.GetTrait(Factor.EXTRAVERSION),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("energy"),
          repo.GetState("confusion"),
          repo.GetState("stress")
        ),
        repo.GetAction("hallucinate")
      );
    hallucinate.SetDebugLabel("hallucinate");

    // Sleep
    var sleep =
      new InfluencedInteraction(
        0,
        LS(
          repo.GetTrait(Factor.CONSCIENTIOUSNESS)
        ),
        LS(
          repo.GetState("energy")
        ),
        repo.GetAction("sleep")
      );
    sleep.SetDebugLabel("sleep");

    // Attack
    var attack =
      new InfluencedInteraction(
        1,
        LS(
          repo.GetTrait(Factor.AGREEABLENESS),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("energy")
        ),
        repo.GetAction("attack")
      );
    attack.SetDebugLabel("attack");

    repo.RegisterInteraction(accuse);
    repo.RegisterInteraction(investigate);
    repo.RegisterInteraction(despair);
    repo.RegisterInteraction(hallucinate);
    repo.RegisterInteraction(sleep);
    repo.RegisterInteraction(attack);

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

  IEnumerable<Trait> TraitLinksFromState(State state, BrainRepository repo) {
    if (state.name == "anger") {
      return LS(
        repo.GetTrait(Factor.AGREEABLENESS),
        repo.GetTrait(Factor.CONSCIENTIOUSNESS),
        repo.GetTrait(Factor.NEUROTICISM)
      );
    }

    if (state.name == "energy") {
      return LS(
        repo.GetTrait(Factor.EXTRAVERSION),
        repo.GetTrait(Factor.OPENNESS)
      );
    }

    if (state.name == "confusion") {
      return LS(
        repo.GetTrait(Factor.CONSCIENTIOUSNESS)
      );
    }

    if (state.name == "stress") {
      return LS(
        repo.GetTrait(Factor.NEUROTICISM)
      );
    }

    return null;
  }
}
