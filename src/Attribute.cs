// Attribute.cs
// Created by Aaron C Gaudette on 09.04.17

using System;

namespace BehaviorEngine {

  public abstract class Attribute {

    public readonly float initialState;
    Class family;

    public bool Instance { get { return instance; } }
    bool instance;

    public ulong ID { get { return id; } }
    ulong id;

    // Creates a new Attribute archetype
    protected Attribute(Class family, float initialState = 0) {
      this.family = family;
      this.initialState = initialState;
      instance = false;
      id = family.RegisterAttribute(this);
    }

    // Creates a new Attribute instance
    protected Attribute(Attribute attribute) {
      id = attribute.id;
      instance = true;
      family = attribute.family;
      State = initialState = attribute.GetArchetype().initialState;
    }

    public Attribute GetArchetype() {
      return family[ID];
    }

    public abstract Attribute GetNewInstance();

    public abstract float State { get; protected set; }

    public virtual void Modify(float offset) {
      State += offset;
    }
  }

  public class NormalizedAttribute : Attribute {

    // Archetype
    public NormalizedAttribute(Class family, float initialState)
      : base(family, initialState) { }

    // Instance
    NormalizedAttribute(Attribute attribute) : base(attribute) { }

    public override Attribute GetNewInstance() {
      return new NormalizedAttribute(this);
    }

    public override float State {
      get { return state; }
      // Clamp between 0 and 1
      protected set { state = Math.Min(1, Math.Max(0, value)); }
    }
    float state;
  }
}
