// Utility.cs
// Created by Aaron C Gaudette on 09.04.17

using System;
using System.Collections.Generic;
using BehaviorEngine;

public interface ILabeled {
  string Label { get; set; }
}

public class Utility {

  public static string CollectionToString<L>(
    string prefix, ICollection<L> collection, Func<L, string> result
  ) {
    prefix += " ";

    if (collection == null || collection.Count == 0)
      prefix += "= null\n";

    else {
      prefix += "(" + collection.Count + ") = ";
      foreach (L l in collection)
        prefix += "[" + result(l) + "] ";
      prefix += "\n";
    }

    return prefix;
  }
}
