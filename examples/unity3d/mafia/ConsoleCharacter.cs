// ConsoleCharacter.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public abstract class ConsoleCharacter : UnityEntity {

  public CharacterUnit unit;

  public ConsoleCharacter(string label) : base(label) {
    unit = new CharacterUnit();
  }
}
