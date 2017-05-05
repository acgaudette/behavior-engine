// Render.cs
// Created by Aaron C Gaudette on 02.05.17

using UnityEngine;

public class Render {

  public const string ENDL = "\n";

  public static void Log(string observation, string analysis) {
    string timestamp = "[00:00:00]"; // stub
    Debug.Log(
      timestamp + ENDL
      + "observation: " + observation + ENDL
      + "analysis: " + analysis + ENDL
    );
  }

  public static void Print(string message) {
    Debug.Log(message + ENDL);
  }
}
