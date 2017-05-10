// Relationship.cs

using System;
using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class Relationship {

    public class Affinities {

      public enum MatchResult { POSITIVE = 1, NEGATIVE = -1, DNE = 0 };

      Dictionary<string, State> positive, negative;

      public Affinities() {
        positive = new Dictionary<string, State>();
        negative = new Dictionary<string, State>();
      }

      public Affinities (
        IEnumerable<State> positive,
        IEnumerable<State> negative
      ) : this() {
        RegisterPositive(positive);
        RegisterNegative(negative);
      }

      public void RegisterPositive(IEnumerable<State> states) {
        foreach (State state in states)
          positive[state.name] = state;
      }

      public void RegisterNegative(IEnumerable<State> states) {
        foreach (State state in states)
          negative[state.name] = state;
      }

      public MatchResult Match(string name) {
        return positive.ContainsKey(name) ? MatchResult.POSITIVE
          : negative.ContainsKey(name) ? MatchResult.NEGATIVE
          : MatchResult.DNE;
      }
    }

    public class Axis {

      public float Value { get; private set; }

      public Affinities affinities;

      public static implicit operator float(Axis a) {
        return a.Value;
      }

      public Axis() {
        Value = 0; // Center
        affinities = new Affinities();
      }

      public Axis(Affinities affinities) {
        Value = 0; // Center
        this.affinities = affinities;
      }

      public void Offset(float offset) {
        Value = Math.Min(1, Math.Max(-1, Value + offset));
      }
    }

    public readonly Character target;

    public Axis trust, agreement;

    public Relationship(Character target) {
      this.target = target;

      trust = new Axis();
      agreement = new Axis();
    }

    public string RenderAxes() {
      return "( " + trust.Value + ", " + agreement.Value + " )";
    }
  }
}
