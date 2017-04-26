// BrainRepository.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class BrainRepository : IRepository {

    public HashSet<InfluencedEffect> effects;
    public Dictionary<string, ICharacterAction> actions;

    Dictionary<TraitType, Trait> traits;

    public void RegisterTrait(Trait p) {
      traits[p.type] = p;
    }

    public IEnumerable<IAttribute> AttributePrototypes {
      get {
        List<IAttribute> attributes = new List<IAttribute>();

        // Not ideal
        foreach (Trait trait in traits.Values)
          attributes.Add(trait as IAttribute);

        return attributes;
      }
    }

    public IEnumerable<Interaction> Interactions {
      get; set;
    }

    public BrainRepository() {
      effects = new HashSet<InfluencedEffect>();
      actions = new Dictionary<string, ICharacterAction>();
      traits = new Dictionary<TraitType, Trait>();
      Interactions = new HashSet<Interaction>();
    }
  }
}
