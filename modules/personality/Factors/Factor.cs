// PersonalityFactor.cs
// Created by Daniel W. Zhang on 23.04.17
// General structure for a personality factor

namespace BehaviorEngine.Personality {

  public class Factor : Float.NormalizedAttribute {

    public FactorEnum factorType;

    public Factor(FactorEnum factorType, InitializeState i) : base(i) {
      this.factorType = factorType;
    }
  }
}
