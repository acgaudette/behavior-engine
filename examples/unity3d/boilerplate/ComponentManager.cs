// ComponentManager.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BehaviorEngine;

using System;
using System.Reflection;
using UnityEditor;

public class ComponentManager : MonoBehaviour {

  public float pollRate = 4; // Rate to update all universes/entities
  [HideInInspector] public float lastPoll;

  public void IncrementTick() { tick++; }
  [SerializeField] ulong tick = 0;

  // Editor references
  public List<UniverseComponent> universes = new List<UniverseComponent>();
  public List<ClassComponent> classes = new List<ClassComponent>();

  void Awake() {
    lastPoll = -pollRate;
  }

  void Start() {
    // Only roots are visible for now
    GenerateUniverse(Universe.root, "Universe.root");
    GenerateClass(Class.root, "Class.root");
  }

  // Generate universe component within the scene
  // Returns a reference to the generated component
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

  // Generate class component within the scene
  // Returns a reference to the generated component
  public ClassComponent GenerateClass(Class reference, string label) {
    GameObject o = new GameObject();
    ClassComponent component = o.AddComponent<ClassComponent>();

    component.reference = reference;
    o.name = label;
    o.transform.parent = transform;

    classes.Add(component);
    return component;
  }

  // Generate entity components within the scene
  // Returns the latest ReadOnlyCollection of the core entities
  public ReadOnlyCollection<Entity> GenerateEntities(
    List<EntityComponent> cache, ReadOnlyCollection<Entity> latest
  ) {
    foreach (EntityComponent target in cache)
      Destroy(target.gameObject);

    cache.Clear();
    foreach (Entity target in latest) {
      if (!(target is UnityEntity)) continue;

      GameObject o = new GameObject();
      EntityComponent component = o.AddComponent<EntityComponent>();
      component.reference = target as UnityEntity;

      // Name
      if (target is ILabeled)
        o.name = (target as ILabeled).Label;
      else o.name = "Unlabeled";

      // Parent
      foreach (UniverseComponent universe in universes) {
        if (universe.reference == target.GetUniverse())
          o.transform.parent = universe.transform;
      }

      cache.Add(component);
    }

    return latest;
  }

  public void ClearConsole() {
    Assembly.GetAssembly(typeof(SceneView))
      .GetType("UnityEditorInternal.LogEntries")
      .GetMethod("Clear")
      .Invoke(new object(), null);
  }
}
