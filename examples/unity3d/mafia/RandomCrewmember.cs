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
      if ((entity as IDestroyable).Destroy || entity == this)
        continue;

      targets.Add(entity as Crewmember);
    }

    return targets[Random.Range(0, targets.Count)];
  }

  // Randomly select a reaction Effect
  protected override IList<Effect> Reaction(
    Interaction interaction, IEntity host
  ) {
    int i = 0;
    int target = Random.Range(0, BrainRepo.Effects.Count);
    Effect effect = null;

    foreach (Effect e in BrainRepo.Effects) {
      if (i == target) {
        effect = e;
        break;
      }
      i++;
    }

    return new List<Effect>(1) { effect };
  }

  // Randomly select an observation Effect
  protected override IList<Effect> Observation(
    Interaction interaction, IEntity host, ICollection<IEntity> targets
  ) {
    return Reaction(interaction, host);
  }

  // Randomly score Interactions
  protected override float Score(
    Interaction interaction, ICollection<IEntity> targets
  ) {
    return Random.Range(0, 1f);
  }
}
