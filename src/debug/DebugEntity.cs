// DebugEntity.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public abstract partial class Entity : Debug.Labeled, IEntity {

    protected override void AssignVerboseDebugLabel(ref string label) {
      label = GetDebugLabel() + "\n";
      label += "STATUS\n";

      label += Debug.Utility.CollectionToString(
        "Attributes", GetAttributes(), (a) => {
          return a.GetVerboseDebugLabel();
        }
      );

      label += Debug.Utility.CollectionToString(
        "Interactions", Interactions, i => i.GetDebugLabel()
      );
    }
  }
}
