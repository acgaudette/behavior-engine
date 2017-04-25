// DebugAttribute.cs
// Created by Aaron C Gaudette on 24.04.17

namespace BehaviorEngine {

  public partial interface IAttribute {
    string GetLabel();
    string GetVerboseLabel();
  }

  public partial interface IAttributeInstance {
    string GetLabel();
    string GetVerboseLabel();
  }

  public partial class Attribute<T> : Root, IAttribute {

    public partial class Instance : Root, IAttributeInstance {

      public override string GetLabel() {
        return Prototype.GetLabel();
      }

      public override string GetVerboseLabel() {
        return Prototype.GetLabel() + " (" + State + ")";
      }
    }
  }
}
