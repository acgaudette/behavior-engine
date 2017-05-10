// CharacterDebugger.cs
// Created by Aaron C Gaudette on 09.05.17

using System.Diagnostics;
using System.Collections.Generic;
using BehaviorEngine.Personality;

namespace BehaviorEngine.Debug {

  public static class CharacterDebugger {

#if BVE_DEBUG

    static EntityEvents.OnPollEventHandler LogCharacterData = (
      object sender,
      Interaction choice, ICollection<IEntity> targets, float highscore
    ) => {
      Character character = sender as Character;
      string debug = character.GetDebugLabel() + "\n";
      debug += "CHARACTER DATA\n";

      debug += "Relationship count = "
        + character.relationships.Count + "\n";

      debug += "AFFINITIES\n";
      debug += "Trust/Positive:\n";
      foreach (State state in character.trustAffinities.GetPositive())
        debug += "  " + state.name + "\n";
      debug += "Trust/Negative:\n";
      foreach (State state in character.trustAffinities.GetNegative())
        debug += "  " + state.name + "\n";
      debug += "Agreement/Positive:\n";
      foreach (State state in character.agreementAffinities.GetPositive())
        debug += "  " + state.name + "\n";
      debug += "Agreement/Negative:\n";
      foreach (State state in character.agreementAffinities.GetNegative())
        debug += "  " + state.name + "\n";

      Logger.Log(debug);
    };

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
      debug += "Trust " + t + ", Agreement " + a + "\n";
      debug += "Axes = " + relationship.RenderAxes() + "\n";

      Logger.Log(debug);
    };

#endif

    [Conditional("BVE_DEBUG")]
    public static void Attach(Character character) {

#if BVE_DEBUG

      character.OnPoll += LogCharacterData;
      character.OnUpdateRelationship += RenderRelationship;

#endif

    }

    [Conditional("BVE_DEBUG")]
    public static void Detach(Character character) {

#if BVE_DEBUG

      character.OnPoll -= LogCharacterData;
      character.OnUpdateRelationship -= RenderRelationship;

#endif

    }
  }
}
