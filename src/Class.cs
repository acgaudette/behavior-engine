// Class.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BehaviorEngine {

  // Container for behavior engine type references
  // e.g. you can't have an interaction "instance," so to speak
  public class Class {

    public static Class root = null; // Static access to root class (assign manually)

    // Reference lists
    public List<Effect> effects;
    public List<Interaction> interactions;

    ulong idptr, offset;
    Dictionary<ulong, Attribute> attributes;

    public Class() {
      effects = new List<Effect>();
      interactions = new List<Interaction>();

      idptr = offset = 0;
      attributes = new Dictionary<ulong, Attribute>();
    }

    public Attribute this[ulong i] {
      get { return attributes.ContainsKey(i) ? attributes[i] : null; }
    }

    public ulong AttributeCount { get { return idptr - offset; } }

    public bool Registered(Attribute attribute) {
      return attribute != null && attributes.ContainsKey(attribute.ID);
    }

    public ICollection<Attribute> GetAttributes() {
      return attributes.Values;
    }

    internal ulong RegisterAttribute(Attribute attribute) {
      attributes.Add(idptr, attribute);
      return idptr++;
    }

    internal bool DeregisterAttribute(Attribute attribute) {
      if (attributes.Remove(attribute.ID)) {
        offset++;
        return true;
      }

      return false; // Not registered
    }
  }
}
