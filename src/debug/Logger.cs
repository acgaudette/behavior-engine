// Debugger.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Diagnostics;

namespace BehaviorEngine.Debug {

  public static class Logger {

    public delegate void DoLog(string message);

#if BEHAVIORENGINE_DEBUG

    static DoLog log = m => System.Console.WriteLine(m);

#endif

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void SetLogger(DoLog log) {
      if (log == null) return;

#if BEHAVIORENGINE_DEBUG

      Logger.log = log;

#endif

    }

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void Log(object message) {

#if BEHAVIORENGINE_DEBUG

      log(message.ToString());

#endif

    }
  }
}
