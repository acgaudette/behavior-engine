// Attribute.cs
// Created by Aaron C Gaudette on 09.04.17

using System;

namespace BehaviorEngine {

  public class Attribute {

    public float initialState;
    public Class family;

    protected float state;

    public ulong ID { get { return id; } }
    ulong id;

    public bool Instance { get { return instance; } }
    bool instance;

    public Attribute(Class family, float initialState = 0) {
      state = this.initialState = initialState;
      id = family.RegisterAttribute(this);
      instance = false;
      this.family = family;
    }

    public Attribute(Attribute attribute) {
      state = this.initialState = attribute.initialState;
      id = attribute.id;
      instance = true;
      family = attribute.family;
    }

    public Attribute GetArchetype() {
      return family[ID];
    }

    public float GetState() { return state; }

    public virtual void Modify(float offset) {
      state += offset;
    }
  }

  public class NormalizedAttribute : Attribute {

    public NormalizedAttribute(Class family, float initialState)
      : base(family, initialState) { }

    public override void Modify(float offset) {
      state = Math.Min(1, Math.Max(0, state + offset));
    }
  }
}
