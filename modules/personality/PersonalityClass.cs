// PersonalityClass.cs
// Created by Daniel W. Zhang on 04.23.17
// Extension of BehaviorEngine's Class for the personality module

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class PersonalityClass : Class {

    public Dictionary<string, PersonalityFactor> personalityFactors;

    public PersonalityClass(Dictionary<string, 
      BehaviorEngine.Attribute<float>.InitializeState> delegates = null) : 
      base() {

      Random r = new Random ();

      personalityFactors = new Dictionary<string, PersonalityFactor> ();

      foreach (string factor in SharedData.names) {
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
}
