// Root.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Diagnostics;

namespace BehaviorEngine {

public abstract class Root {

  public delegate void Logger(string message);
  public static Logger logger = m => System.Console.WriteLine(m);

#if LABELED

    string label = "";

#endif

    public virtual string GetLabel() {

#if LABELED

      return label;

#else

      return "?";

#endif

    }

    public virtual string GetVerboseLabel() {
      return GetLabel();
    }

    [Conditional("LABELED")]
    public void SetLabel(string value) {

#if LABELED

      label = value;

#endif

    }

    [Conditional("LABELED")]
    public virtual void Log() {
      Log(GetLabel());
    }

    [Conditional("LABELED")]
    public static void Log(string message) {
      if (logger == null) return;
      logger(message);
    }
  }
}
