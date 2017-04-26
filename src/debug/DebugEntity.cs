// DebugEntity.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial interface IEntity {

#if BEHAVIORENGINE_DEBUG

    string GetLabel();
    string GetVerboseLabel();

#endif

  }

  public abstract partial class Entity : Debug.Labeled, IEntity {

    public override void AssignVerboseLabel(ref string label) {

#if BEHAVIORENGINE_DEBUG

      label = GetLabel() + "\n";
      label += "STATUS\n";

      label += Debug.Utility.CollectionToString(
        "Attributes", GetAttributes(), (a) => {
          return a.GetVerboseLabel();
        }
      );

      label += Debug.Utility.CollectionToString(
        "Interactions", Interactions, i => i.GetLabel()
      );

#endif

    }

  }
}
