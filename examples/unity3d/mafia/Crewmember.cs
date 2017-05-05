// Crewmember.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public class Crewmember : Character, IDestroyable {

  public const string M = "his";
  public const string F = "her";

  public string role;
  public string pronoun;

  public string Title {
    get { return name + " the " + role; }
  }

  public Crewmember(string name, string role, string pronoun) : base(name) {
    this.role = role;
    this.pronoun = pronoun;
  }

  public bool Destroy { get; set; }
}
