// CrewmemberRenderer.cs
// Created by Aaron C Gaudette on 09.05.17

using System.Collections.Generic;
using BehaviorEngine;
using BehaviorEngine.Personality;

public partial class Crewmember : Character, IDestroyable {

  EntityEvents.OnReactEventHandler RenderReaction = (
    object sender,
    Interaction interaction, IEntity host, IList<Effect> effects
  ) => {
    Render.LogBiometrics("");
  };

  EntityEvents.OnObserveEventHandler RenderObservation = (
    object sender,
    Interaction interaction, IEntity host,
    ICollection<IEntity> targets, IList<Effect> effects
  ) => {
    Render.LogBiometrics("");
  };

  void HookRenderer() {
    OnReact += RenderReaction;
    OnObserve += RenderObservation;
  }
}
