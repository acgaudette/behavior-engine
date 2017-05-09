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

    // Output timestamp string
    public override string ToString() {
      return "[" + Pad(cycle) + ":" + Pad(tick) + ":" + Pad(offset) + "]";
    }

    // Pad with a single zero
    string Pad(int i) {
      return (i < 10 && i >= 0) ? "0" + i : i.ToString();
    }
  }

  static List<string> log = new List<string>(); // Log store

  // Static timestamp data
  static int cycle = 0, tick = 0, offset = 0;

  // Log timestamped observation/analysis block
  public static void Log(
    string observation, string analysis
  ) {
    UpdateTimestamp();

    LogEntry(
      GetTimestamp() + ENDL
      + "observation: " + observation + ENDL
      + "analysis: " + analysis + ENDL
    );
  }

  public static void LogBiometrics(string data) {
    LogEntry(
      GetTimestamp() + ENDL
      + data + ENDL
    );
  }

  // Log a single line
  public static void Print(string message) {
    LogEntry(message + ENDL);
  }

  // Output log to file
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

  // Increment static cycle variable
  public static void IncrementCycle() {
    cycle++;
    tick = 0;
    offset = 0;
  }

  // Increment static timestamp
  static Timestamp GetTimestamp() {
    return new Timestamp(cycle, tick, offset);
  }

  static void UpdateTimestamp() {
    int seconds = Random.Range(3, 15);

    if (offset + seconds > 60)
      tick++;

    offset += seconds;
    offset %= 60;
  }

  // Output/store log entry
  static void LogEntry(string entry) {
    Debug.Log(entry);
    log.Add(Clean(entry));
  }

  // Clean a string of tags
  static string Clean(string entry) {
    return Regex.Replace(entry, "<.*?>", "");
  }
}
