// InputTrigger.cs
// Created by Aaron C Gaudette on 01.05.17

using UnityEngine;

public class InputTrigger : Trigger {

  public KeyCode key = KeyCode.Space;

  public override bool Ready {
    get { return Input.GetKeyUp(key); }
  }
}
