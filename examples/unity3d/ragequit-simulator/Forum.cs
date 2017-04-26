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
    SimpleRepo repo = new SimpleRepo();

    // Attributes
    trollFactor = new NormalizedAttribute(
      () => Random.Range(0, 1f)
    );
    anger = new NormalizedAttribute(() => 0);

    repo.RegisterAttribute(trollFactor);
    repo.RegisterAttribute(anger);

    trollFactor.SetDebugLabel("Troll Factor");
    anger.SetDebugLabel("Anger");

    // Users
    User[] users = {
      new User("Julian"),
      new User("Andy"),
      new User("Mike")
    };

    // Effects
    annoy = new Effect();
    annoy.modifiers.Add(new FloatModifier(anger, .2f));
    repo.effects.Add(annoy);
    annoy.SetDebugLabel("Annoy");

    incite = new Effect();
    incite.modifiers.Add(new FloatModifier(anger, .4f));
    repo.effects.Add(incite);
    incite.SetDebugLabel("Anger");

    calm = new Effect();
    calm.modifiers.Add(new FloatModifier(anger, -.1f));
    repo.effects.Add(calm);
    calm.SetDebugLabel("Calm Down");

    // Interactions
    start = new Interaction(0);
    flame = new Interaction(1);
    quit = new Interaction(0);

    repo.RegisterInteraction(start);
    repo.RegisterInteraction(flame);
    repo.RegisterInteraction(quit);

    start.SetDebugLabel("Start Thread");
    flame.SetDebugLabel("Flame");
    quit.SetDebugLabel("Ragequit");

    // Attribution
    foreach (User user in users) {
      user.Subscribe(repo);
      Universe.root.entities.Add(user);
    }

    // Unity hooks
    ComponentManager hook = GetComponent<ComponentManager>();
    hook.Hook("Universe.root", Universe.root);
    hook.Hook<SimpleRepoComponent>("simple-repo", repo);
  }
}
