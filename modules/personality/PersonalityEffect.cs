using System.Collections;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {
  public class PersonalityEffect : Effect, ILabeled {

    public string Label { get; set; }

    public PersonalityEffect() : base() {}
  }
}