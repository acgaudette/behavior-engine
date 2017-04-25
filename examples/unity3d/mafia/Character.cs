// Placeholder

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class Character : ConsoleCharacter {

  public Character(string name) : base(name) { }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    return Random.Range(0, 1f);
  }
}
