// TriggerableUniverseComponent.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;

public class TriggerableUniverseComponent : UniverseComponent {

  public KeyCode key;
  [SerializeField] ulong tick = 0;

  protected override void Update() {
    if (reference == null) return;

    if (Input.GetKeyUp(key)) {
      ReplaceEntities();
      PollAll();
      tick++;
    }
  }
}
