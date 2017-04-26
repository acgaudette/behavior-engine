// FloatModifier.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine.Float {

  public class FloatModifier : Effect.Modifier<float> {

    public FloatModifier(Attribute<float> attribute, float offset)
      : base(attribute, offset) { }

    protected override void Modify(Attribute<float>.Instance instance) {
      instance.State += offset;
    }

    // Debug
    protected override void AssignVerboseDebugLabel(ref string label) {
      label = (offset > 0 ? "+" : "") + offset + " "
        + (Attribute == null ? "null" : Attribute.GetDebugLabel());
    }
  }
}
