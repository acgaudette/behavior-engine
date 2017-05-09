using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Relationship {

    public Character target;

    //TODO: Replace with two Dictionaries of lists, where key is the associated
    //value for relationship (agreeability, trust, etc.). One is for positive,
    //other for negative. Perhaps a custom data structure?

    public float agreeability;
    public List<State> positiveAgreeableStates;
    public List<State> negativeAgreeableStates;

    public float trustworthiness;
    public List<State> positiveTrustStates;
    public List<State> negativeTrustStates;

    public Relationship(Character p) : this(p, .5f, .5f) { }

    public Relationship(Character p, float agree, float trust) {
      target = p;

      agreeability = agree;
      trustworthiness = trust;

      positiveAgreeableStates = new List<State>();
      negativeAgreeableStates = new List<State>();
      positiveTrustStates = new List<State>();
      negativeTrustStates = new List<State>();
    }

    public void registerPositiveAgreeNegativeTrust(
      List<State> relevantStates
    ) {
      positiveAgreeableStates.AddRange(relevantStates);
      negativeTrustStates.AddRange(relevantStates);
    }

    public void registerNegativeAgreePositiveTrust(
      List<State> relevantStates
    ) {
      negativeAgreeableStates.AddRange(relevantStates);
      positiveTrustStates.AddRange(relevantStates);
    }

    public void registerPositiveAgree(List<State> relevantStates) {
      positiveAgreeableStates.AddRange(relevantStates);
    }

    public void registerNegativeAgree(List<State> relevantStates) {
      negativeAgreeableStates.AddRange(relevantStates);
    }

    public void registerPositiveTrust(List<State> relevantStates) {
      positiveTrustStates.AddRange(relevantStates);
    }

    public void registerNegativeTrust(List<State> relevantStates) {
      negativeTrustStates.AddRange(relevantStates);
    }

    public string RenderAxis() {
      return "( " + trustworthiness + ", " + agreeability + " )";
    }
  }
}
