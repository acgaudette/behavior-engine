// PersonalityClass.cs
// Created by Daniel W. Zhang on 04.23.17
// Extension of BehaviorEngine's Class for the personality module

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class PersonalityFactorClass : Class {

    public Dictionary<string, PersonalityFactor> personalityFactors;

    public PersonalityFactorClass(Dictionary<string, 
      BehaviorEngine.Attribute<float>.InitializeState> delegates = null) : 
      base() {

      Random r = new Random ();

      personalityFactors = new Dictionary<string, PersonalityFactor> ();

      foreach (string factor in SharedData.PersonalityFactorNames) {
        if (delegates == null ||
            !delegates.ContainsKey (factor)) {
          float val = ((float)r.NextDouble ());
          personalityFactors[factor] =
              new PersonalityFactor(factor, () => val);
        } else {
          var del = delegates [factor];
          personalityFactors[factor] = new PersonalityFactor (factor, del);
        }
        attributes.Add(personalityFactors[factor]);
      }
    }
  }
}
