// ComponentManager.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

using System;
using System.Reflection;
using UnityEditor;

public class ComponentManager : MonoBehaviour {

  public float defaultPollRate = 8; // Rate to update all universes/entities

  // Editor mirrors to Unity components (for display purposes)
  public List<UniverseComponent> universes = new List<UniverseComponent>();
  public List<ClassComponent> classes = new List<ClassComponent>();

  void Awake() {
    Root.logger = m => Debug.Log(m);
  }

  void Update() {
    for (int i = universes.Count - 1; i > 0; --i) {
      if (universes[i].reference == null) {
        Destroy(universes[i]);
        universes.RemoveAt(i);
      }
    }

    for (int i = classes.Count - 1; i > 0; --i) {
      if (classes[i].reference == null) {
        Destroy(classes[i]);
        classes.RemoveAt(i);
      }
    }
  }

  // Generates a Universe component within the scene
  // Returns a reference to the generated component
  public UniverseComponent Hook(string label, Universe reference) {
    foreach (UniverseComponent c in universes) {
      if (c.reference == reference)
        return null;
    }

    GameObject o = new GameObject();
    UniverseComponent component = o.AddComponent<UniverseComponent>();

    component.pollRate = defaultPollRate;
    component.manager = this;
    component.reference = reference;

    o.name = label;
    o.transform.parent = transform;

    universes.Add(component);
    return component;
  }

  // Generates a Class component within the scene
  // Returns a reference to the generated component
  public ClassComponent Hook(string label, Class reference) {
    foreach (ClassComponent c in classes) {
      if (c.reference == reference)
        return null;
    }

    GameObject o = new GameObject();
    ClassComponent component = o.AddComponent<ClassComponent>();

    component.reference = reference;
    o.name = label;
    o.transform.parent = transform;

    classes.Add(component);
    return component;
  }

  // Generate Entity components within the scene
  public void GenerateEntities(
    UniverseComponent parent, ICollection<Entity> latest
  ) {
    foreach (EntityComponent target in parent.entities)
      Destroy(target.gameObject);

    parent.entities.Clear();
    foreach (Entity target in latest) {
      if (!(target is UnityEntity)) continue;

      GameObject o = new GameObject();
      EntityComponent component = o.AddComponent<EntityComponent>();
      component.reference = target as UnityEntity;

      o.name = target.GetLabel();

      o.transform.parent = parent.transform;
      parent.entities.Add(component);
    }
  }

  // Clears the Unity console via reflection
  public void ClearConsole() {
    Assembly.GetAssembly(typeof(SceneView))
      .GetType("UnityEditorInternal.LogEntries")
      .GetMethod("Clear")
      .Invoke(new object(), null);
  }
}
