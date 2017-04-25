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

    PersonalityFactorClass fiveFactors;
    PersonalityPropertyClass properties; 

    /** Make sure BrainInteractionFactory is loaded with interactions and
     * properties before instatiating any instances of this class
     */
    public Brain(PersonalityFactorClass fiveFactors,
        PersonalityPropertyClass properties) {
      this.fiveFactors = fiveFactors;
      this.properties = properties;
    }

    public PersonalityEffect EffectFromInteraction(Interaction i) {
      //TODO
      return null;
    }
  }
}