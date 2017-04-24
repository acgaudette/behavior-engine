// ConsoleActionReader.cs
// Created by Aaron C Gaudette on 24.04.17

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine.Personality;

public class ConsoleActionReader {

  public static bool LoadFile(
    string path, CharacterUnit<ConsoleAction> unit
  ) {
    if (unit == null)
      return false;

    int count = 0;
    try {
      using (StreamReader reader = File.OpenText(@path)) {
        List<string> buffer = new List<string>();

        for (string line = null;;) {
          line = reader.ReadLine();

          if (line == null || line[0] != '\t') {
            if (buffer.Count > 0) {
              unit.actions.Add(new ConsoleAction(buffer.ToArray()));
              count++;
            }

            if (line == null) break;

            buffer = new List<string>() { line };
          } else {
            buffer.Add(line.Substring(1));
          }
        }
      }
    } catch (Exception e) {
      Debug.LogError(e);
      return false;
    }

    Debug.Log("ConsoleActionReader: loaded " + count + " actions");
    return true;
  }
}
