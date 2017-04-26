// IRepository.cs
// Created by Aaron C Gaudette on 09.04.17

using System.Collections.Generic;

namespace BehaviorEngine {

  // Container for Attribute and Interaction prototypes (and more)
  public interface IRepository {

    IEnumerable<IAttribute> AttributePrototypes { get; }
    IEnumerable<Interaction> Interactions { get; }
  }
}
