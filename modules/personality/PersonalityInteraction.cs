namespace BehaviorEngine.Personality {

  public class PersonalityInteraction : Interaction, ILabeled {

    public string Label { get; set; }

    public PersonalityInteraction(int limiter) : base(limiter) {

    }
  }
}
