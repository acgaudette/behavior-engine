// UniverseComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

[RequireComponent(typeof(Trigger))]
public class UniverseComponent : MonoBehaviour {

  [HideInInspector] public ComponentManager manager;
  public Universe reference;
  public ulong tick = 0;

  public List<EntityComponent> entities = new List<EntityComponent>();

  Trigger trigger;
  ICollection<IEntity> lastEntities;
  int lastCount = 0;

  protected virtual void Start() {
    trigger = GetComponent<Trigger>();
    trigger.Initialize();
  }

  protected virtual void Update() {
    if (reference == null) return;

    if (trigger.Ready) {
      ReplaceEntities();
      PollAll();
    }
  }

  protected virtual void PollAll() {
    foreach (IEntity target in lastEntities) target.Poll();

    tick++;
  }

  protected virtual void ReplaceEntities() {
    // Remove Entities marked as destroyed from the Universe
    reference.entities.RemoveWhere(
      e => e is IDestroyable && (e as IDestroyable).Destroy
    );

    ICollection<IEntity> current = reference.entities;

    if (lastEntities != current || lastCount != current.Count) {
      manager.GenerateEntities(this, current);
      lastEntities = current;
      lastCount = current.Count;
    }
  }
}
