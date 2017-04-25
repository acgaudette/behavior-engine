// Render.cs
// Created by Aaron C Gaudette on 10.04.17
// Comment this out to use the basic Extensions debug information

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public partial class User : UnityEntity {

  protected override void OnReact(
    Interaction interaction, Entity host, IList<Effect> effects
  ) {
    if (effects == null) return;

    string suffix = " by " + host.GetLabel() + "'s flame message";

    if (effects.Contains(Forum.annoy))
      Debug.Log(GetLabel() + " is frustrated" + suffix);

    else if (effects.Contains(Forum.incite))
      Debug.Log(
        "<color=#fc4e4e>" + GetLabel() + " is enraged" + suffix + "</color>"
      );
  }

  protected override void OnObserve(
    Interaction interaction, Entity host,
    ICollection<Entity> targets, IList<Effect> effects
  ) {
    if (effects == null) return;

    string suffix = interaction == Forum.start ?
      " after reading " + host.GetLabel() + "'s post"
      : " after " + host.GetLabel() + "'s massive rage";

    if (effects.Contains(Forum.calm) && GetAttributeState(Forum.anger) > 0) {
      if (interaction == Forum.start) {
        Debug.Log(
          "<color=cyan>" + GetLabel() + " calms down" + suffix + "</color>"
        );
      } else {
        Debug.Log(
          "<color=cyan>" + GetLabel() + " feels pretty great" + suffix + "</color>"
        );
      }
    }

    else if (effects.Contains(Forum.annoy))
      Debug.Log(GetLabel() + " is annoyed" + suffix);

    else if (effects.Contains(Forum.incite))
      Debug.Log(
        "<color=#fc4e4e>" + GetLabel() + " is totally triggered"
        + suffix + "</color>"
      );
  }

  protected override void OnPoll(
    Interaction choice, ICollection<Entity> targets, float highscore
  ) {
    if (choice == Forum.quit)
      Ragequit();
    else if (Universe.root.entities.Count == 1)
      Logoff();

    else if (choice == Forum.start) {
      float anger = GetAttributeState(Forum.anger);
      Debug.Log(
        GetLabel() + " starts a new thread, " + ((anger > .66f) ?
          "super angry" : (anger > .33f) ?
          "pretty controversial" : "easy topic")
      );
    }

    else if (choice == Forum.flame) {
      Entity e = null;
      foreach (Entity target in targets) { e = target; break; }

      Debug.Log(
        "<color=orange>" + GetLabel() + " sends an angry DM to "
        + e.GetLabel() + "!</color>"
      );
    }
  }

  void Ragequit() {
    Debug.Log(
      "<b><color=red>" + GetLabel() + " flipped out and ragequit!</color></b>"
    );
    destroy = true;
  }

  void Logoff() {
    Debug.Log(
      "<b>Nobody is left, so " + GetLabel() + " logs off the server</b>"
    );
    destroy = true;
  }
}
