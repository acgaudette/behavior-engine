// ComponentManager.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

public class ComponentManager : MonoBehaviour {

  public float pollRate = 2;

  public List<UniverseComponent> universes = new List<UniverseComponent>();
  public List<ClassComponent> classes = new List<ClassComponent>();

  void Start() {
    // Only sees the roots for now
    GenerateUniverse(Universe.root, "Universe.root");
    GenerateClass(Class.root, "Class.root");
  }

  public UniverseComponent GenerateUniverse(Universe reference, string label) {
    GameObject o = new GameObject();
    UniverseComponent component = o.AddComponent<UniverseComponent>();

    component.reference = reference;
    component.manager = this;

    o.name = label;
    o.transform.parent = transform;

    universes.Add(component);
    return component;
  }

  public ClassComponent GenerateClass(Class reference, string label) {
    GameObject o = new GameObject();
    ClassComponent component = o.AddComponent<ClassComponent>();

    component.reference = reference;
    o.name = label;
    o.transform.parent = transform;

    classes.Add(component);
    return component;
  }

  public ReadOnlyCollection<Entity> GenerateEntities(
    List<EntityComponent> cache, ReadOnlyCollection<Entity> latest
  ) {
    foreach (EntityComponent target in cache)
      Destroy(target.gameObject);

    cache.Clear();
    foreach (Entity target in latest) {
      GameObject o = new GameObject();
      EntityComponent component = o.AddComponent<EntityComponent>();
      component.reference = target;

      if (target is ILabeled)
        o.name = (target as ILabeled).Label;
      else o.name = "Unlabeled";
      foreach (UniverseComponent universe in universes) {
        if (universe.reference == target.GetUniverse())
          o.transform.parent = universe.transform;
      }

      cache.Add(component);
    }

    return latest;
  }
}
