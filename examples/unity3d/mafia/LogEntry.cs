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

    public string Render(Crewmember host, Crewmember target) {
      if (message == "?" || message == "...") return message;

      return host.name + " " + FormatString(message, host, target)
        + (target == null ? "" : " " + target.name)
        + FormatString(finisher, host, target);
    }

    // Fill in pronouns
    string FormatString(string s, Crewmember host, Crewmember target) {
      return s.Replace("%p", host.pronoun)
        .Replace("%tp", target.pronoun)
        .Replace("%s", " ");
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

    Render.Log(
      observations[i].Render(self, target),
      analyses[j].Render(self, target)
    );
  }
}
