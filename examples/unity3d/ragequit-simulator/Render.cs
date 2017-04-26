// Render.cs
// Created by Aaron C Gaudette on 10.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Debug;
using BehaviorEngine.Float;

public partial class User : Entity, IDestroyable {

  // Helper method
  static string SenderDebugLabel(object sender) {
    return (sender as ILabeled).GetDebugLabel();
  }

  EntityEvents.OnReactEventHandler RenderReaction = (
    object sender,
    Interaction interaction, IEntity host, IList<Effect> effects
  ) => {
    if (effects == null) return;

    string suffix = " by " + host.GetDebugLabel() + "'s flame message";

    if (effects.Contains(Forum.annoy))
      Debug.Log((sender as ILabeled).GetDebugLabel() + " is frustrated" + suffix);

    else if (effects.Contains(Forum.incite))
      Debug.Log(
        "<color=#fc4e4e>" + SenderDebugLabel(sender)
          + " is enraged" + suffix + "</color>"
      );
  };

  EntityEvents.OnObserveEventHandler RenderObservation = (
    object sender,
    Interaction interaction, IEntity host,
    ICollection<IEntity> targets, IList<Effect> effects
  ) => {
    if (effects == null) return;

    string suffix = interaction == Forum.start ?
      " after reading " + host.GetDebugLabel() + "'s post"
      : " after " + host.GetDebugLabel() + "'s massive rage";

    if (
      effects.Contains(Forum.calm)
      && (sender as User).GetAttributeState(Forum.anger) > 0
    ) {
      if (interaction == Forum.start) {
        Debug.Log(
          "<color=cyan>" + SenderDebugLabel(sender)
            + " calms down" + suffix + "</color>"
        );
      } else {
        Debug.Log(
          "<color=cyan>" + SenderDebugLabel(sender)
            + " feels pretty great" + suffix + "</color>"
        );
      }
    }

    else if (effects.Contains(Forum.annoy))
      Debug.Log(SenderDebugLabel(sender) + " is annoyed" + suffix);

    else if (effects.Contains(Forum.incite))
      Debug.Log(
        "<color=#fc4e4e>" + SenderDebugLabel(sender)
          + " is totally triggered" + suffix + "</color>"
      );
  };

  EntityEvents.OnPollEventHandler RenderPoll = (
    object sender,
    Interaction choice, ICollection<IEntity> targets, float highscore
  ) => {
    if (choice == Forum.quit)
      (sender as User).Ragequit();
    else if (Universe.root.entities.Count == 1)
      (sender as User).Logoff();

    else if (choice == Forum.start) {
      float anger = (sender as User).GetAttributeState(Forum.anger);
      Debug.Log(
        SenderDebugLabel(sender) + " starts a new thread, "
          + ((anger > .66f) ?
            "super angry" : (anger > .33f) ?
            "pretty controversial" : "easy topic")
      );
    }

    else if (choice == Forum.flame) {
      IEntity e = null;
      foreach (IEntity target in targets) { e = target; break; }

      Debug.Log(
        "<color=orange>" + SenderDebugLabel(sender)
          + " sends an angry DM to " + e.GetDebugLabel() + "!</color>"
      );
    }
  };

  public void HookRenderer() {
    OnReact += RenderReaction;
    OnObserve += RenderObservation;
    OnPoll += RenderPoll;
  }

  void Ragequit() {
    Debug.Log(
      "<b><color=red>" + GetDebugLabel() + " flipped out and ragequit!</color></b>"
    );
    Destroy = true;
  }

  void Logoff() {
    Debug.Log(
      "<b>Nobody is left, so " + GetDebugLabel() + " logs off the server</b>"
    );
    Destroy = true;
  }
}
