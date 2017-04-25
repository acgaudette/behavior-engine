using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class SharedData {
    public static readonly IList<string> PersonalityFactorNames = 
        new List<string> {
      "Agreeableness",
      "Conscientiousness",
      "Extraversion",
      "Neuroticism",
      "Openness"
    };
    public static readonly IList<string> PersonalityPropertyNames =
        new List<string> {
      "angry",
      "sad",
      "happy",
      "confused"
    };
  }
}