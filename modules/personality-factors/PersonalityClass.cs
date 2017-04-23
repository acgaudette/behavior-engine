using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;

public class PersonalityClass : Class {

  public Agreeableness agreeableness;
  public Conscientiousness conscientiousness;
  public Extraversion extraversion;
  public Neuroticism neuroticism;
  public Openness openness;

  /** You must initialize PersonalityAttributes outside! */
  public PersonalityClass() : base() {
    attributes.Add (agreeableness);
    attributes.Add (conscientiousness);
    attributes.Add (extraversion);
    attributes.Add (neuroticism);
    attributes.Add (openness);
  }
}
