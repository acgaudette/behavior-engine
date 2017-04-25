// PersonalityClass.cs
// Created by Daniel W. Zhang on 23.04.17
// Extension of BehaviorEngine's Class for the personality module

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class FactorClass : Class {

    public Dictionary<FactorEnum, Factor> personalityFactors;

    public FactorClass(Dictionary<FactorEnum, 
      BehaviorEngine.Attribute<float>.InitializeState> delegates = null) : 
      base() {

      Random r = new Random ();

      personalityFactors = new Dictionary<FactorEnum, Factor> ();

      foreach (FactorEnum factor in Enum.GetValues(typeof(FactorEnum))) {
        if (delegates == null ||
            !delegates.ContainsKey (factor)) {
          float val = ((float)r.NextDouble ());
          personalityFactors[factor] =
              new Factor(factor, () => val);
        } else {
          var del = delegates [factor];
          personalityFactors[factor] = new Factor(factor, del);
        }
        attributes.Add(personalityFactors[factor]);
      }
    }
  }
}
