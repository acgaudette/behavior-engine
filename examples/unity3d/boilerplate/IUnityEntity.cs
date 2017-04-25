// IUnityEntity.cs
// Created by Aaron C Gaudette on 04.09.17

using System.Collections.Generic;
using UnityEngine;
using BehaviorEngine;
using BehaviorEngine.Float;

public interface IUnityEntity {

  bool Destroy {
    get; set;
  }

  bool Print {
    get; set;
  }

  // Helper function
  float GetAttributeState(IAttribute attribute);
}
