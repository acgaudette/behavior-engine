// MafiaUniverseComponent.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;

public class MafiaUniverseComponent : UniverseComponent {

  public enum Stage { KILL, FALLOUT, LYNCH };
  public Stage stage = Stage.KILL;

  public ulong falloutTicks = 10;
  ulong lastTick = 0;

  protected override void Start() {
    base.Start();
    ReplaceEntities();
    ChangeStage(stage);
  }

  // State machine
  protected override void Update() {
    switch (stage) {
      case Stage.KILL:
        if (DoKillStage())
          ChangeStage(Stage.FALLOUT);
        break;

      case Stage.FALLOUT:
        if (DoFalloutStage())
          ChangeStage(Stage.LYNCH);
        break;

      case Stage.LYNCH:
        if (DoLynchStage())
          ChangeStage(Stage.KILL);
        break;
    }
  }

  // Initialize variables for each state
  void ChangeStage(Stage to) {
    switch (to) {
      case Stage.FALLOUT:
        lastTick = tick;
        Debug.Log("Fallout stage");
        break;

      case Stage.LYNCH:
        Debug.Log("Lynch stage");
        break;

      case Stage.KILL:
        Debug.Log("Kill stage");
        break;
    }

    stage = to;
  }

  bool DoKillStage() {
    return true;
  }

  bool DoFalloutStage() {
    base.Update();
    return tick > lastTick + falloutTicks;
  } 
  bool DoLynchStage() {
    return true;
  }
}
