// Mafia.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Personality;

public class Mafia : MonoBehaviour {

  void Awake() {
    // Initialize environment
    Universe.root = new Universe();
    Class.root = new Class();

    HashSet<PersonalityFactor> inputFactors = new HashSet<PersonalityFactor>();
    inputFactors.Add(new PersonalityFactor(SharedData.PersonalityFactorNames[1],
      () => .2f));
    HashSet<PersonalityProperty> inputProperties = 
      new HashSet<PersonalityProperty>();
    inputProperties.Add(new PersonalityProperty(
      SharedData.PersonalityPropertyNames[0], () => .5f));
    Dictionary<PersonalityProperty, float> inputTargets = 
      new Dictionary<PersonalityProperty, float>();
    inputTargets[new PersonalityProperty(
      SharedData.PersonalityPropertyNames[3], () => .2f)] = .3f;
    Brain.CentralBrainRepository.registerEffect(
      new PersonalityEffect(inputFactors, inputProperties, inputTargets, null));

    // Users
    Character[] characters = {
      new Character("Julian", null, null),
      new Character("Andy", null, null),
      new Character("Eugene", null, null)
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
