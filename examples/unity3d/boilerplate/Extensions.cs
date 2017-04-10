// Extensions.cs
// Created by Aaron C Gaudette on 04.09.17
// All core classes are extended to add label (debug) information

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public interface ILabeled {
  string Label { get; set; }
  string ToString();
}

public abstract class UnityEntity : Entity, ILabeled {

  public void SetDebug(bool to) { print = to; }

  public bool Print {
    get { return print; }
    set { print = value; }
  }
  bool print = false;

  public string Label {
    get { return label; }
    set { label = value; }
  }
  string label;

  string Prefix { get { return "ENTITY " + Label + ":\n"; } }

  public UnityEntity(string label) : base() {
    Label = label;
  }

  public override string ToString() {
    return Label;
  }

  protected override void OnReact(Interaction interaction, Entity host, List<Effect> effects) {
    if (!print) return;

    string debug = Prefix;
    debug += "REACTION\n";

    debug += "Interaction = " + ((UnityInteraction)interaction).Label + "\n";
    debug += "Host = " + ((UnityEntity)host).Label + (host == this ? " (self)" : "") + "\n";
    debug += Utility.EffectsToString(effects);

    Debug.Log(debug);
  }

  protected override void OnObserve(
    Interaction interaction, Entity host, List<Entity> targets, List<Effect> effects
  ) {
    if (!print) return;

    string debug = Prefix;
    debug += "OBSERVATION\n";
    debug += "Interaction = " + ((UnityInteraction)interaction).Label + "\n";
    debug += "Host = " + ((UnityEntity)host).Label + "\n";

    debug += Utility.EntityLabelsToString(new ReadOnlyCollection<Entity>(targets), "Targets");
    debug += Utility.EffectsToString(effects);

    Debug.Log(debug);
  }

  protected override void OnPoll(
    Interaction choice, ReadOnlyCollection<Entity> targets, float highscore
  ) {
    if (!print) return;

    string debug = Prefix;
    debug += "POLL\n";
    debug += "Interaction = "
      + (choice == null ? "null" : ((UnityInteraction)choice).Label) + "\n";

    debug += Utility.EntityLabelsToString(targets, "Targets");
    debug += "Score = " + highscore + "\n";

    Debug.Log(debug);
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
