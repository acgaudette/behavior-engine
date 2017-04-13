// UniverseComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class UniverseComponent : MonoBehaviour {

  public Universe reference;
  public List<EntityComponent> entities = new List<EntityComponent>();
  [HideInInspector] public ComponentManager manager;

  ICollection<Entity> lastEntities;
  int lastCount = 0;

  void Update() {
    if (reference == null) {
      Debug.LogWarning("UniverseComponent: Reference is null!");
      return;
    }

    // Display
    if (Time.time - manager.lastPoll > manager.pollRate) {
      if (reference == Universe.root) {
        //manager.ClearConsole();
      }

      ICollection<Entity> current = reference.entities;

      if (lastEntities != current || lastCount != current.Count) {
        lastEntities = manager.GenerateEntities(entities, current);
        lastCount = current.Count;
      }

      // Poll
      foreach (Entity target in current) {
        target.Poll();
      }

      // Remove entities marked as destroyed from the universe
      reference.entities.RemoveWhere((e) => (e as UnityEntity).destroy);

      if (reference == Universe.root) {
        manager.lastPoll = Time.time;
        manager.IncrementTick();
      }
    }
  }
}
