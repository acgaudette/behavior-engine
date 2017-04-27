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
      return new NormalizedAttribute.Instance(this, initializeState);
    }

    protected override float TransformState(float raw) {
      return transform(Math.Min(1, Math.Max(0, raw)));
    }
  }
}
