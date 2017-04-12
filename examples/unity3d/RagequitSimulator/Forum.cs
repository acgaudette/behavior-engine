// Forum.cs
// Created by Aaron C Gaudette on 10.04.17

using UnityEngine;
using BehaviorEngine;

public class Forum : MonoBehaviour {

  // Probably should be storing these in a Class, not here
  public static UnityAttribute trollFactor, anger;
  public static UnityEffect annoy, incite, calm;
  public static UnityInteraction start, flame, quit;

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    Class.root = new Class();

    // Attributes
    trollFactor = new UnityAttribute("Troll Factor", Class.root, 0);
    anger = new UnityAttribute("Anger", Class.root, 0);

    // Users
    User julian = new User("Julian");
    User andy = new User("Andy");
    User mike = new User("Mike");

    User[] users = { julian, andy, mike };

    // Effects
    annoy = new UnityEffect("Annoy");
    annoy.modifiers.Add(new Effect.Modifier(anger, .2f));
    Class.root.effects.Add(annoy);

    incite = new UnityEffect("Anger");
    incite.modifiers.Add(new Effect.Modifier(anger, .4f));
    Class.root.effects.Add(incite);

    calm = new UnityEffect("Calm Down");
    calm.modifiers.Add(new Effect.Modifier(anger, -.1f));
    Class.root.effects.Add(calm);

    // Interactions
    start = new UnityInteraction("Start Thread", 0);
    flame = new UnityInteraction("Flame", 1);
    quit = new UnityInteraction("Ragequit", 0);

    Class.root.interactions.Add(start);
    Class.root.interactions.Add(flame);
    Class.root.interactions.Add(quit);

    // Attribution
    foreach (User user in users) {
      user.Subscribe(Class.root);
      Universe.root.AddEntity(user);
    }

    // Initialize attributes
    julian.GetAttribute(trollFactor).Modify(.4f);
    andy.GetAttribute(trollFactor).Modify(.7f);
    mike.GetAttribute(trollFactor).Modify(.1f);
  }
}
