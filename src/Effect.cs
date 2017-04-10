// Effect.cs
// Created by Aaron C Gaudette on 09.04.17

using System;
using System.Collections.Generic;

namespace BehaviorEngine {

  public class Effect {
    public class Modifier {
      public Attribute attribute = null;
      public float offset;

      public Modifier(Attribute attribute, float offset) {
        SetAttribute(attribute);
        this.offset = offset;
      }

      public bool SetAttribute(Attribute attribute) {
        if (attribute.Instance) return false;
        this.attribute = attribute;
        return true;
      }

      public bool Apply(Entity e) {
        if (attribute == null) return false;

        Attribute target = e.GetAttribute(attribute);

        // Target entity does not have the attribute being affected
        if (target == null) return false;

        target.Modify(offset);
        return true;
      }
    }

    public List<Modifier> modifiers;

    public Effect() {
      modifiers = new List<Modifier>();
    }

    public bool Trigger(Entity target) {
      foreach (Modifier m in modifiers)
        m.Apply(target);

      return true;
    }
  }

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
