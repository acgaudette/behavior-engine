// CharacterDebugger.cs
// Created by Aaron C Gaudette on 09.05.17

using System.Diagnostics;
using BehaviorEngine.Personality;

namespace BehaviorEngine.Debug {

  public static class CharacterDebugger {

#if BVE_DEBUG

    static Character.OnUpdateRelationshipEventHandler RenderRelationship = (
      object sender,
      Character target,
      float trustOffset, float agreementOffset,
      Relationship relationship
    ) => {
      Character character = sender as Character;

      string debug = character.GetDebugLabel() + "\n";
      debug += "RELATIONSHIP\n";

      string t = (trustOffset >= 0 ? "+" : "")
        + trustOffset;
      string a = (agreementOffset >= 0 ? "+" : "")
        + agreementOffset;

      debug += "Target = " + target.GetDebugLabel() + "\n";
      debug += "Agreement " + a + ", Trust " + t + "\n";
      debug += "Axes = " + relationship.RenderAxes() + "\n";

      Logger.Log(debug);
    };

#endif

    [Conditional("BVE_DEBUG")]
    public static void Attach(Character character) {

#if BVE_DEBUG

      character.OnUpdateRelationship += RenderRelationship;

#endif

    }

    [Conditional("BVE_DEBUG")]
    public static void Detach(Character character) {

#if BVE_DEBUG

      character.OnUpdateRelationship -= RenderRelationship;

#endif

    }
  }
}
