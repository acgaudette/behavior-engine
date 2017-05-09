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

  public const string DATAPATH
    = "./Assets/behavior-engine/examples/unity3d/mafia/data/";

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    BrainRepository repo = new BrainRepository();

    // Characters
    Crewmember[] characters = {
      new Crewmember("Jurgen", "Burgstaller", "Engineer", Crewmember.M),
      new Crewmember("Francis", "Bertrand", "First Mate", Crewmember.M),
      new Crewmember("Eugene", "Parsons", "Swabbie", Crewmember.M),
      new Crewmember("Peter", "Strickland", "Security", Crewmember.M),
      new Crewmember("Yasmin", "Pahlavi", "Navigator", Crewmember.F),
      new Crewmember("Leonie", "Cruz", "Pilot", Crewmember.F),
      new Crewmember("Micaela", "Valenti", "Quartermaster", Crewmember.F),
      new Crewmember("Nora", "Murphy", "Captain", Crewmember.F)
    };

    // Link each character to the central repository
    foreach (Character character in characters)
      character.Repository = repo;

    /* Actions */

    if (!LoadActions("observations.txt", "analyses.txt", repo)) {
      Debug.LogError("Mafia: Invalid Action data");
      return;
    }

    //List<string> actionIDs = new List<string>(repo.GetActionIDs());

    /* Attributes (Traits, States) */

    Trait.RegisterFactors(repo, Distributions.Normal());

    List<State> states = new List<State>();

    // The higher the anger, the rasher the actions
    State anger = 
      new State(
        "anger", () => Random.Range(0, .25f),
        Transformations.EaseSquared()
      );
    repo.RegisterState(anger);
    states.Add(anger);

    // Need energy to perform certain actions
    State energy = new State(
      "energy", () => 1,
      Transformations.Linear()
    );
    repo.RegisterState(energy);
    states.Add(energy);

    // The higher the confusion, the more likely energy will needlessly go down
    State confusion
      = new State(
      "confusion", () => Random.Range(0, .25f),
      Transformations.InvertedSquared()
    );
    repo.RegisterState(confusion);
    states.Add(confusion);

    // The higher the stress, the more likely people will be suspicious of
    // each other
    State stress
    = new State(
        "stress", () => Random.Range(0, .25f),
        Transformations.InvertedSquared()
      );
    repo.RegisterState(stress);
    states.Add(stress);

    /**
     * Randomly assigning pos/neg agree/trust for the purposes of
     * the relationship graph
     * Ideally, this should not be randomly assigned, perhaps chosen when
     * creating the characters
     */
    foreach(Crewmember c in characters) {
      List<State> negAgree = new List<State>();
      List<State> posAgree = new List<State>();
      List<State> negTrust = new List<State>();
      List<State> posTrust = new List<State>();
      foreach(State s in states) {
        if(Random.Range(0f, 1f) < .3f) {
          negAgree.Add(s);
        }
      }

      foreach(State s in states) {
        if(Random.Range(0f, 1f) < .3f) {
          if(!negAgree.Contains(s)) {
            posAgree.Add(s);
          }
        }
      }

      foreach(State s in states) {
        if(Random.Range(0f, 1f) < .3f) {
          negTrust.Add(s);
        }
      }

      foreach(State s in states) {
        if(Random.Range(0f, 1f) < .3f) {
          if(!negTrust.Contains(s)) {
            posTrust.Add(s);
          }
        }
      }
      c.registerNegativeAgree(negAgree);
      c.registerPositiveAgree(posAgree);
      c.registerNegativeTrust(negTrust);
      c.registerPositiveTrust(posTrust);
    }

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
    var highangerhighstress_onother = 
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
        repo.GetAction("highangerhighstress_onother")
      );
    highangerhighstress_onother.SetDebugLabel("highangerhighstress_onother");

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
          repo.GetState("stress"),
          repo.GetState("confusion")
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
    var highenergyhighanger_onother =
      new InfluencedInteraction(
        1,
        LS(
          repo.GetTrait(Factor.AGREEABLENESS),
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("energy"),
          repo.GetState("anger")
        ),
        repo.GetAction("highenergyhighanger_onother")
      );
    highenergyhighanger_onother.SetDebugLabel("highenergyhighanger_onother");

    repo.RegisterInteraction(highangerhighstress_onother);
    repo.RegisterInteraction(investigate);
    repo.RegisterInteraction(despair);
    repo.RegisterInteraction(hallucinate);
    repo.RegisterInteraction(sleep);
    repo.RegisterInteraction(highenergyhighanger_onother);

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

  // Load observations/analyses files into repository Actions
  bool LoadActions(
    string observationsFilename, string analysesFilename,
    BrainRepository repo
  ) {
    ConsoleReader.Node actionObservations, actionAnalyses;
    bool valid = true;

    valid &= ConsoleReader.LoadFile(
      DATAPATH + observationsFilename, out actionObservations
    );
    valid &= ConsoleReader.LoadFile(
      DATAPATH + analysesFilename, out actionAnalyses
    );
    valid &= actionObservations.children.Length == actionAnalyses.children.Length;

    if (!valid) return false;

    for (int i = 0; i < actionObservations.children.Length; ++i) {
      ConsoleReader.Node observation = actionObservations.children[i];
      ConsoleReader.Node analysis = actionAnalyses.children[i];

      int observationCount = observation.children.Length;
      int analysisCount = analysis.children.Length;

      // Actions must have child nodes
      if (observationCount > 0 && analysisCount > 0) {
        LogEntry.Phrase[] observations
          = new LogEntry.Phrase[observationCount];
        LogEntry.Phrase[] analyses
          = new LogEntry.Phrase[analysisCount];

        for (int j = 0; j < observationCount; ++j) {
          string finisher = observation.children[j].children.Length > 0 ?
            observation.children[j].children[0].data : "";

          observations[j] = new LogEntry.Phrase(
            observation.children[j].data, finisher
          );
        }

        for (int k = 0; k < analysisCount; ++k) {
          string finisher = analysis.children[k].children.Length > 0 ?
            analysis.children[k].children[0].data : "";

          analyses[k] = new LogEntry.Phrase(
            analysis.children[k].data, finisher
          );
        }

        repo.RegisterAction(
          new LogEntry(observation.data, observations, analyses)
        );
      }
    }

    return true;
  }

  // Get the Traits associated with a given State
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
