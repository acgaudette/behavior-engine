// Attribute.cs
// Created by Aaron C Gaudette on 09.04.17

using System;

namespace BehaviorEngine {

  public interface IAttribute {
    IAttributeInstance GetNewInstance();
  }

  public interface IAttributeInstance {
    IAttribute Prototype { get; }
  };

  public class Attribute<T> : IAttribute {

    public delegate T InitializeState();
    InitializeState initializeState;

    public InitializeState Initializer {
      get { return initializeState; }
    }

    protected Attribute(InitializeState initializeState) {
      this.initializeState = initializeState;
    }

    public virtual IAttributeInstance GetNewInstance() {
      return new Attribute<T>.Instance(this);
    }

    protected virtual T TransformState(T raw) {
      return raw;
    }

    public class Instance : IAttributeInstance {
      T state;

      Attribute<T> prototype;

      internal Instance(Attribute<T> prototype) {
        this.prototype = prototype;
        State = prototype.Initializer();
      }

      public IAttribute Prototype { get { return prototype as IAttribute; } }

      public T State {
        get { return state; }
        set { state = prototype.TransformState(value); }
      }
    }
  }

  public class NormalizedAttribute : Attribute<float> {

    protected NormalizedAttribute(InitializeState initializeState)
      : base(initializeState) { }

    protected override float TransformState(float raw) {
      return Math.Min(1, Math.Max(0, raw));
    }
  }
}
