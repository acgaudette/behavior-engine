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
  }

  Phrase[] phrases;

  public LogEntry(string id, Phrase[] phrases) {
    ID = id;
    this.phrases = phrases;
  }

  public virtual void Perform(CharacterActionInfo info) {
    int i = Random.Range(0, phrases.Length);

    Crewmember target = null;
    foreach (IEntity e in info.targets) {
      target = e as Crewmember;
      break;
    }

    Crewmember self = info.character as Crewmember;

    string observation = self.Title + " " + phrases[i].message
      + (target == null ? "" : " " + target.Title);
    string analysis = ""; // stub

    Render.Log(observation, analysis);
  }
}
