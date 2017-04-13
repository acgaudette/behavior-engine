// Universe.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BehaviorEngine {

  public class Universe {

    public static Universe root = null; // Static access to root universe (assign manually)

    List<Entity> entities;

    public Universe() {
      entities = new List<Entity>();
    }

    public ReadOnlyCollection<Entity> GetEntities() {
      return new ReadOnlyCollection<Entity>(entities);
    }

    public bool ContainsEntity(Entity target) {
      return entities.Contains(target);
    }

    public bool AddEntity(Entity target) {
      if (entities.Contains(target)) return false; // No duplicates

      entities.Add(target);
      return true;
    }

    public bool RemoveEntity(Entity target) {
      if (!entities.Contains(target)) return false;

      entities.Remove(target);
      return true;
    }
  }
}
