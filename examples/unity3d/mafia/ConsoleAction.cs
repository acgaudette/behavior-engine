// ConsoleAction.cs
// Created by Aaron C Gaudette on 24.04.17

using UnityEngine;
using BehaviorEngine.Personality;

public struct ConsoleAction : ICharacterAction {

  string[] messages;

  public ConsoleAction(string[] messages) {
    this.messages = messages;
  }

  public void Perform() {
    int r = Random.Range(0, messages.Length);
    Debug.Log(messages[r]);
  }

  public override string ToString() {
    string s = "";
    foreach (string message in messages)
      s += "\"" + message + "\" ";
    return s;
  }
}
