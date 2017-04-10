// RandomGenerator.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public class RandomEntity : UnityEntity {

  public RandomEntity(string label) : base(label) { }

  protected override List<Effect> GetReaction(Interaction interaction, Entity host) {
    if (host == this) return null;
    List<Effect> effects = GetClass().effects;
    return effects;
  }

  protected override List<Effect> GetObservation(
    Interaction interaction, Entity host, List<Entity> targets
  ) {
    return null;
  }

  protected override float Score(Interaction interaction, List<Entity> targets) {
    return Random.Range(0, 1f);
  }
}

public class RandomGenerator : MonoBehaviour {

  void Awake() {
    // Initialize
    Universe.root = new Universe();
    Class.root = new Class();

    // Entities
    RandomEntity alice = new RandomEntity("alice");
    RandomEntity john = new RandomEntity("john");

    // Attributes
    UnityAttribute life = new UnityAttribute("life", Class.root, 1);
    UnityAttribute strength = new UnityAttribute("strength", Class.root, .5f);

    // Effects
    UnityEffect waste = new UnityEffect("waste");
    waste.modifiers.Add(new Effect.Modifier(life, -0.1f));
    waste.modifiers.Add(new Effect.Modifier(strength, -0.2f));

    UnityEffect strengthen = new UnityEffect("strengthen");
    strengthen.modifiers.Add(new Effect.Modifier(strength, 0.3f));

    // Interactions
    UnityInteraction testSelf = new UnityInteraction("testSelf", 0);
    UnityInteraction testOther = new UnityInteraction("testOther", 1);

    // Class
    Class.root.interactions.Add(testSelf);
    Class.root.interactions.Add(testOther);
    Class.root.effects.Add(waste);
    Class.root.effects.Add(strengthen);

    // Attribution
    alice.Subscribe(Class.root);
    john.Subscribe(Class.root);

    // Universe
    Universe.root.AddEntity(alice);
    Universe.root.AddEntity(john);
  }
}
