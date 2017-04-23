using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class PersonalityInteraction : Interaction, ILabeled {

  public string Label { get; set; }

  public PersonalityInteraction(int limiter) : base(limiter) {

  }
}
