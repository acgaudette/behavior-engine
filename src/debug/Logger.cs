// Debugger.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Diagnostics;

namespace BehaviorEngine.Debug {

  public static class Logger {

    public delegate void DoLog(string message);

#if BVE_DEBUG

    static DoLog log = m => System.Console.WriteLine(m);

#endif

    [Conditional("BVE_DEBUG")]
    public static void SetLogger(DoLog log) {
      if (log == null) return;

#if BVE_DEBUG

      Logger.log = log;

#endif

    }

    [Conditional("BVE_DEBUG")]
    public static void Log(object message) {

#if BVE_DEBUG

      log(message.ToString());

#endif

    }
  }
}
