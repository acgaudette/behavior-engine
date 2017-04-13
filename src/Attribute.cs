// Attribute.cs
// Created by Aaron C Gaudette on 09.04.17

using System;

namespace BehaviorEngine {

  public abstract class Attribute {

    public delegate float InitializeState();
    public readonly InitializeState initializeState;
    Class family;

    public bool Instance { get { return instance; } }
    bool instance;

    public ulong ID { get { return id; } }
    ulong id;

    // Creates a new Attribute archetype
    protected Attribute(Class family, InitializeState initializeState) {
      this.family = family;
      this.initializeState = initializeState;
      instance = false;
      id = family.RegisterAttribute(this);
    }

    // Creates a new Attribute instance
    protected Attribute(Attribute attribute) {
      id = attribute.id;
      instance = true;
      family = attribute.family;
      initializeState = attribute.initializeState;
      State = attribute.GetArchetype().initializeState();
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
    public NormalizedAttribute(Class family, InitializeState initializeState)
      : base(family, initializeState) { }

    // Instance
    protected NormalizedAttribute(Attribute attribute) : base(attribute) { }

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
