// Mafia.cs
// Created by Aaron C Gaudette on 24.04.17

using UnityEngine;
using BehaviorEngine;

public class Mafia : MonoBehaviour {

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    Class.root = new Class();

    // Users
    Character[] characters = {
      new Character("Julian"),
      new Character("Andy"),
      new Character("Eugene")
    };

    /* See Forum.cs for examples */

    // Attribution
    foreach (Character character in characters) {
      character.Subscribe(Class.root);
      Universe.root.entities.Add(character);
    }

    // Unity hooks
    GetComponent<ComponentManager>().Hook("Universe.root", Universe.root);
    GetComponent<ComponentManager>().Hook("Class.root", Class.root);
  }
}
