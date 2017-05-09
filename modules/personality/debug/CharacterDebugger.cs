// CharacterDebugger.cs
// Created by Aaron C Gaudette on 09.05.17

using System.Diagnostics;
using BehaviorEngine.Personality;

namespace BehaviorEngine.Debug {

  public static class CharacterDebugger {

#if BVE_DEBUG

    static Character.OnAugmentRelationshipEventHandler
    LogAugment = (
      object sender,
      Character target,
      float agreeabilityOffset, float trustworthinessOffset,
      Relationship relationship
    ) => {
      Character character = sender as Character;

      string debug = character.GetDebugLabel() + "\n";
      debug += "RELATIONSHIP\n";

      string trust = (trustworthinessOffset >= 0 ? "+" : "")
        + trustworthinessOffset;
      string agree = (agreeabilityOffset >= 0 ? "+" : "")
        + agreeabilityOffset;

      debug += "Target = " + target.GetDebugLabel() + "\n";
      debug += "Trustworthiness " + trust
        + ", Agreeability " + agree + "\n";
      debug += "Axis = " + relationship.RenderAxis() + "\n";

      Logger.Log(debug);
    };

#endif

    [Conditional("BVE_DEBUG")]
    public static void Attach(Character character) {

#if BVE_DEBUG

      character.OnAugmentRelationship += LogAugment;

#endif

    }

    [Conditional("BVE_DEBUG")]
    public static void Detach(Character character) {

#if BVE_DEBUG

      character.OnAugmentRelationship -= LogAugment;

#endif

    }
  }
}
