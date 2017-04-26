// IEntity.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  public partial interface IEntity {

    ICollection<Interaction> Interactions { get; set; }

    IAttributeInstance GetAttribute(IAttribute prototype);
    ICollection<IAttributeInstance> GetAttributes();

    bool AddAttribute(IAttribute prototype);
    bool RemoveAttribute(IAttribute prototype);

    void Poll();

    void React(Interaction interaction, IEntity host);
    void Observe(
      Interaction interaction, IEntity host, ICollection<IEntity> targets
    );

    event EntityEvents.OnReactEventHandler OnReact;
    event EntityEvents.OnObserveEventHandler OnObserve;
    event EntityEvents.OnPollEventHandler OnPoll;
  }

  public static class EntityEvents {

    public delegate void OnReactEventHandler(
      object sender,
      Interaction interaction, IEntity host, IList<Effect> effects
    );

    public delegate void OnObserveEventHandler(
      object sender,
      Interaction interaction, IEntity host,
      ICollection<IEntity> targets, IList<Effect> effects
    );

    public delegate void OnPollEventHandler(
      object sender,
      Interaction choice, ICollection<IEntity> targets, float highscore
    );
  }
}
