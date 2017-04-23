using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Person : Entity, ILabeled {

    public string Label { set; get; }

    public Person(string label) : base() {
      Label = label;
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
      return 1;
    }
  }
}
