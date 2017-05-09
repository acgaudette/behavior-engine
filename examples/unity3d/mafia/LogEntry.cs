// LogEntry.cs
// Created by Aaron C Gaudette on 24.04.17

using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;

public class LogEntry : ICharacterAction {

  const string ENDL = "\n";

  public string ID { get; set; }

  public struct Phrase {
    public string message;
    public string finisher;

    public Phrase(string message, string finisher = "") {
      this.message = message;
      this.finisher = finisher;
    }

    public string Render(string name, string pronoun, string target) {
      if (message == "?" || message == "...") return message;

      string m = message.Replace("%p", pronoun); // Fill in pronoun
      target = string.IsNullOrEmpty(target) ? "" : " " + target;
      return name + " " + m + target + " " + finisher;
    }
  }

  Phrase[] observations;
  Phrase[] analyses;

  public LogEntry(string id, Phrase[] observations, Phrase[] analyses) {
    ID = id;
    this.observations = observations;
    this.analyses = analyses;
  }

  public virtual void Perform(CharacterActionInfo info) {
    int i = Random.Range(0, observations.Length);
    int j = Random.Range(0, analyses.Length);

    Crewmember self = info.character as Crewmember, target = null;

    foreach (IEntity e in info.targets) {
      target = e as Crewmember;
      break;
    }

    string targetName = target == null ? "" : target.name;

    Render.Log(
      observations[i].Render(self.name, self.pronoun, targetName),
      analyses[j].Render(self.name, self.pronoun, targetName)
    );
  }
}
