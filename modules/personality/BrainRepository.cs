using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class BrainRepository : IRepository {

    public HashSet<InfluencedEffect> effects =
      new HashSet<InfluencedEffect>();

    // Refactor
    public CharacterUnit<ICharacterAction> characterUnit =
      new CharacterUnit<ICharacterAction>();

    Dictionary<FactorEnum, Factor> factors =
      new Dictionary<FactorEnum, Factor>();

    // Helper method
    public void RegisterFactor(Factor f) {
      factors[f.factorType] = f;
    }

    public IEnumerable<IAttribute> AttributePrototypes {
      get {
        List<IAttribute> attributes = new List<IAttribute>();

        foreach (Factor factor in factors.Values)
          attributes.Add(factor as IAttribute);

        return attributes;
      }
    }

    public ICollection<Interaction> Interactions {
      get; set;
    }
  }
}
