// DebugEffect.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial class Effect : Root {

    public partial interface IModifier {
      string GetLabel();
      string GetVerboseLabel();
    }

    public override string GetVerboseLabel() {
      string debug = GetLabel() + " (";

      foreach (IModifier modifier in modifiers) {
        debug += " " + (
          modifier == null ? "null" : modifier.GetVerboseLabel()
        );
      }

      return debug + " )";
    }
  }
}
