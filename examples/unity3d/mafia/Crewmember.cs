// Crewmember.cs
// Created by Aaron C Gaudette on 24.04.17

using BehaviorEngine.Personality;

public partial class Crewmember : Character, IDestroyable {

  public const string M = "his";
  public const string F = "her";

  public string firstName;
  public string role;
  public string pronoun;

  public string Title {
    get { return firstName + " " + name + " (" + role + ")"; }
  }

  public Crewmember(
    string firstName, string lastName, string role, string pronoun
  ) : base(lastName) {
    this.firstName = firstName;
    this.role = role;
    this.pronoun = pronoun;

    HookRenderer();
  }

  public Crewmember ChooseVote() {
    Character chosen = null;
    float currVal = 100f; // Magic number

    foreach (Relationship r in relationships.Values) {
      // Skip dead targets
      if ((r.target as IDestroyable).Destroy)
        continue;

      if(chosen == null) {
        chosen = r.target;
      }

      float calculation = 0f;
      calculation += r.trustworthiness + r.agreeability;

      /**
       * Proposed calculation as an alternative to the addition: use 
       * trustworthiness and agreeability as the max of a random function 
       * to different values that will be summed together for the calcuation
       * value. This adds some randomness to the selection, but will cause some
       * bias for higher valued targets to not be picked, especially if you add
       * an offset for high enough values.
       * Example: trustworthiness is at .8, or above .5, so we do a random(.8)
       * and add an offset of .25 because we don't want this crewmember to be
       * picked that easily.
       */

      if(calculation < currVal) {
        currVal = calculation;
        chosen = r.target;
      }
    }

    return chosen as Crewmember;
  }

  public bool Destroy { get; set; }
}
