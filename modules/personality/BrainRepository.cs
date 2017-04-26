// BrainRepository.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class BrainRepository : IRepository {

    public HashSet<InfluencedEffect> effects;
    public Dictionary<string, ICharacterAction> actions;

    Dictionary<FactorEnum, Factor> factors;

    public void RegisterFactor(Factor f) {
      factors[f.factorType] = f;
    }

    public IEnumerable<IAttribute> AttributePrototypes {
      get {
        List<IAttribute> attributes = new List<IAttribute>();

        // Not ideal
        foreach (Factor factor in factors.Values)
          attributes.Add(factor as IAttribute);

        return attributes;
      }
    }

    public IEnumerable<Interaction> Interactions {
      get; set;
    }

    public BrainRepository() {
      effects = new HashSet<InfluencedEffect>();
      actions = new Dictionary<string, ICharacterAction>();
      factors = new Dictionary<FactorEnum, Factor>();
      Interactions = new HashSet<Interaction>();
    }
  }
}
