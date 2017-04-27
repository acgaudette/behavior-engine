// Transformations.cs
// Created by Aaron C Gaudette on 26.05.17

using Transformation = BehaviorEngine.Float.NormalizedAttribute.Transform;

namespace BehaviorEngine.Float {

  // Normalized (0 - 1) transformations
  public static class Transformations {

    public static Transformation Linear() {
      return x => x;
    }
  }
}
