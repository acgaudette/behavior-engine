// UniverseComponent.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class UniverseComponent : MonoBehaviour {

  [HideInInspector] public ComponentManager manager;
  public Universe reference;
  public List<EntityComponent> entities = new List<EntityComponent>();

  ICollection<IEntity> lastEntities;
  int lastCount = 0;

  protected virtual void Update() {
    if (reference == null) return;

    ReplaceEntities();
    PollAll();
  }

  protected virtual void PollAll() {
    foreach (IEntity target in lastEntities) target.Poll();

    // Remove Entities marked as destroyed from the Universe
    reference.entities.RemoveWhere(
      e => e is IDestroyable && (e as IDestroyable).Destroy
    );
  }

  protected virtual void ReplaceEntities() {
    ICollection<IEntity> current = reference.entities;

    if (lastEntities != current || lastCount != current.Count) {
      manager.GenerateEntities(this, current);
      lastEntities = current;
      lastCount = current.Count;
    }
  }
}
