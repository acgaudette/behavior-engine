// MafiaUniverseComponent.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;
using BehaviorEngine;

public class MafiaUniverseComponent : UniverseComponent {

  public enum Stage { KILL, FALLOUT, LYNCH, END };
  public Stage stage = Stage.KILL;
  [HideInInspector] public MafiaCharacter killer;
  public float sleepTime = 1.5f;

  public ulong falloutTicks = 4;

  MafiaCharacter victim = null;
  ulong lastTick = 0;
  float lastTime = 0;
  int step = 0;
  int day = 0;

  protected override void Start() {
    base.Start();

    ReplaceEntities();
    killer = SelectRandom();
    ChangeStage(stage);
  }

  // Initialize variables for each state
  void ChangeStage(Stage to) {
    switch (to) {
      case Stage.FALLOUT:
        lastTick = tick;
        break;

      case Stage.LYNCH:
        victim = SelectRandom();
        break;

      case Stage.KILL:
        day++;
        victim = SelectRandom(killer);
        break;

      case Stage.END:
        break;
    }

    lastTime = Time.time;
    step = 0;
    stage = to;
  }

  bool UpdateKillStage() {
    if (RenderKill(victim)) {
      Kill(victim);
      return true;
    }

    return false;
  }

  bool UpdateFalloutStage() {
    Step(0, "<b>The group gathers together...\n</b>");

    if (tick >= lastTick + falloutTicks) {
      if (tick == lastTick + falloutTicks)
        return trigger.Ready; // Trigger last step before next stage
    } else base.Update();

    return false;
  } 

  bool UpdateLynchStage() {
    if (RenderLynch(victim)) {
      Kill(victim);
      return true;
    }

    return false;
  }

  void UpdateEndStage() {
    if ((killer as IDestroyable).Destroy) {
      bool single = reference.entities.Count == 1;
      string s = "<b>The killer, " + killer.name + ", is dead--only ";

      int i = 0;
      foreach (IEntity e in reference.entities) {
        if (!single && i == reference.entities.Count - 1)
          s += " and ";

        s += (e as MafiaCharacter).name;

        if (!single && i < reference.entities.Count - 2)
          s += ", ";

        i++;
      }

      s += " " + (single ? "remains" : "remain") + ".\n</b>";

      Step(0, s);
    }

    else {
      MafiaCharacter last = SelectRandom();
      Step(0,
        "<b>Everyone is dead--only " + last.name
        + ", the killer, remains.\n</b>"
      );
    }
  }

  bool RenderKill(MafiaCharacter victim) {
    Step(0,
      "<b><color=white><size=18>Day "
      + day + "</size></color></b>\n"
    );

    if (Sleep(1)) return false;

    Step(1, "<b>The KILLER goes on the hunt...\n</b>");

    if (Sleep(2)) return false;

    Step(2,
      "<b><color=red>" + victim.name
      + " is murdered!\n</color></b>"
    );

    return step == 3;
  }

  bool RenderLynch(MafiaCharacter victim) {
    Step(0, "<b>The group deliberates over the likely killer...\n</b>");

    if (Sleep(1)) return false;

    Step(1,
      "<b><color=red>" + victim.name
      + " is thrown from the roof!\n</color></b>"
    );

    Step(2,
      "<b>The group investigates the evidence on " + victim.name
      + "'s body...\n</b>"
    );

    if (Sleep(3)) return false;

    bool match = victim == killer;
    string color = match ? "cyan" : "#fc4e4e";

    Step(3,
      "<color=" + color + ">" + victim.name + " <b>"
      + (match ? "IS" : "IS NOT")
      + "</b> the killer!\n</color>"
    );

    return step == 4;
  }

  MafiaCharacter SelectRandom(IEntity avoid = null) {
    IEntity first = null;
    foreach (IEntity e in reference.entities) {
      first = e;
      break;
    }

    // Exit early if there are no Entities or if the only Entity is the avoid
    if (first == null || (reference.entities.Count == 1 && first == avoid))
      return null;

    IEntity target = null;

    do {
      int index = Random.Range(0, reference.entities.Count);
      int i = 0;
      foreach (IEntity e in reference.entities) {
        if (i == index) {
          target = e;
          break;
        }
        i++;
      }
    } while (target == avoid as MafiaCharacter);

    return target as MafiaCharacter;
  }

  void Kill(IDestroyable d) {
    d.Destroy = true;
    ReplaceEntities();
  }

  void Step(int s, string message) {
    if (step == s) {
      Debug.Log(message);
      step++;
    }
  }

  bool Sleep(int s, float t = -1) {
    t = t < 0 ? sleepTime : t;

    if (step != s)
      return false;

    if (Time.time > lastTime + t) {
      lastTime = Time.time;
      return false;
    }

    return true;
  }

  // State machine
  protected override void Update() {
    //TODO: two-person case
    if (
      stage != Stage.END
      && (reference.entities.Count == 1 || (killer as IDestroyable).Destroy)
    ) {
      ChangeStage(Stage.END);
    }

    switch (stage) {
      case Stage.KILL:
        if (UpdateKillStage())
          ChangeStage(Stage.FALLOUT);
        break;

      case Stage.FALLOUT:
        if (UpdateFalloutStage())
          ChangeStage(Stage.LYNCH);
        break;

      case Stage.LYNCH:
        if (UpdateLynchStage())
          ChangeStage(Stage.KILL);
        break;

      case Stage.END:
        UpdateEndStage();
        break;
    }
  }
}
