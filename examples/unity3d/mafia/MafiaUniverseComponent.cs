// MafiaUniverseComponent.cs
// Created by Aaron C Gaudette on 01.05.17

using System.Collections.Generic;

using UnityEngine;
using BehaviorEngine;

public class MafiaUniverseComponent : UniverseComponent {

  public enum Stage { KILL, FALLOUT, LYNCH, END };
  public Stage stage = Stage.KILL;
  public MafiaRenderer mafiaRenderer = new MafiaRenderer();
  public ulong falloutTicks = 4;
  [HideInInspector] public Crewmember killer;

  Crewmember victim = null;
  Dictionary<Crewmember, Crewmember> accusations;
  ulong lastTick = 0;
  int cycle = 0;

  protected override void Start() {
    base.Start();
    cycle = Random.Range(999, 9999);

    ReplaceEntities();
    killer = SelectRandom();
    ChangeStage(stage);

    // Render
    mafiaRenderer.RenderStart(reference.entities);
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
        victim = SelectByVote();
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
    if (mafiaRenderer.RenderLynch(victim, killer, accusations)) {
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

  Crewmember SelectByVote() {
    Dictionary<Crewmember, int> votes = new Dictionary<Crewmember, int>();
    accusations = new Dictionary<Crewmember, Crewmember>();

    foreach(IEntity e in reference.entities) {
      // Skip dead Entities
      if ((e as IDestroyable).Destroy)
        continue;

      var member = e as Crewmember;
      Crewmember target = member.ChooseVote();
      accusations[member] = target;

      if(!votes.ContainsKey(target)) {
        votes[target] = 1;
      } else {
        votes[target]++;
      }
    }

    Crewmember selected = null;
    int best = -1;

    foreach(var entry in votes) {
      if(entry.Value > best) {
        selected = entry.Key;
        best = entry.Value;
      }
    }

    return selected;
  }

  void Kill(IDestroyable d) {
    d.Destroy = true;
    ReplaceEntities();
  }
}
