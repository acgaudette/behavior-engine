using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class Person : Entity, ILabeled {
  public Person(string label) : base(label) {
  }

  protected override IList<Effect> GetReaction(
    Interaction interaction, Entity host
  ) {
    return new List<Effect> ();
  }

  protected override IList<Effect> GetObservation(
    Interaction interaction, Entity host, ICollection<Entity> targets
  ) {
    return new List<Effect> ();
  }

  protected override float Score(
    Interaction interaction, ICollection<Entity> targets
  ) {
    if (destroy) {
      return 0;
    }
    return 1;
  }
}
