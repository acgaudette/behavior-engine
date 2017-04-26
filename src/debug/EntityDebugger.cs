// EntityDebugger.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Diagnostics;
using System.Collections.Generic;

namespace BehaviorEngine.Debug {

  public static class EntityDebugger {

    [Conditional("BEHAVIORENGINE_DEBUG")]
    public static void Attach(IEntity entity) {

#if BEHAVIORENGINE_DEBUG

      entity.OnReact += (
        object sender,
        Interaction interaction, IEntity host, IList<Effect> effects
      ) => {
        string debug = entity.GetLabel() + "\n";
        debug += "REACTION\n";

        debug += "Interaction = " + interaction.GetLabel() + "\n";
        debug += "Host = " + host.GetLabel()
          + (host == entity ? " (self)" : "") + "\n";

        debug += Utility.CollectionToString(
          "Effects", effects, e => e.GetVerboseLabel()
        );

        Debugger.Log(debug);
      };

      entity.OnObserve += (
        object sender,
        Interaction interaction, IEntity host,
        ICollection<IEntity> targets, IList<Effect> effects
      ) => {
        string debug = entity.GetLabel() + "\n";
        debug += "OBSERVATION\n";

        debug += "Interaction = " + interaction.GetLabel() + "\n";
        debug += "Host = " + host.GetLabel() + "\n";

        debug += Utility.CollectionToString(
          "Targets", targets, t => t.GetLabel()
        );

        debug += Utility.CollectionToString(
          "Effects", effects, e => e.GetVerboseLabel()
        );

        Debugger.Log(debug);
      };

      entity.OnPoll += (
        object sender,
        Interaction choice, ICollection<IEntity> targets, float highscore
      ) => {
        string debug = entity.GetLabel() + "\n";
        debug += "POLL\n";

        debug += "Interaction = "
          + (choice == null ? "null" : choice.GetLabel()) + "\n";

        debug += Utility.CollectionToString(
          "Targets", targets, t => t.GetLabel()
        );

        debug += "Score = " + highscore + "\n";

        Debugger.Log(debug);
      };

#endif

    }
  }
}
