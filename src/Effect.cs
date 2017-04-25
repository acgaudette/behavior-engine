// Effect.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  public partial class Effect : Root {

    public partial interface IModifier {
      IAttribute Attribute { get; }
      bool Apply(Entity e);
    }

    public abstract class Modifier<T> : Root, IModifier {
      public T offset;
      Attribute<T> attribute;

      public Modifier(Attribute<T> attribute, T offset) {
        this.attribute = attribute;
        this.offset = offset;
      }

      public IAttribute Attribute { get { return attribute; } }

      public bool Apply(Entity e) {
        Attribute<T>.Instance instance = e.GetAttribute(attribute)
          as Attribute<T>.Instance;

        // Target Entity does not have the Attribute being affected
        if (instance == null) return false;

        Modify(instance);
        return true;
      }

      protected abstract void Modify(Attribute<T>.Instance instance);
    }

    public List<IModifier> modifiers;

    public Effect() {
      modifiers = new List<IModifier>();
    }

    public bool Trigger(Entity target) {
      if (modifiers.Count == 0)
        return false;

      bool effective = true;

      foreach (IModifier m in modifiers)
        effective &= m.Apply(target);

      OnTrigger(target, effective);

      return effective;
    }

    // Event
    protected virtual void OnTrigger(Entity target, bool effective) { }
  }
}
