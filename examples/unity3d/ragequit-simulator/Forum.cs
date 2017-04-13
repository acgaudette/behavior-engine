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
    trollFactor = new UnityAttribute(
      "Troll Factor", () => Random.Range(0, 1f)
    );
    anger = new UnityAttribute("Anger", () => 0);

    Class.root.attributes.Add(trollFactor);
    Class.root.attributes.Add(anger);

    // Users
    User[] users = {
      new User("Julian"),
      new User("Andy"),
      new User("Mike")
    };

    // Effects
    annoy = new UnityEffect("Annoy");
    annoy.modifiers.Add(new UnityEffect.UnityModifier(anger, .2f));
    Class.root.effects.Add(annoy);

    incite = new UnityEffect("Anger");
    incite.modifiers.Add(new UnityEffect.UnityModifier(anger, .4f));
    Class.root.effects.Add(incite);

    calm = new UnityEffect("Calm Down");
    calm.modifiers.Add(new UnityEffect.UnityModifier(anger, -.1f));
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
      Universe.root.entities.Add(user);
    }

    // Unity hooks
    GetComponent<ComponentManager>().Hook("Universe.root", Universe.root);
    GetComponent<ComponentManager>().Hook("Class.root", Class.root);
  }
}
