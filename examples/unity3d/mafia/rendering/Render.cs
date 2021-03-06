// Render.cs
// Created by Aaron C Gaudette on 02.05.17

using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Render {

  public const string ENDL = "\n";

  const string STYLE_CSS =
    "<style>body{background-color:black}.log{"
    + "background-color:black;color:red;font-family:monospace;"
    + "padding-left:16px;padding-top:16px;"
    + "padding-right:16px;padding-bottom:16px"
    + "}</style>";

  const string STYLE_CSS_MOBILE =
    "<style>body{background-color:black}.log{"
    + "background-color:black;color:red;font-family:monospace;"
    + "padding-left:16px;padding-top:16px;"
    + "padding-right:16px;padding-bottom:16px;"
    + "font-size:60%"
    + "}</style>";

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

  public static void LogBiometrics(Crewmember source, string data) {
    LogEntry(
      GetTimestamp() + ENDL
      + "BIOMETRICS AI -- " + source.name.ToUpper()
      + ", " + source.firstName.ToUpper()
      + data + ENDL
    );
  }

  // Log a single line
  public static void Print(string message) {
    LogEntry(message + ENDL);
  }

  // Output log to file
  public static bool WriteToFile(
    string directory, string filename, bool html = true
  ) {
    return Output(directory + filename, false)
      && (!html || Output(directory + filename, true));
  }

  static bool Output(string path, bool html, bool mobile = false) {
    string rawPath = path;

    if (html) {
      path += (mobile ? "-mobile" : "") + ".html";

      for (int i = 0; i < log.Count; ++i) {
        log[i] = Regex.Replace(log[i], @"\n", "<br>") + "<br>";
      }

      log.Insert(
        0, "<html>" + (mobile ? STYLE_CSS_MOBILE : STYLE_CSS)
          + "<div class='log'>"
      );
      log.Add("</div></html>");
    } else {
      path += ".txt";
    }

    try {
      File.WriteAllLines(@path, log.ToArray());
    } catch (System.Exception e) {
      Debug.LogError("Render: " + e);
      return false;
    }

    Debug.Log(
      "Render: Wrote " + log.Count + " lines to " + Path.GetFileName(path)
    );

    if (html && !mobile) {
      return Output(rawPath, true, true);
    }

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
