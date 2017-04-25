// Forum.cs
// Created by Aaron C Gaudette on 10.04.17

using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;

public class Forum : MonoBehaviour {

  public static NormalizedAttribute trollFactor, anger;
  public static Effect annoy, incite, calm;
  public static Interaction start, flame, quit;

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    Class.root = new Class();

    // Attributes
    trollFactor = new NormalizedAttribute(
      () => Random.Range(0, 1f)
    );
    anger = new NormalizedAttribute(() => 0);

    Class.root.attributes.Add(trollFactor);
    Class.root.attributes.Add(anger);

    trollFactor.SetLabel("Troll Factor");
    anger.SetLabel("Anger");

    // Users
    User[] users = {
      new User("Julian"),
      new User("Andy"),
      new User("Mike")
    };

    // Effects
    annoy = new Effect();
    annoy.modifiers.Add(new FloatModifier(anger, .2f));
    Class.root.effects.Add(annoy);
    annoy.SetLabel("Annoy");

    incite = new Effect();
    incite.modifiers.Add(new FloatModifier(anger, .4f));
    Class.root.effects.Add(incite);
    incite.SetLabel("Anger");

    calm = new Effect();
    calm.modifiers.Add(new FloatModifier(anger, -.1f));
    Class.root.effects.Add(calm);
    calm.SetLabel("Calm Down");

    // Interactions
    start = new Interaction(0);
    flame = new Interaction(1);
    quit = new Interaction(0);

    Class.root.interactions.Add(start);
    Class.root.interactions.Add(flame);
    Class.root.interactions.Add(quit);

    start.SetLabel("Start Thread");
    flame.SetLabel("Flame");
    quit.SetLabel("Ragequit");

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
