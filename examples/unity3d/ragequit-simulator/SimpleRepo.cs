// SimpleRepo.cs
// Created by Aaron C Gaudette on 25.04.17

using System.Collections.Generic;
using BehaviorEngine;

public class SimpleRepo : IRepository {

  public HashSet<Effect> effects;

  HashSet<IAttribute> attributes;
  HashSet<Interaction> interactions;

  public IEnumerable<IAttribute> AttributePrototypes {
    get { return attributes; }
  }

  public void RegisterAttribute(IAttribute prototype) {
    attributes.Add(prototype);
  }

  public ICollection<Interaction> Interactions {
    get { return interactions; }
  }

  public void RegisterInteraction(Interaction interaction) {
    interactions.Add(interaction);
  }

  public SimpleRepo() {
    effects = new HashSet<Effect>();
    attributes = new HashSet<IAttribute>();
    interactions = new HashSet<Interaction>();
  }
}
