// Transformations.cs
// Created by Aaron C Gaudette on 26.05.17

using System;

using Transformation = BehaviorEngine.Float.NormalizedAttribute.Transform;

namespace BehaviorEngine.Float {

  // Normalized (0 - 1) transformations
  public static class Transformations {

    public static Transformation Linear() {
      return x => x;
    }

    public static Transformation Squared() {
      return x => x * x;
    }

    public static Transformation InvertedSquared() {
      return x => 1 - (1 - x) * (1 - x);
    }

    public static Transformation Power(float p) {
      return x => (float)Math.Pow(x, p);
    }

    public static Transformation InvertedPower(float p) {
      return x => (float)(1 - Math.Pow(1 - x, p));
    }

    public static Transformation EaseSquared() {
      return x => x * x / (x * x + (1 - x) * (1 - x));
    }

    public static Transformation Ease(float p) {
      return x => (float)(
        Math.Pow(x, p) / (Math.Pow(x, p) + Math.Pow(1 - x, p))
      );
    }
  }
}
