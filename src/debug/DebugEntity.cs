// DebugEntity.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;
using System.Diagnostics;

namespace BehaviorEngine {

  public abstract partial class Entity : Root {

    public override void Log() {
      string debug = GetLabel() + "\n";
      debug += "STATUS\n";

      debug += Utility.CollectionToString(
        "Attributes", GetAttributes(), (a) => {
          return a.GetVerboseLabel();
        }
      );

      debug += Utility.CollectionToString(
        "Interactions", interactions, i => i.GetLabel()
      );
    }

    [Conditional("LABELED")]
    public virtual void LogReaction(
      Interaction interaction, Entity host, IList<Effect> effects
    ) {
      string debug = GetLabel() + "\n";
      debug += "REACTION\n";

      debug += "Interaction = " + interaction.GetLabel() + "\n";
      debug += "Host = " + host.GetLabel()
        + (host == this ? " (self)" : "") + "\n";

      debug += Utility.CollectionToString(
        "Effects", effects, e => e.GetVerboseLabel()
      );
    }

    [Conditional("LABELED")]
    public virtual void LogObservation(
      Interaction interaction, Entity host,
      ICollection<Entity> targets, IList<Effect> effects
    ) {
      string debug = GetLabel() + "\n";

      debug += "OBSERVATION\n";
      debug += "Interaction = " + interaction.GetLabel() + "\n";
      debug += "Host = " + host.GetLabel() + "\n";

      debug += Utility.CollectionToString(
        "Targets", targets, t => t.GetLabel()
      );

      debug += Utility.CollectionToString(
        "Effects", effects, e => e.GetVerboseLabel()
      );

      Root.Log(debug);
    }

    [Conditional("LABELED")]
    public virtual void LogPoll(
      Interaction choice, ICollection<Entity> targets, float highscore
    ) {
      string debug = GetLabel() + "\n";
      debug += "POLL\n";
      debug += "Interaction = "
        + (choice == null ? "null" : choice.GetLabel()) + "\n";

      debug += Utility.CollectionToString(
        "Targets", targets, t => t.GetLabel()
      );

      debug += "Score = " + highscore + "\n";

      Root.Log(debug);
    }
  }
}
