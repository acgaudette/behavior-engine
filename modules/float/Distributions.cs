// Distributions.cs
// Created by Aaron C Gaudette on 25.04.17

using System;

using Distribution = BehaviorEngine.Attribute<float>.InitializeState;

namespace BehaviorEngine.Float {

  // Normalized (0 - 1) distributions
  public static class Distributions {

    static Random random = new Random();

    public static Distribution Uniform() {
      return () => (float)random.NextDouble();
    }

    public static Distribution Normal(float deviation = .5f / 3) {
      // Box-Muller
      double u = random.NextDouble(), v = random.NextDouble();
      return () => (float)(
        deviation * Math.Sqrt(-2 * Math.Log(u))
          * Math.Cos(2 * Math.PI * v) + .5f
      );
    }
  }
}
