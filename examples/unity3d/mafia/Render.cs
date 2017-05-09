// Render.cs
// Created by Aaron C Gaudette on 02.05.17

using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Render {

  public const string ENDL = "\n";

  static List<string> log = new List<string>();

  public static void Log(string observation, string analysis) {
    string timestamp = "[00:00:00]"; // stub

    string entry = timestamp + ENDL
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
    } catch (Exception e) {
      Debug.LogError("Render: " + e);
      return false;
    }

    Debug.Log("Render: Wrote " + log.Count + " lines to file");

    return true;
  }

  static string Clean(string entry) {
    return Regex.Replace(entry, "<.*?>", "");
  }
}
