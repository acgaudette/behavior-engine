// Character.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public class Character : Person, IDestroyable {

  public Character(string name) : base(name) { }

  public bool Destroy { get; set; }
}
