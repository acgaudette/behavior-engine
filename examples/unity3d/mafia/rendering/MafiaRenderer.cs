// MafiaRenderer.cs
// Created by Aaron C Gaudette on 04.05.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;

[System.Serializable]
public class MafiaRenderer {

  public float sleepTime = 0;

  int step;
  float lastTime;

  Dictionary<string, string[]> messageTable;

  public MafiaRenderer() {
    ResetStep();
    lastTime = 0;
  }

  public void Load() {
    ConsoleReader.Node messages;
    if (!ConsoleReader.LoadFile(
      Mafia.DATAPATH + "messages.txt", out messages
    )) {
      Debug.LogError("MafiaRenderer: Invalid message data");
    }

    messageTable = new Dictionary<string, string[]>();

    foreach (var message in messages)
      messageTable[message.data] = message.ChildrenToString();
  }

  public void ResetStep() {
    step = 0;
  }

  public void Mark(float t) {
    lastTime = t;
  }

  public void RenderStart(ICollection<IEntity> entities) {
    ConsoleReader.Node root;
    ConsoleReader.LoadFile(Mafia.DATAPATH + "foreword.txt", out root);

    foreach (ConsoleReader.Node line in root)
      if (line.data != "") Render.Print(line.data);

    string register = "CREW REGISTER";

    foreach (IEntity entity in entities) {
      Crewmember c = entity as Crewmember;
      register += "\n" + c.name + ", " + c.firstName + " ... {" + c.role + "}";
    }

    Render.Print(register);
    Render.Print("[ BEGIN TRANSCRIPT ]");
  }

  public bool RenderKill(
    Crewmember victim, int cycle, int offset, Crewmember lastKiller
  ) {
    Step(0,
      "[ SHIP CYCLE " + (cycle + offset) + " ]", log: false
    );

    if (Sleep(0)) return false;

    if (step == 1) Render.IncrementCycle();

    Step(1,
      (lastKiller != null ? lastKiller.name : "The crew")
      + " " + RandomMessage("enter")
    );

    if (Sleep(1)) return false;

    Step(2,
      "<color=red>" + victim.Title + " "
        + RandomMessage("missing") + "</color>",
      cycle == 1 ? "?" : victim.name + " has been murdered."
    );

    return step == 3;
  }

  // Fallout
  public void RenderFallout() { }

  // Lynch
  public bool RenderLynch(
    Crewmember victim, Crewmember killer,
    Dictionary<Crewmember, Crewmember> accusations
  ) {
    Step(0,
      "The group appears to be arguing and gesturing wildly",
      "They are arguing over the likely killer"
    );

    string a = "";
    foreach (Crewmember member in accusations.Keys) {
      a += "\n" + member.name + " points at " + accusations[member].name;
    }

    Step(1, a);

    if (Sleep(1)) return false;

    Step(2,
      "<color=red>" + victim.Title + " is iced.</color>"
    );

    Step(3,
      "The group investigates " + victim.name + "'s body",
      "They are looking for evidence."
    );

    if (Sleep(3)) return false;

    bool match = victim == killer;
    string color = match ? "cyan" : "#fc4e4e";

    Step(4,
      match ?
        RandomMessage("evidence")
        : "No evidence to incriminate " + victim.name + " is found.",
      "<color=" + color + ">" + victim.name + " "
      + (match ? "is" : "is NOT") + " the killer.</color>"
    );

    return step == 5;
  }

  // End
  public void RenderEnd(
    Crewmember killer, Crewmember last, ICollection<IEntity> entities
  ) {
    Step(0, last.name + " reaches for the camera and breaks the connection");

    if ((killer as IDestroyable).Destroy) {
      bool single = entities.Count == 1;
      string s = "The killer, " + killer.name + ", is dead--only ";

      int i = 0;
      foreach (IEntity e in entities) {
        if (!single && i == entities.Count - 1)
          s += " and ";

        s += (e as Crewmember).Title;

        if (!single && i < entities.Count - 2)
          s += ", ";

        i++;
      }

      s += " " + (single ? "remains" : "remain") + ".";

      Step(1, s);
    }

    else {
      Step(1,
        "Everyone is dead--only " + last.name
        + ", the killer, remains."
      );
    }

    Step(2, "[ END TRANSCRIPT ]", log: false);

    if (step == 3) {
      Render.WriteToFile(Mafia.DATAPATH + "log.txt");
      step++;
    }
  }

  // Render a specific message at a step
  void Step(
    int s, string message, string analysis = "...", bool log = true
  ) {
    if (step == s) {
      if (log) {
        Render.Log("<b>" + message + "</b>", analysis);
      }

      else Render.Print(message);

      step++;
    }
  }

  // Pause rendering at a step
  bool Sleep(int s, float t = -1) {
    t = t < 0 ? sleepTime : t;

    if (step != s + 1)
      return false;

    if (Time.time > lastTime + t) {
      lastTime = Time.time;
      return false;
    }

    return true;
  }

  // Select a random message from a message key in the message table
  string RandomMessage(string key) {
    if (!messageTable.ContainsKey(key))
      return null;

    return messageTable[key][Random.Range(0, messageTable[key].Length)];
  }
}
