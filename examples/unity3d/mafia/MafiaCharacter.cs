// MafiaCharacter.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public class MafiaCharacter : Character, IDestroyable {

  public MafiaCharacter(string name) : base(name) { }

  public bool Destroy { get; set; }
}
