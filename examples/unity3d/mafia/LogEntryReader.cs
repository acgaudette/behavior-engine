// LogEntryReader.cs
// Created by Aaron C Gaudette on 24.04.17

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine.Personality;

public class LogEntryReader {

  public static bool LoadFile(string path, BrainRepository repo) {
    if (repo == null)
      return false;

    int count = 0;

    if (!File.Exists(@path)) {
      Debug.LogError(
        "LogEntryReader: File does not exist at "
          + new FileInfo(@path).FullName
      );
      return false;
    }

    try {
      using (StreamReader reader = File.OpenText(@path)) {
        List<LogEntry.Phrase> buffer = new List<LogEntry.Phrase>();

        for (string key = null, line = null;;) {
          line = reader.ReadLine();

          if (line == null || line[0] != '\t') {
            // Catch first, single-line Action
            if (key == null && buffer.Count == 0) {
              key = line;
            // Error
            } else if (key == null) {
              Debug.LogError(
                "LogEntryReader: Parse error: tabbed data with no header"
              );
              return false;
            }

            // Add Action
            repo.RegisterAction(
              new LogEntry(
                key,
                buffer.Count == 0 ?
                  new LogEntry.Phrase[] { new LogEntry.Phrase(key) }
                  : buffer.ToArray()
              )
            );
            count++;

            // Exit loop
            if (line == null)
              break;

            key = line;
            buffer = new List<LogEntry.Phrase>();
          }

          // Add tabbed data
          else buffer.Add(new LogEntry.Phrase(line.Substring(1)));
        }
      }
    } catch (Exception e) {
      Debug.LogError("LogEntryReader: " + e);
      return false;
    }

    Debug.Log("LogEntryReader: loaded " + count + " actions");
    return true;
  }
}
