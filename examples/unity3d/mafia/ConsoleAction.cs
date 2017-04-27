// ConsoleAction.cs
// Created by Aaron C Gaudette on 24.04.17

using UnityEngine;
using BehaviorEngine.Personality;

public class ConsoleAction : ICharacterAction {

  public string ID { get; set; }
  string[] messages;

  public ConsoleAction(string id, string[] messages) {
    ID = id;
    this.messages = messages;
  }

  public virtual void Perform(CharacterActionInfo info) {
    int i = Random.Range(0, messages.Length);

    // Render
    Debug.Log(info.person.name + " " + messages[i] + "\n");
  }

  public override string ToString() {
    string s = ID + ": ";
    foreach (string message in messages)
      s += "\"" + message + "\" ";
    return s;
  }
}
