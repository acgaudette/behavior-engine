// Extensions.cs
// Created by Aaron C Gaudette on 04.09.17
// All core classes are extended to add label (debug) information

using BehaviorEngine;

public interface ILabeled {
  string Label { get; set; }
  string ToString();
}

public abstract class UnityEntity : Entity, ILabeled {
  string label;

  public string Label {
    get { return label; }
    set { label = value; }
  }

  public UnityEntity(string label) : base() {
    Label = label;
  }

  public override string ToString() {
    return Label;
  }
}

public class UnityAttribute : NormalizedAttribute, ILabeled {
  string label;

  public string Label {
    get { return label; }
    set { label = value; }
  }

  public UnityAttribute(
    string label, Class family, float initialState
  ) : base(family, initialState) {
    Label = label;
  }

  public override string ToString() {
    return Instance ? "I" + ID + "_" + (family[ID] as UnityAttribute).Label :
      "A" + ID + "_" + Label;
  }
}

public class UnityInteraction : Interaction, ILabeled {
  string label;

  public string Label {
    get { return label; }
    set { label = value; }
  }

  public UnityInteraction(string label, int limiter) : base(limiter) {
    Label = label;
  }

  public override string ToString() {
    return Label;
  }
}

public class UnityEffect : Effect, ILabeled {
  string label;

  public string Label {
    get { return label; }
    set { label = value; }
  }

  public UnityEffect(string label) : base() {
    Label = label;
  }

  public override string ToString() {
    string s = Label + " [ Mods = ";

    foreach (Effect.Modifier modifier in modifiers) {
      s += "<" + (modifier.attribute == null ?
        "null" : modifier.attribute.ToString())
        + ", " + (modifier.offset >= 0 ? "+" : "") + modifier.offset + "> ";
    }

    return s + "]";
  }
}
