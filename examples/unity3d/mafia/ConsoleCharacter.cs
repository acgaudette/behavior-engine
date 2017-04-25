// ConsoleCharacter.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public abstract class ConsoleCharacter : UnityEntity {

  public CharacterUnit<ConsoleAction> unit;

  public ConsoleCharacter() : base() {
    unit = new CharacterUnit<ConsoleAction>();
  }
}
