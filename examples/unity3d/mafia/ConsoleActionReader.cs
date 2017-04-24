// ConsoleActionReader.cs
// Created by Aaron C Gaudette on 24.04.17

using System;
using System.IO;
using UnityEngine;
using BehaviorEngine.Personality;

public class ConsoleActionReader {

  public static bool LoadFile(
    string path, CharacterUnit<ConsoleAction> unit
  ) {
    if (unit == null)
      return false;

    string[] lines;
    try {
      lines = File.ReadAllLines(@path);
    } catch (Exception e) {
      Debug.LogError(e);
      return false;
    }

    foreach (string line in lines) {
      unit.actions.Add(new ConsoleAction(new string[]{ line }));
    }

    Debug.Log("ConsoleActionReader: loaded " + lines.Length + " actions");
    return true;
  }
}
