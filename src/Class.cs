// Class.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  // Container for behavior engine type prototypes
  public class Class {

    public static Class root = null; // Static access to root class (assign manually)

    // Reference prototypes
    public HashSet<IAttribute> attributes;
    public HashSet<Effect> effects;
    public HashSet<Interaction> interactions;

    public Class() {
      attributes = new HashSet<IAttribute>();
      effects = new HashSet<Effect>();
      interactions = new HashSet<Interaction>();
    }
  }
}
