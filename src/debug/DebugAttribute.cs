// DebugAttribute.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial class Attribute<T> : Debug.Labeled, IAttribute {

    public partial class Instance : Debug.Labeled, IAttributeInstance {

      protected override void AssignDebugLabel(ref string label) {
        label = Prototype.GetDebugLabel();
      }

      protected override void AssignVerboseDebugLabel(ref string label) {
        label = Prototype.GetDebugLabel() + " (" + State + ")";
      }
    }
  }
}
