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

  public bool forceRandomResults = false;

  // Macro
  static List<T> LS<T>(params T[] p) {
    List<T> list = new List<T>();
    foreach (T t in p) list.Add(t);
    return list;
  }

  public const string DATAPATH
    = "./Assets/behavior-engine/examples/unity3d/mafia/data/";

  struct CrewmemberData {

    public readonly string firstName, lastName, role, pronoun;

    public CrewmemberData(
      string firstName, string lastName, string role, string pronoun
    ) {
      this.firstName = firstName;
      this.lastName = lastName;
      this.role = role;
      this.pronoun = pronoun;
    }
  }

  CrewmemberData[] crewmemberData = {
    new CrewmemberData("Jurgen", "Burgstaller", "Engineer", Crewmember.M),
    new CrewmemberData("Francis", "Bertrand", "First Mate", Crewmember.M),
    new CrewmemberData("Eugene", "Parsons", "Swabbie", Crewmember.M),
    new CrewmemberData("Peter", "Strickland", "Security", Crewmember.M),
    new CrewmemberData("Yasmin", "Pahlavi", "Navigator", Crewmember.F),
    new CrewmemberData("Leonie", "Cruz", "Pilot", Crewmember.F),
    new CrewmemberData("Micaela", "Valenti", "Quartermaster", Crewmember.F),
    new CrewmemberData("Nora", "Murphy", "Captain", Crewmember.F)
  };

  void Awake() { Initialize(); }

  void Initialize() {
    // Initialize environment
    Universe.root = new Universe();
    BrainRepository repo = new BrainRepository();

    // Characters
    Crewmember[] characters = GetCharacters();

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
        "anger", () => Random.Range(.1f, .25f),
        Transformations.EaseSquared()
      );
    repo.RegisterState(anger);
    states.Add(anger);

    // The higher the stress, the more likely people will be suspicious of
    // each other
    State stress
    = new State(
      "stress", () => Random.Range(.1f, .25f),
      Transformations.EaseSquared()
    );
    repo.RegisterState(stress);
    states.Add(stress);

    // The higher the confusion, the more likely energy will needlessly go down
    State confusion
      = new State(
      "confusion", () => Random.Range(.1f, .25f),
      Transformations.EaseSquared()
    );
    repo.RegisterState(confusion);
    states.Add(confusion);

    /* Relationships */
    /**
     * Randomly assigning pos/neg agree/trust for the purposes of
     * the relationship graph
     * Ideally, this should not be randomly assigned, perhaps chosen when
     * creating the characters
     */
    for (int i = 0; i < characters.Length; ++i) {

      var c = characters[i];
       
      List<State> negTrust = new List<State>();
      List<State> posTrust = new List<State>();
      List<State> posAgree = new List<State>();
      List<State> negAgree = new List<State>();

      //states[indexer];

      if(i < ((characters.Length * 3) / 4)) {
        negTrust.Add(repo.GetState("anger"));
        negAgree.Add(repo.GetState("anger"));
        posTrust.Add(repo.GetState("stress"));
        posAgree.Add(repo.GetState("stress"));
      } else {
        negTrust.Add(repo.GetState("stress"));
        negAgree.Add(repo.GetState("stress"));
        posTrust.Add(repo.GetState("anger"));
        posAgree.Add(repo.GetState("anger"));
      }

      c.agreementAffinities.RegisterPositive(posAgree);
      c.agreementAffinities.RegisterNegative(negAgree);

      c.trustAffinities.RegisterPositive(posTrust);
      c.trustAffinities.RegisterNegative(negTrust);
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
    var stress_on_self = 
      new InfluencedInteraction(
        0,
        LS(
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("stress")
        ),
        repo.GetAction("stress_on_self")
      );
    stress_on_self.SetDebugLabel("stress_on_self");

    var stress_on_other = 
      new InfluencedInteraction(
        1,
        LS(
          repo.GetTrait(Factor.NEUROTICISM)
        ),
        LS(
          repo.GetState("stress")
        ),
        repo.GetAction("stress_on_other")
      );
    stress_on_other.SetDebugLabel("stress_on_other");

    repo.RegisterInteraction(stress_on_other);

    var anger_on_other =
      new InfluencedInteraction(
        1,
        LS(
          repo.GetTrait(Factor.AGREEABLENESS)
        ),
        LS(
          repo.GetState("anger")
        ),
        repo.GetAction("anger_on_other")
      );
    anger_on_other.SetDebugLabel("anger_on_other");

    repo.RegisterInteraction(anger_on_other);

    // Attribution
    foreach (Character character in characters) {
      character.Subscribe();
      Universe.root.entities.Add(character);
    }


    // Manually force certain attributes
    for(int i = 0; i < characters.Length; ++i) {
      var crewmember = characters[i];
      foreach(var attr in crewmember.GetAttributeInstances()) {
        if(attr.Prototype.Equals(repo.GetState("anger"))) {
          var instance = attr as State.TransformedInstance;
          if(i < ((characters.Length * 3) / 4)) {
            instance.State = 0f;
          } else {
            instance.State = Random.Range(.85f, .95f);
          }
        }
      }
    }

    // Unity hooks
    ComponentManager hook = GetComponent<ComponentManager>();
    hook.Hook("Universe.root", Universe.root);
    hook.Hook<BrainRepoComponent>("brain-repo", repo);
  }

  Crewmember[] GetCharacters() {
    var crewmembers = new List<Crewmember>();

    if (forceRandomResults) {
      foreach (var data in crewmemberData) {
        crewmembers.Add(
          new RandomCrewmember(data.firstName, data.lastName, data.role, data.pronoun)
        );
      }
    } else {
      foreach (var data in crewmemberData) {
        crewmembers.Add(
          new Crewmember(data.firstName, data.lastName, data.role, data.pronoun)
        );
      }
    }

    return crewmembers.ToArray();
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
        repo.GetTrait(Factor.NEUROTICISM),
        repo.GetTrait(Factor.EXTRAVERSION)
      );
    }

    return null;
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
}
