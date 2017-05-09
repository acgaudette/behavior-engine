// CharacterDebugger.cs
// Created by Aaron C Gaudette on 09.05.17

using System.Diagnostics;
using BehaviorEngine.Personality;

namespace BehaviorEngine.Debug {

  public static class CharacterDebugger {

#if BVE_DEBUG

    static Character.OnRegisterRelationshipEventHandler
    LogRelationshipRegistration = (
      object sender,
      Character target, Relationship relationship
    ) => {
      Character character = sender as Character;

      string debug = character.GetDebugLabel() + "\n";
      debug += "RELATIONSHIP\n";

      debug += "Target = " + target.GetDebugLabel() + "\n";
      debug += "Axis = " + relationship.RenderAxis() + "\n";

      Logger.Log(debug);
    };

#endif

    [Conditional("BVE_DEBUG")]
    public static void Attach(Character character) {

#if BVE_DEBUG

      character.OnRegisterRelationship += LogRelationshipRegistration;

#endif

    }

    [Conditional("BVE_DEBUG")]
    public static void Detach(Character character) {

#if BVE_DEBUG

      character.OnRegisterRelationship -= LogRelationshipRegistration;

#endif

    }
  }
}
