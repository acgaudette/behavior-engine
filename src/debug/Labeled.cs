// Labeled.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Diagnostics;

namespace BehaviorEngine.Debug {

  public interface ILabeled {
    string GetDebugLabel();
    string GetVerboseDebugLabel();
  }

  public abstract class Labeled : ILabeled {

#if BVE_DEBUG

    string label = "unlabeled";

#endif

    public string GetDebugLabel() {
      string label = "unassigned";
      AssignDebugLabel(ref label);
      return label;
    }

    public string GetVerboseDebugLabel() {
      string label = "unassigned";
      AssignVerboseDebugLabel(ref label);
      return label;
    }

    [Conditional("BVE_DEBUG")]
    protected virtual void AssignDebugLabel(ref string label) {

#if BVE_DEBUG

      label = this.label;

#endif

    }

    [Conditional("BVE_DEBUG")]
    protected virtual void AssignVerboseDebugLabel(ref string label) {
      AssignDebugLabel(ref label);
    }

    [Conditional("BVE_DEBUG")]
    public void SetDebugLabel(string label) {

#if BVE_DEBUG

      this.label = label;

#endif

    }
  }
}
