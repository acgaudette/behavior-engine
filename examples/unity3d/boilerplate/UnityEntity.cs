// UnityEntity.cs
// Created by Aaron C Gaudette on 04.09.17

using System.Collections.Generic;
using BehaviorEngine;

// Decorator
public class UnityEntity : BehaviorEngine.Debug.Labeled, IEntity {

  bool debug;
  IEntity entity;

  public UnityEntity(IEntity entity) {
    debug = false;
    this.entity = entity;

    entity.OnReact += OnReact;
    entity.OnObserve += OnObserve;
    entity.OnPoll += OnPoll;
  }

  public void StartDebug() {
    if (debug) return;
    BehaviorEngine.Debug.EntityDebugger.Attach(entity);
    debug = true;
  }

  public void StopDebug() {
    if (!debug) return;
    BehaviorEngine.Debug.EntityDebugger.Detach(entity);
    debug = false;
  }

  // Wrappers

  public ICollection<Interaction> Interactions {
    get { return entity.Interactions; }
    set { entity.Interactions = value; }
  }

  public IAttributeInstance GetAttribute(IAttribute prototype) {
    return entity.GetAttribute(prototype);
  }

  public ICollection<IAttributeInstance> GetAttributes() {
    return entity.GetAttributes();
  }

  public bool AddAttribute(IAttribute prototype) {
    return entity.AddAttribute(prototype);
  }

  public bool RemoveAttribute(IAttribute prototype) {
    return entity.RemoveAttribute(prototype);
  }

  public void Subscribe(IRepository repo) {
    entity.Subscribe(repo);
  }

  public void Poll() { entity.Poll(); }

  public void React(Interaction interaction, IEntity host) {
    entity.React(interaction, host);
  }

  public void Observe(
    Interaction interaction, IEntity host, ICollection<IEntity> targets
  ) {
    entity.Observe(interaction, host, targets);
  }

  public event EntityEvents.OnReactEventHandler OnReact;
  public event EntityEvents.OnObserveEventHandler OnObserve;
  public event EntityEvents.OnPollEventHandler OnPoll;
}
