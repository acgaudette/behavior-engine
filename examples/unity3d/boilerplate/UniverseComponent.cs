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

      // Remove entities marked as destroyed from the universe
      for (int i = 0; i < current.Count; ++i) {
        if ((current[i] as UnityEntity).destroy) {
          current[i].GetUniverse().RemoveEntity(current[i]);
          i--;
        }
      }

      if (reference == Universe.root) {
        manager.lastPoll = Time.time;
        manager.IncrementTick();
      }
    }
  }
}
