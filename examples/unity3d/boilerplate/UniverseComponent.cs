// UniverseComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public class UniverseComponent : MonoBehaviour {

  public Universe reference;
  public List<EntityComponent> entities = new List<EntityComponent>();
  [HideInInspector] public ComponentManager manager;

  ReadOnlyCollection<Entity> lastEntities;
  float lastTime = 0;

  void Update() {
    if (reference == null) {
      Debug.LogWarning("UniverseComponent: Reference is null!");
      return;
    }

    // Display
    if (Time.time - lastTime > manager.pollRate) {
      ReadOnlyCollection<Entity> current = reference.GetAllEntities();

      if (lastEntities == null || lastEntities.Count != current.Count)
        lastEntities = manager.GenerateEntities(entities, current);

      for (int i = 0; i < current.Count; ++i) {
        if (lastEntities[i] != current[i]) {
          lastEntities = manager.GenerateEntities(entities, current);
          break;
        }
      }

      // Poll
      foreach (Entity target in current) {
        target.Poll();
      }

      lastTime = Time.time;
    }
  }
}
