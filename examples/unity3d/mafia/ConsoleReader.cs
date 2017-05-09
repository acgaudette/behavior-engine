// ConsoleReader.cs
// Created by Aaron C Gaudette on 24.04.17

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleReader {

  public const string TAB_STRING = "  ";

  public struct Node : IEnumerable<Node> {

    public const string NULL_DATA = "null";

    public readonly string data;
    public readonly Node[] children;

    public int Count {
      get {
        int count = 1;
        foreach (Node node in children)
          count += node.Count;
        return count;
      }
    }

    public Node(string data, Node[] children = null) {
      this.data = data;
      this.children = children == null ? new Node[0] : children;
    }

    public IEnumerator<Node> GetEnumerator() {
      return ((IEnumerable<Node>)children).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public override string ToString() {
      return ToStringIndented();
    }

    string ToStringIndented(string indent = null) {
      if (indent != null)
        indent += ConsoleReader.TAB_STRING;

      string result = indent + data + "\n";

      foreach (Node node in children)
        result += indent + node.ToStringIndented("" + indent); 

      return result;
    }
  }

  public static bool LoadFile(string path, out Node root) {
    if (!File.Exists(@path)) {
      Debug.LogError(
        "ConsoleReader: File does not exist at "
          + new FileInfo(@path).FullName
      );

      root = new Node(Node.NULL_DATA);
      return false;
    }

    var lines = new Stack<List<Node>>();
    lines.Push(new List<Node>());

    try {
      using (StreamReader reader = File.OpenText(@path)) {
        var data = new Stack<string>();

        string line, lastLine = "root";
        int indent = -1, lastIndent;

        do {
          line = reader.ReadLine();

          lastIndent = indent;
          indent = GetIndent(line);

          line = Clean(line);

          if (indent > lastIndent) {
            data.Push(lastLine);
            lines.Push(new List<Node>());
          }

          else if (indent < lastIndent) {
            lines.Peek().Add(new Node(lastLine));

            while (lastIndent - indent > 0) {
              Node[] nodes = lines.Pop().ToArray();
              lines.Peek().Add(new Node(data.Pop(), nodes));

              lastIndent--;
            }
          }

          else lines.Peek().Add(new Node(lastLine));

          lastLine = line;
        }

        while (lastLine != null);

        // Finalize
        while (lines.Count > 1) {
          Node[] nodes = lines.Pop().ToArray();
          lines.Peek().Add(new Node(data.Pop(), nodes));
        }
      }
    } catch (Exception e) {
      Debug.LogError("ConsoleReader: " + e);

      root = new Node(Node.NULL_DATA);
      return false;
    }

    if (lines.Count == 0) {
      Debug.LogWarning("ConsoleReader: Input file is empty");

      root = new Node(Node.NULL_DATA);
      return false;
    }

    root = lines.Pop()[0];
    Debug.Log("ConsoleReader: loaded " + root.Count + " nodes");

    return true;
  }

  static int GetIndent(string line) {
    if (line == null)
      return -1;

    if (line.Length < TAB_STRING.Length)
      return 0;

    int indent = 0;

    for (
      int i = 0;
      line.Substring(i, TAB_STRING.Length).Equals(TAB_STRING)
        && i + TAB_STRING.Length <= line.Length;
      i += TAB_STRING.Length
    ) indent++;

    return indent;
  }

  static string Clean(string line) {
    if (line == null) return null;

    if (line.Length < TAB_STRING.Length)
      return line;

    while (line.Substring(0, TAB_STRING.Length).Equals(TAB_STRING)) {
      line = line.Substring(
        TAB_STRING.Length, line.Length - TAB_STRING.Length
      );
    }

    return line;
  }
}
