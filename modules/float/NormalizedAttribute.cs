// NormalizedAttribute.cs
// Created by Aaron C Gaudette on 24.04.17

using System;

namespace BehaviorEngine.Float {

  public class NormalizedAttribute : Attribute<float> {

    public NormalizedAttribute(Initializer defaultInitializer)
      : base (defaultInitializer) { }

    public override IAttributeInstance NewInstance(
      Initializer initializeState
    ) {
      return new NormalizedAttribute.Instance(this, initializeState);
    }

    protected override float TransformState(float raw) {
      return Math.Min(1, Math.Max(0, raw));
    }
  }
}
