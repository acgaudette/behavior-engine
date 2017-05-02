// RateTrigger.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;

public class RateTrigger : Trigger {

  public float pollRate = 1;
  float lastPoll = 0;

  public override void Initialize() {
    lastPoll = -pollRate;
  }

  public override bool Ready {
    get {
      if (Time.time - lastPoll > pollRate) {
        lastPoll = Time.time;
        return true;
      }

      return false;
    }
  }
}
