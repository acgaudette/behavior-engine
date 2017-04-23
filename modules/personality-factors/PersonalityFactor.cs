using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public abstract class PersonalityFactor : NormalizedAttribute, ILabeled {

  public float value;

  public string Label { get; set; }

  public PersonalityFactor(InitializeState i) : base(i) {}
}