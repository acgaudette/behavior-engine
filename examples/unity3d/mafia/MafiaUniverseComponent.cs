// MafiaUniverseComponent.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;
using BehaviorEngine;

public class MafiaUniverseComponent : UniverseComponent {

  public enum Stage { KILL, FALLOUT, LYNCH, END };
  public Stage stage = Stage.KILL;
  public MafiaRenderer mafiaRenderer = new MafiaRenderer();
  public ulong falloutTicks = 4;
  [HideInInspector] public Crewmember killer;

  Crewmember victim = null;
  ulong lastTick = 0;
  int cycle = 0;

  protected override void Start() {
    base.Start();
    cycle = Random.Range(999, 9999);

    ReplaceEntities();
    killer = SelectRandom();
    ChangeStage(stage);

    // Render
    mafiaRenderer.RenderStart();
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
        cycle++;
        victim = SelectRandom(killer);
        break;

      case Stage.END:
        break;
    }

    mafiaRenderer.Mark(Time.time);
    mafiaRenderer.ResetStep();
    stage = to;
  }

  // Kill
  bool UpdateKillStage() {
    if (mafiaRenderer.RenderKill(victim, cycle)) {
      Kill(victim);
      return true;
    }

    return false;
  }

  // Fallout
  bool UpdateFalloutStage() {
    mafiaRenderer.RenderFallout();

    if (tick >= lastTick + falloutTicks) {
      if (tick == lastTick + falloutTicks)
        return trigger.Ready; // Trigger last step before next stage
    } else base.Update();

    return false;
  } 

  // Lynch
  bool UpdateLynchStage() {
    if (mafiaRenderer.RenderLynch(victim, killer)) {
      Kill(victim);
      return true;
    }

    return false;
  }

  // End
  void UpdateEndStage() {
    mafiaRenderer.RenderEnd(killer, reference.entities);
  }

  Crewmember SelectRandom(IEntity avoid = null) {
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
    } while (target == avoid as Crewmember);

    return target as Crewmember;
  }

  void Kill(IDestroyable d) {
    d.Destroy = true;
    ReplaceEntities();
  }
}
