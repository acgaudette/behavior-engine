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

  public MafiaRenderer() {
    ResetStep();
    lastTime = 0;
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

    foreach (ConsoleReader.Node line in root.children)
      if (line.data != "") Render.Print(line.data);

    string register = "CREW REGISTER";

    foreach (IEntity entity in entities) {
      Crewmember c = entity as Crewmember;
      register += "\n" + c.name + "...{" + c.role + "}";
    }

    Render.Print(register);
    Render.Print("[ BEGIN TRANSCRIPT ]");
  }

  public bool RenderKill(Crewmember victim, int cycle) {
    Step(0,
      "[ SHIP CYCLE " + cycle + " ]", log: false
    );

    if (Sleep(0)) return false;

    if (step == 1) Render.IncrementCycle();

    Step(1, "The crew files into the conference room.");

    if (Sleep(1)) return false;

    Step(2,
      "<color=red>" + victim.Title + " is missing.</color>",
      victim.name + " has been murdered."
    );

    return step == 3;
  }

  // Fallout
  public void RenderFallout() {
    //Step(0, "The crew files into the conference room.");
  }

  // Lynch
  public bool RenderLynch(Crewmember victim, Crewmember killer) {
    Step(0,
      "The group appears to be arguing and gesturing wildly",
      "They are arguing over the likely killer"
    );

    if (Sleep(0)) return false;

    Step(1,
      "<color=red>" + victim.Title + " is iced.</color>"
    );

    Step(2,
      "The group investigates " + victim.name + "'s body",
      "They are looking for evidence."
    );

    if (Sleep(2)) return false;

    bool match = victim == killer;
    string color = match ? "cyan" : "#fc4e4e";

    Step(3,
      match ?
        "Incriminating evidence is located"
        : "No evidence to incriminate " + victim.name + " is found.",
      "<color=" + color + ">" + victim.name + " "
      + (match ? "is" : "is NOT") + " the killer.</color>"
    );

    return step == 4;
  }

  // End
  public void RenderEnd(Crewmember killer, ICollection<IEntity> entities) {
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

      Step(0, s);
    }

    else {
      Crewmember last = null;
      foreach (IEntity e in entities) {
        last = e as Crewmember;
        break;
      }

      Step(0,
        "Everyone is dead--only " + last.name
        + ", the killer, remains."
      );
    }

    Step(1, "[ END TRANSCRIPT ]", log: false);
    if (step == 2) {
      Render.WriteToFile(Mafia.DATAPATH + "log.txt");
      step++;
    }
  }

  //
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

  //
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

}
