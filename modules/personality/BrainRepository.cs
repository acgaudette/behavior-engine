// BrainRepository.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class BrainRepository : IRepository {

    public HashSet<InfluencedEffect> effects;
    public Dictionary<string, ICharacterAction> actions;

    Dictionary<PropertyType, Property> properties;

    public void RegisterProperty(Property p) {
      properties[p.type] = p;
    }

    public IEnumerable<IAttribute> AttributePrototypes {
      get {
        List<IAttribute> attributes = new List<IAttribute>();

        // Not ideal
        foreach (Property property in properties.Values)
          attributes.Add(property as IAttribute);

        return attributes;
      }
    }

    public IEnumerable<Interaction> Interactions {
      get; set;
    }

    public BrainRepository() {
      effects = new HashSet<InfluencedEffect>();
      actions = new Dictionary<string, ICharacterAction>();
      properties = new Dictionary<PropertyType, Property>();
      Interactions = new HashSet<Interaction>();
    }
  }
}
