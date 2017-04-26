// Debugger.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Diagnostics;

namespace BehaviorEngine.Debug {

  public static class Debugger {

    public delegate void Logger(string message);

#if BEHAVIORENGINE_DEBUG

    static Logger log = m => System.Console.WriteLine(m);

#endif

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void SetLogger(Logger log) {
      if (log == null) return;

#if BEHAVIORENGINE_DEBUG

      Debugger.log = log;

#endif

    }

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void Log(string message) {

#if BEHAVIORENGINE_DEBUG

      log(message);

#endif

    }
  }
}
