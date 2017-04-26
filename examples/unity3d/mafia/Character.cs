// Character.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;
using BehaviorEngine.Personality;

public class Character : Person, IDestroyable {

  public Character(string name) : base(name) { }

  protected override float Score(
    Interaction interaction, ICollection<IEntity> targets
  ) {
    return Random.Range(0, 1f); // ! (placeholder)
  }

  public bool Destroy { get; set; }
}
