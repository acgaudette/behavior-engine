// UniverseComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class UniverseComponent : MonoBehaviour {

  public float pollRate;
  [SerializeField] ulong tick = 0;

  [HideInInspector] public ComponentManager manager;
  public Universe reference;
  public List<EntityComponent> entities = new List<EntityComponent>();

  float lastPoll = 0;
  ICollection<IEntity> lastEntities;
  int lastCount = 0;

  void Start() {
    lastPoll = -pollRate; // Start immediately 
  }

  void Update() {
    if (reference == null) return;

    // Display
    if (Time.time - lastPoll > pollRate) {
      //if (reference == Universe.root) manager.ClearConsole();

      ICollection<IEntity> current = reference.entities;

      if (lastEntities != current || lastCount != current.Count) {
        manager.GenerateEntities(this, current);
        lastEntities = current;
        lastCount = current.Count;
      }

      // Poll
      foreach (IEntity target in current) {
        target.Poll();
      }

      // Remove Entities marked as destroyed from the Universe
      reference.entities.RemoveWhere(
        e => e is IDestroyable && (e as IDestroyable).Destroy
      );

      lastPoll = Time.time;
      tick++;
    }
  }
}
