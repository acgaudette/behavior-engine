// NormalizedAttribute.cs
// Created by Aaron C Gaudette on 24.04.17

using System;

namespace BehaviorEngine.Float {

  public class NormalizedAttribute : Attribute<float> {

    public NormalizedAttribute(InitializeState initializeState)
      : base(initializeState) { }

    public override IAttributeInstance GetNewInstance() {
      return new NormalizedAttribute.Instance(this);
    }

    protected override float TransformState(float raw) {
      return Math.Min(1, Math.Max(0, raw));
    }
  }
}
