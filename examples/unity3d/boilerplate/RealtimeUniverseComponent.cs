// RealtimeUniverseComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class RealtimeUniverseComponent : UniverseComponent {

  public float pollRate;
  float lastPoll = 0;

  void Start() {
    lastPoll = -pollRate; // Start immediately 
  }

  protected override void Update() {
    if (reference == null) return;

    // Display
    if (Time.time - lastPoll > pollRate) {
      ReplaceEntities();
      PollAll();

      lastPoll = Time.time;
    }
  }
}
