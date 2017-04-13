// Extensions.cs
// Created by Aaron C Gaudette on 04.09.17
// All Behavior Engine core classes are extended here with debug information

using System.Collections.Generic;
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

  public float GetAttributeState(IAttribute attribute) {
    return (GetAttribute(attribute) as UnityAttribute.Instance).State;
  }

  public override string ToString() {
    string s = Prefix;
    s += "STATUS\n";

    s += Utility.CollectionToString(
      "Attributes", GetAttributes(), (a) => {
      UnityAttribute.Instance i = a as UnityAttribute.Instance;
      return i == null ? "?" : i.Prototype + " (" + i.State + ")";
      }
    );
    s += Utility.CollectionToString("Interactions", interactions, i => i.ToString());

    return s;
  }

  protected override void OnReact(
    Interaction interaction, Entity host, IList<Effect> effects
  ) {
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
    Interaction interaction, Entity host,
    ICollection<Entity> targets, IList<Effect> effects
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
    Interaction choice, ICollection<Entity> targets, float highscore
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

  public UnityAttribute(
    string label, InitializeState initializeState
  ) : base(initializeState) {
    Label = label;
  }

  public override IAttributeInstance GetNewInstance() {
    return new UnityAttribute.Instance(this);
  }

  public override string ToString() {
    return Label;
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
    string s = Label + " (";

    foreach (IModifier modifier in modifiers) {
      UnityModifier m = modifier as UnityModifier;

      if (m == null) s += "?";
      else {
        s += " " + (m.offset >= 0 ? "+" : "") + m.offset
          + " " + (m.Attribute == null ? "null" : m.Attribute.ToString());
      }
    }

    return s + ")";
  }

  public class UnityModifier : Modifier<float> {

    public UnityModifier(Attribute<float> attribute, float offset) : base(attribute, offset) { }

    protected override void Modify(Attribute<float>.Instance instance) {
      instance.State += offset;
    }
  }
}
