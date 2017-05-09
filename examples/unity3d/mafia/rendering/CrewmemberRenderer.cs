// CrewmemberRenderer.cs
// Created by Aaron C Gaudette on 09.05.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;
using BehaviorEngine.Personality;

public partial class Crewmember : Character, IDestroyable {

  const int BIOMETRICS_ERROR = 5;

  EntityEvents.OnReactEventHandler RenderReaction = (
    object sender,
    Interaction interaction, IEntity host, IList<Effect> effects
  ) => {
    // Don't always render
    if (Random.Range(0, 1f) > .5f) return;

    string data = "";

    foreach (Effect effect in effects) {
      foreach (IModifier mod in effect.Modifiers) {
        // Filter States
        State state = mod.Attribute as State;
        if (state == null) continue;

        // Get instance
        var instance = (sender as IEntity)[state] as State.TransformedInstance;
        if (instance == null) continue;

        float percent = 100;
        float offset = (mod as FloatModifier).offset;
        float current = instance.TransformedState;

        if (Mathf.Abs(offset) < instance.TransformedState)
          percent *= offset / current;

        string indicator = current < .2f ?
          "LOW" : current > .8f ? "HIGH" : "NORMAL";

        // Randomly fail
        if (Random.Range(0, 1f) > .75f)
          indicator = "UNCERTAIN";
        string percentString = percent.ToString("F2") + "%";
        if (Random.Range(0, 1f) > .85f)
          percentString = "??.??%";

        // Error
        percent += Random.Range(-BIOMETRICS_ERROR, BIOMETRICS_ERROR);

        data += "\n_" + state.name + " ";
        data += (percent >= 0 ? "+" : "")
          + percentString + " +/- " + BIOMETRICS_ERROR + "%"
          + " STATUS=" + indicator;
      }
    }

    Render.LogBiometrics(sender as Crewmember, data);
  };

  EntityEvents.OnObserveEventHandler RenderObservation = (
    object sender,
    Interaction interaction, IEntity host,
    ICollection<IEntity> targets, IList<Effect> effects
  ) => {
    //Render.LogBiometrics(sender as Crewmember, "");
  };

  void HookRenderer() {
    OnReact += RenderReaction;
    OnObserve += RenderObservation;
  }
}
