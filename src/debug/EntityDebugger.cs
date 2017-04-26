// EntityDebugger.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Diagnostics;
using System.Collections.Generic;

namespace BehaviorEngine.Debug {

  public static class EntityDebugger {

#if BEHAVIORENGINE_DEBUG

    static EntityEvents.OnReactEventHandler LogReaction = (
      object sender,
      Interaction interaction, IEntity host, IList<Effect> effects
    ) => {
      IEntity entity = sender as IEntity;

      string debug = entity.GetDebugLabel() + "\n";
      debug += "REACTION\n";

      debug += "Interaction = " + interaction.GetDebugLabel() + "\n";
      debug += "Host = " + host.GetDebugLabel()
        + (host == entity ? " (self)" : "") + "\n";

      debug += Utility.CollectionToString(
        "Effects", effects, e => e.GetVerboseDebugLabel()
      );

      Logger.Log(debug);
    };

    static EntityEvents.OnObserveEventHandler LogObservation = (
      object sender,
      Interaction interaction, IEntity host,
      ICollection<IEntity> targets, IList<Effect> effects
    ) => {
      IEntity entity = sender as IEntity;

      string debug = entity.GetDebugLabel() + "\n";
      debug += "OBSERVATION\n";

      debug += "Interaction = " + interaction.GetDebugLabel() + "\n";
      debug += "Host = " + host.GetDebugLabel() + "\n";

      debug += Utility.CollectionToString(
        "Targets", targets, t => t.GetDebugLabel()
      );
      debug += Utility.CollectionToString(
        "Effects", effects, e => e.GetVerboseDebugLabel()
      );

      Logger.Log(debug);
    };

    static EntityEvents.OnPollEventHandler LogPoll = (
      object sender,
      Interaction choice, ICollection<IEntity> targets, float highscore
    ) => {
      IEntity entity = sender as IEntity;

      string debug = entity.GetDebugLabel() + "\n";
      debug += "POLL\n";

      debug += "Interaction = "
        + (choice == null ? "null" : choice.GetDebugLabel()) + "\n";

      debug += Utility.CollectionToString(
        "Targets", targets, t => t.GetDebugLabel()
      );

      debug += "Score = " + highscore + "\n";

      Logger.Log(debug);
    };

#endif

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void Attach(IEntity entity) {

#if BEHAVIORENGINE_DEBUG

      entity.OnReact += LogReaction;
      entity.OnObserve += LogObservation;
      entity.OnPoll += LogPoll;

#endif

    }

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void Detach(IEntity entity) {

#if BEHAVIORENGINE_DEBUG

      entity.OnReact -= LogReaction;
      entity.OnObserve -= LogObservation;
      entity.OnPoll -= LogPoll;

#endif

    }
  }
}
