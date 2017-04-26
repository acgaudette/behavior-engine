// DebugEffect.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial class Effect : Debug.Labeled {

    public partial interface IModifier {

#if BEHAVIORENGINE_DEBUG

      string GetLabel();
      string GetVerboseLabel();

#endif

    }

    public override void AssignVerboseLabel(ref string label) {

#if BEHAVIORENGINE_DEBUG

      label = GetLabel() + " (";

      foreach (IModifier modifier in modifiers) {
        label += " " + (
          modifier == null ? "null" : modifier.GetVerboseLabel()
        );
      }

      label += " )";

#endif

    }
  }
}
