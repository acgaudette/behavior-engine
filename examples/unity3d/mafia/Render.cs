// Render.cs
// Created by Aaron C Gaudette on 02.05.17

using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Render {

  public const string ENDL = "\n";

  public struct Timestamp {

    public int cycle, tick, offset;

    public Timestamp(int cycle, int tick, int offset) {
      this.cycle = cycle;
      this.tick = tick;
      this.offset = offset;
    }

    public override string ToString() {
      return "[" + Pad(cycle) + ":" + Pad(tick) + ":" + Pad(offset) + "]";
    }

    string Pad(int i) {
      return i < 10 ? "0" + i : i.ToString();
    }
  }

  static List<string> log = new List<string>();

  static int cycle = 0, tick = 0, offset = 0;

  public static void Log(
    string observation, string analysis
  ) {
    string entry = GetTimestamp() + ENDL
      + "observation: " + observation + ENDL
      + "analysis: " + analysis + ENDL;

    Debug.Log(entry);
    log.Add(Clean(entry));
  }

  public static void Print(string message) {
    string entry = message + ENDL;

    Debug.Log(entry);
    log.Add(Clean(entry));
  }

  public static bool WriteToFile(string path) {
    try {
      File.WriteAllLines(@path, log.ToArray());
    } catch (System.Exception e) {
      Debug.LogError("Render: " + e);
      return false;
    }

    Debug.Log(
      "Render: Wrote " + log.Count + " lines to " + Path.GetFileName(path)
    );

    return true;
  }

  public static void IncrementCycle() {
    cycle++;
    tick = 0;
    offset = 0;
  }

  static Timestamp GetTimestamp() {
    int seconds = Random.Range(3, 15);

    if (offset + seconds > 60)
      tick++;

    offset += seconds;
    offset %= 60;

    return new Timestamp(cycle, tick, offset);
  }

  static string Clean(string entry) {
    return Regex.Replace(entry, "<.*?>", "");
  }
}
