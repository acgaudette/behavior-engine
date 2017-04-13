// Universe.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  public class Universe {

    public static Universe root = null; // Static access to root universe (assign manually)

    public HashSet<Entity> entities;

    public Universe() {
      entities = new HashSet<Entity>();
    }
  }
}
