using UnityEngine;
using BehaviorEngine;

public class Relationship {
  public Person target;
  public float agreeability;
  public float trustworthiness;

  public Relationship(Person p, float agree, float trust) {
    target = p;
    agreeability = agree;
    trustworthiness = trust;
  }
}
