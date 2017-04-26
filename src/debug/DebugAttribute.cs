// DebugAttribute.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial interface IAttribute {

#if BEHAVIORENGINE_DEBUG

    string GetLabel();
    string GetVerboseLabel();

#endif

  }

  public partial interface IAttributeInstance {

#if BEHAVIORENGINE_DEBUG

    string GetLabel();
    string GetVerboseLabel();

#endif

  }

  public partial class Attribute<T> : Debug.Labeled, IAttribute {

    public partial class Instance : Debug.Labeled, IAttributeInstance {

      public override void AssignLabel(ref string label) {

#if BEHAVIORENGINE_DEBUG

        label = Prototype.GetLabel();

#endif

      }

      public override void AssignVerboseLabel(ref string label) {

#if BEHAVIORENGINE_DEBUG

        label = Prototype.GetLabel() + " (" + State + ")";

#endif

      }
    }
  }
}
