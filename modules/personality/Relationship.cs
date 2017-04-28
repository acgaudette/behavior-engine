namespace BehaviorEngine.Personality {

  public class Relationship {

    public Character target;
    public float agreeability;
    public float trustworthiness;

    public Relationship(Character p, float agree, float trust) {
      target = p;
      agreeability = agree;
      trustworthiness = trust;
    }
  }
}
