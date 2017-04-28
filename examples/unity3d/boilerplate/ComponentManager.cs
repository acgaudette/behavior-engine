// ComponentManager.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorEngine;

using System;
using System.Reflection;

[ExecuteInEditMode]
public class ComponentManager : MonoBehaviour {

  const string DEBUG_FLAG = "BVE_DEBUG";

  public bool compileWithDebugLabeling = true;
  public float defaultPollRate = 8; // Rate to update all universes/entities

  // Editor mirrors to Unity components (for display purposes)
  public List<UniverseComponent> universes = new List<UniverseComponent>();
  public List<RepoComponent> repos = new List<RepoComponent>();

  void Awake() {
    BehaviorEngine.Debug.Logger.SetLogger(m => Debug.Log(m));
  }

  void Update() {
    // Recompile with debug flag
    BuildTargetGroup group = EditorUserBuildSettings.selectedBuildTargetGroup;
    string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
    bool debug = symbols.Contains(DEBUG_FLAG);

    if (compileWithDebugLabeling != debug) {
      if (compileWithDebugLabeling) {
        string to = symbols == "" ? DEBUG_FLAG : symbols + ";" + DEBUG_FLAG;

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
          group, to
        );

        Debug.Log(
          "ComponentManager: Set scripting define symbols to \"" + to + "\""
        );
      } else {
        int index = symbols.IndexOf(";" + DEBUG_FLAG);
        int count = DEBUG_FLAG.Length + 1;
        if (index < 0) {
          index = symbols.IndexOf(DEBUG_FLAG);
          count--;
        }

        if (index >= 0) {
          string to = symbols.Remove(index, count);

          PlayerSettings.SetScriptingDefineSymbolsForGroup(
            group, to
          );

          Debug.Log(
            "ComponentManager: Set scripting define symbols to \"" + to + "\""
          );
        }
      }
    }

    if (!Application.isPlaying) return;

    for (int i = universes.Count - 1; i > 0; --i) {
      if (universes[i].reference == null) {
        Destroy(universes[i]);
        universes.RemoveAt(i);
      }
    }

    for (int i = repos.Count - 1; i > 0; --i) {
      if (repos[i].reference == null) {
        Destroy(repos[i]);
        repos.RemoveAt(i);
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

  // Generates a RepoComponent within the scene
  // Returns a reference to the generated component
  public R Hook<R>(string label, IRepository reference) where R : RepoComponent {
    foreach (R c in repos) {
      if (c.reference == reference)
        return null;
    }

    GameObject o = new GameObject();
    R component = o.AddComponent<R>();

    component.reference = reference;
    o.name = label;
    o.transform.parent = transform;

    repos.Add(component);
    return component;
  }

  // Generate Entity components within the scene
  public void GenerateEntities(
    UniverseComponent parent, ICollection<IEntity> latest
  ) {
    foreach (EntityComponent target in parent.entities)
      Destroy(target.gameObject);

    parent.entities.Clear();
    foreach (IEntity target in latest) {
      GameObject o = new GameObject();
      EntityComponent component = o.AddComponent<EntityComponent>();
      component.reference = new UnityEntity(target); // Decorate

      o.name = target.GetDebugLabel();

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
