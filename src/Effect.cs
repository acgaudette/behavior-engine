// Effect.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  public interface IModifier : Debug.ILabeled {

    IAttribute Attribute { get; }
    bool Apply(IEntity e);
  }

  public partial class Effect : Debug.Labeled {

    public abstract class Modifier<T> : Debug.Labeled, IModifier {
      public T offset;
      Attribute<T> attribute;

      public Modifier(Attribute<T> attribute, T offset) {
        this.attribute = attribute;
        this.offset = offset;
      }

      public IAttribute Attribute { get { return attribute; } }

      public bool Apply(IEntity target) {
        Attribute<T>.Instance instance = target[attribute]
          as Attribute<T>.Instance;

        // Target Entity does not have the Attribute being affected
        if (instance == null) return false;

        Modify(instance);
        return true;
      }

      protected abstract void Modify(Attribute<T>.Instance instance);
    }

    public ICollection<IModifier> Modifiers { get; set; }

    public Effect() {
      Modifiers = new List<IModifier>();
    }

    public bool Trigger(IEntity target) {
      if (Modifiers.Count == 0)
        return false;

      bool effective = true;

      foreach (IModifier m in Modifiers)
        effective &= m.Apply(target);

      // Fire event
      EffectEvents.OnTriggerEventHandler handler = OnTrigger;
      if (handler != null)
        handler(this, target, effective);

      return effective;
    }

    // Event
    public event EffectEvents.OnTriggerEventHandler OnTrigger;
  }

  public static class EffectEvents {

    public delegate void OnTriggerEventHandler(
      object sender,
      IEntity target, bool effective
    );
  }
}
