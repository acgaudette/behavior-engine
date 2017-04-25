// ConsoleCharacter.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public abstract class ConsoleCharacter : UnityEntity {

  public string Name {
    get; private set;
  }

  public CharacterUnit<ConsoleAction> unit;

  public ConsoleCharacter(string name) : base() {
    Name = name;
    unit = new CharacterUnit<ConsoleAction>();
  }
}
