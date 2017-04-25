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

        for (string key = null, line = null;;) {
          line = reader.ReadLine();

          if (line == null || line[0] != '\t') {
            // Catch first, single-line Action
            if (key == null && buffer.Count == 0) {
              key = line;
            // Error
            } else if (key == null) {
              Debug.LogError(
                "ConsoleActionReader: Parse error: tabbed data with no header"
              );
              return false;
            }

            // Add Action
            unit.actions[key] = new ConsoleAction(
              buffer.Count == 0 ? new string[] { key } : buffer.ToArray()
            );
            count++;

            // Exit loop
            if (line == null)
              break;

            key = line;
            buffer = new List<string>();
          }

          // Add tabbed data
          else buffer.Add(line.Substring(1));
        }
      }
    } catch (Exception e) {
      Debug.LogError("ConsoleActionReader: " + e);
      return false;
    }

    Debug.Log("ConsoleActionReader: loaded " + count + " actions");
    return true;
  }
}
