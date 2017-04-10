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

    public ReadOnlyCollection<Entity> GetAllEntities() {
      return new ReadOnlyCollection<Entity>(entities);
    }

    public ReadOnlyCollection<Entity> GetEntities(Entity target) {
      if (entities.Contains(target))
        return new ReadOnlyCollection<Entity>(entities);

      return null;
    }

    public bool AddEntity(Entity target) {
      if (entities.Contains(target)) return false;

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
