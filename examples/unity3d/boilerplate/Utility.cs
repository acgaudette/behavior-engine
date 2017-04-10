// Utility.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using BehaviorEngine;

public class Utility {
  public static string EntityLabelsToString(
    ReadOnlyCollection<Entity> entities, string prefix = "Entities"
  ) {
    string s = prefix + " ";

    if (entities == null || entities.Count == 0) s += "= null\n";
    else {
      s += "(" + entities.Count + ") = ";
      foreach (Entity entity in entities)
        s += ((UnityEntity)entity).Label + " ";
      s += "\n";
    }

    return s;
  }

  public static string EffectsToString(List<Effect> effects) {
    string s = "Effects ";

    if (effects == null || effects.Count == 0) s += "= null\n";
    else {
      s += "(" + effects.Count + ") = ";
      foreach (Effect e in effects)
        s += (UnityEffect)e + " ";
      s += "\n";
    }

    return s;
  }
}
