// RandomCrewmember.cs
// Created by Aaron C Gaudette on 09.05.17
// Base-case testing Character that makes decisions randomly

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class RandomCrewmember : Crewmember {

  public RandomCrewmember(
    string firstName, string lastName, string role, string pronoun
  ) : base(firstName, lastName, role, pronoun) { }

  public override Crewmember ChooseVote() {
    List<Crewmember> targets = new List<Crewmember>();
    // Select random Entity (hack)
    foreach (IEntity entity in Universe.root.entities) {
      // Skip dead targets
      if ((entity as IDestroyable).Destroy) continue;

      targets.Add(entity as Crewmember);
    }

    return targets[Random.Range(0, targets.Count)];
  }

  // Randomly score Interactions
  protected override float Score(
    Interaction interaction, ICollection<IEntity> targets
  ) {
    return Random.Range(0, 1);
  }
}
