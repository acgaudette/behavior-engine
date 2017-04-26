// Labeled.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Diagnostics;

namespace BehaviorEngine.Debug {

  public abstract class Labeled {

#if BEHAVIORENGINE_DEBUG

    string label = "unlabeled";

    public string GetLabel() {
      string label = "unassigned";
      AssignLabel(ref label);
      return label;
    }

    public string GetVerboseLabel() {
      string label = "unassigned";
      AssignVerboseLabel(ref label);
      return label;
    }

#endif

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public virtual void AssignLabel(ref string label) {

#if BEHAVIORENGINE_DEBUG

      label = this.label;

#endif

    }

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public virtual void AssignVerboseLabel(ref string label) {
      AssignLabel(ref label);
    }

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public void SetLabel(string label) {

#if BEHAVIORENGINE_DEBUG

      this.label = label;

#endif

    }
  }
}
