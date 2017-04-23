using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorEngine;

public class PersonalityClass : Class {

  private static string[] names = {
    "Agreeableness",
    "Conscientiousness",
    "Extraversion",
    "Neuroticism",
    "Openness"
  };

  public Dictionary<string, PersonalityFactor> personalityFactors;

  public PersonalityClass(Dictionary<string, 
    BehaviorEngine.Attribute<float>.InitializeState> delegates = null) : base() {

    Random r = new Random ();

    personalityFactors = new Dictionary<string, PersonalityFactor> ();

    foreach (string factor in names) {
      if (delegates == null ||
          !delegates.ContainsKey (factor)) {
        float val = ((float)r.NextDouble ());
        personalityFactors.Add (factor,
          new PersonalityFactor (factor, () => val));
      } else {
        var del = delegates [factor];
        personalityFactors.Add (factor, new PersonalityFactor (factor, del));
      }
      attributes.Add(personalityFactors[factor]);
    }
  }
}
