// Attribute.cs
// Created by Aaron C Gaudette on 09.04.17

namespace BehaviorEngine {

  public partial interface IAttribute {
    IAttributeInstance GetNewInstance();
  }

  public partial interface IAttributeInstance {
    IAttribute Prototype { get; }
  }

  // Attribute prototype
  public partial class Attribute<T> : Root, IAttribute {

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

    // Attribute instance
    public partial class Instance : Root, IAttributeInstance {
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
}
