// Attribute.cs
// Created by Aaron C Gaudette on 09.04.17

namespace BehaviorEngine {

  public partial interface IAttribute {
    IAttributeInstance NewInstance();
  }

  public partial interface IAttributeInstance {
    IAttribute Prototype { get; }
  }

  // Attribute prototype
  public partial class Attribute<T> : Debug.Labeled, IAttribute {

    public delegate T Initializer();
    Initializer defaultInitializer;

    // Construct with default initializer
    public Attribute(Initializer defaultInitializer) {
      this.defaultInitializer = defaultInitializer;
    }

    // Get instance from prototype
    public virtual IAttributeInstance NewInstance(
      Initializer initializeState
    ) {
      return new Attribute<T>.Instance(this, initializeState);
    }

    public IAttributeInstance NewInstance() {
      return NewInstance(defaultInitializer);
    }

    // The prototype determines how instance state is transformed
    protected virtual T TransformState(T raw) {
      return raw;
    }

    // Attribute instance
    public partial class Instance : Debug.Labeled, IAttributeInstance {
      T state;
      Attribute<T> prototype;

      internal Instance(Attribute<T> prototype, Initializer initializeState) {
        this.prototype = prototype;
        State = initializeState();
      }

      // Prototype reference
      public IAttribute Prototype { get { return prototype as IAttribute; } }

      // Only instances have state
      public T State {
        get { return state; }
        set { state = prototype.TransformState(value); }
      }
    }
  }
}
