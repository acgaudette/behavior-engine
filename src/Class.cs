// Class.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BehaviorEngine {

  // Container for behavior engine type prototypes
  public class Class {

    public static Class root = null; // Static access to root class (assign manually)

    // Reference lists
    public List<IAttribute> attributes;
    public List<Effect> effects;
    public List<Interaction> interactions;

    public Class() {
      attributes = new List<IAttribute>();
      effects = new List<Effect>();
      interactions = new List<Interaction>();
    }
  }
}
