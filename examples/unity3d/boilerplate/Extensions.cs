// Extensions.cs
// Created by Aaron C Gaudette on 04.09.17
// All Behavior Engine core classes are extended here with debug information

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public abstract class UnityEntity : Entity, ILabeled {

  public bool destroy = false;

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
    string s = Prefix;
    s += "STATUS\n";

    s += Utility.CollectionToString("Attributes", GetAttributes(), a => a + " (" + a.State + ")");
    s += Utility.CollectionToString("Interactions", interactions, i => i.ToString());

    return s;
  }

  protected override void OnReact(Interaction interaction, Entity host, List<Effect> effects) {
    if (print) {
      string debug = Prefix;
      debug += "REACTION\n";

      debug += "Interaction = " + (interaction as ILabeled).Label + "\n";
      debug += "Host = " + (host as ILabeled).Label
        + (host == this ? " (self)" : "") + "\n";
      debug += Utility.CollectionToString("Effects", effects, e => e.ToString());

      Debug.Log(debug);
    }
  }

  protected override void OnObserve(
    Interaction interaction, Entity host, List<Entity> targets, List<Effect> effects
  ) {
    if (print) {
      string debug = Prefix;
      debug += "OBSERVATION\n";
      debug += "Interaction = " + (interaction as ILabeled).Label + "\n";
      debug += "Host = " + (host as ILabeled).Label + "\n";

      debug += Utility.CollectionToString("Targets", targets, t => (t as ILabeled).Label);
      debug += Utility.CollectionToString("Effects", effects, e => e.ToString());

      Debug.Log(debug);
    }
  }

  protected override void OnPoll(
    Interaction choice, ReadOnlyCollection<Entity> targets, float highscore
  ) {
    if (print) {
      string debug = Prefix;
      debug += "POLL\n";
      debug += "Interaction = "
        + (choice == null ? "null" : (choice as ILabeled).Label) + "\n";

      debug += Utility.CollectionToString("Targets", targets, t => (t as ILabeled).Label);
      debug += "Score = " + highscore + "\n";

      Debug.Log(debug);
    }
  }
}

public class UnityAttribute : NormalizedAttribute, ILabeled {
  string label;

  public string Label {
    get { return label; }
    set { label = value; }
  }

  // Archetype
  public UnityAttribute(
    string label, Class family, InitializeState initializeState
  ) : base(family, initializeState) {
    Label = label;
  }

  // Instance
  protected UnityAttribute(Attribute attribute) : base(attribute) { }

  public override Attribute GetNewInstance() {
    return new UnityAttribute(this);
  }

  public override string ToString() {
    return Instance ? ID + "E_" + (GetArchetype() as ILabeled).Label :
      ID + "A_" + Label;
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
    string s = Label + " (Mods = ";

    foreach (Effect.Modifier modifier in modifiers) {
      s += "<" + (modifier.attribute == null ?
        "null" : modifier.attribute.ToString())
        + ", " + (modifier.offset >= 0 ? "+" : "") + modifier.offset + "> ";
    }

    return s + ")";
  }
}
