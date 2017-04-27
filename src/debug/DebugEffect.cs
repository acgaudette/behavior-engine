// DebugEffect.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial class Effect : Debug.Labeled {

    protected override void AssignVerboseDebugLabel(ref string label) {
      label = GetDebugLabel() + " (";

      foreach (IModifier modifier in Modifiers) {
        label += " " + (
          modifier == null ? "null" : modifier.GetVerboseDebugLabel()
        );
      }

      label += " )";
    }
  }
}
