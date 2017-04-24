using System.Collections.Generic;

namespace BehaviorEngine.Personality {
  public class Brain {

    public static class BrainInteractionFactory {

      public static Dictionary<string, PersonalityInteraction> allInteractions =
          new Dictionary<string, PersonalityInteraction>();
      
      public static Dictionary<string, PersonalityEffect> allEffects =
          new Dictionary<string, PersonalityEffect>();

      public static void registerInteraction(PersonalityInteraction i) {
        allInteractions.Add(i.Label, i);
      }

      public static void registerEffect(PersonalityEffect e) {
        allEffects.Add(e.Label, e);
      }

      public static List<PersonalityInteraction> getAllInteractions() {
        return new List<PersonalityInteraction>(allInteractions.Values);
      }

      public static List<PersonalityEffect> getAllEffects() {
        return new List<PersonalityEffect>(allEffects.Values);
      }
    }

    PersonalityClass fiveFactors;
    public Dictionary<PersonalityInteraction, ICharacterAction> d;

    /** Make sure BrainInteractionFactory is loaded with interactions that 
     * will be evolved on before instatiating any instances of this
     * class
     */
    public Brain(PersonalityClass fiveFactors) {
      this.fiveFactors = fiveFactors;



      foreach(PersonalityInteraction i in 
        BrainInteractionFactory.getAllInteractions()) {

      }
    }


  }
}