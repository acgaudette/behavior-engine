// Render.cs
// Created by Aaron C Gaudette on 10.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public partial class User : UnityEntity {

  protected override void OnReact(Interaction interaction, Entity host, List<Effect> effects) {
    if (effects == null) return;

    string suffix = " by " + (host as ILabeled).Label + "'s flame message";

    if (effects.Contains(Forum.annoy))
      Debug.Log(Label + " is frustrated" + suffix);

    else if (effects.Contains(Forum.incite))
      Debug.Log("<color=#fc4e4e>" + Label + " is enraged" + suffix + "</color>");
  }

  protected override void OnObserve(
    Interaction interaction, Entity host, List<Entity> targets, List<Effect> effects
  ) {
    if (effects == null) return;

    string suffix = interaction == Forum.start ?
      " after reading " + (host as ILabeled).Label + "'s post"
      : " after " + (host as ILabeled).Label + "'s massive rage";

    if (effects.Contains(Forum.calm) && GetAttribute(Forum.anger).GetState() > 0) {
      if (interaction == Forum.start)
        Debug.Log("<color=cyan>" + Label + " calms down" + suffix + "</color>");
      else
        Debug.Log("<color=cyan>" + Label + " feels pretty great" + suffix + "</color>");
    }

    else if (effects.Contains(Forum.annoy))
      Debug.Log(Label + " is annoyed" + suffix);

    else if (effects.Contains(Forum.incite))
      Debug.Log("<color=#fc4e4e>" + Label + " is totally triggered" + suffix + "</color>");
  }

  protected override void OnPoll(
    Interaction choice, ReadOnlyCollection<Entity> targets, float highscore
  ) {
    if (choice == Forum.quit)
      Ragequit();
    else if (GetUniverse().GetEntities(this).Count == 1)
      Logoff();

    else if (choice == Forum.start) {
      float anger = GetAttribute(Forum.anger).GetState();
      Debug.Log(
        Label + " starts a new thread, " + ((anger > .66f) ?
          "super angry" : (anger > .33f) ?
          "pretty controversial" : "easy topic")
      );
    }

    else if (choice == Forum.flame) {
      Debug.Log(
        "<color=orange>" + Label + " sends an angry DM to "
        + (targets[0] as ILabeled).Label + "!</color>"
      );
    }
  }

  void Ragequit() {
    Debug.Log("<b><color=red>" + Label + " flipped out and ragequit!</color></b>");
    destroy = true;
  }

  void Logoff() {
    Debug.Log("<b>Nobody is left, so " + Label + " logs off the server</b>");
    destroy = true;
  }
}
