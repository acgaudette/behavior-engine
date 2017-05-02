// Trigger.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;

public class Trigger : MonoBehaviour {

  public virtual void Initialize() { }

  public virtual bool Ready {
    get { return true; } // Triggers constantly by default
  }
}
