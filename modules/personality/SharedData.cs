using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class SharedData {

    public static readonly IList<string> PersonalityPropertyNames =
      new List<string> {
        "angry",
        "sad",
        "happy",
        "confused"
      };
  }
}
