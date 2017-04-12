// Effect.cs
// Created by Aaron C Gaudette on 09.04.17

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
}
