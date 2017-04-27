// NormalizedAttribute.cs
// Created by Aaron C Gaudette on 24.04.17

using System;

namespace BehaviorEngine.Float {

  public class NormalizedAttribute : Attribute<float> {

    public delegate float Transform(float x);
    public Transform transform;

    public NormalizedAttribute(
      Initializer defaultInitializer, Transform transform = null
    ) : base (defaultInitializer) {
      this.transform = transform == null ?
        Transformations.Linear() : transform;
    }

    public override IAttributeInstance NewInstance(
      Initializer initializeState
    ) {
      return new NormalizedAttribute.TransformedInstance(this, initializeState);
    }

    protected override float TransformState(float raw) {
      return Math.Min(1, Math.Max(0, raw));
    }

    public class TransformedInstance : Instance {

      internal TransformedInstance(
        NormalizedAttribute prototype, Initializer initializeState
      ) : base(prototype, initializeState) { }

      public float Transformed {
        get { return (Prototype as NormalizedAttribute).transform(State); }
      }
    }
  }
}
