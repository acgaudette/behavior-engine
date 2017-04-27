// BrainRepository.cs

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class BrainRepository : IRepository {

    /* Traits */

    Dictionary<Factor, Trait> traits;

    public Trait GetTrait(Factor factor) {
      return traits.ContainsKey(factor) ? traits[factor] : null;
    }

    public void RegisterTrait(Trait p) {
      traits[p.type] = p;
    }

    /* State */

    Dictionary<string, State> states;

    public State GetState(string name) {
      return states.ContainsKey(name) ? states[name] : null;
    }

    public void RegisterState(State s) {
      states[s.name] = s;
    }

    public IEnumerable<IAttribute> AttributePrototypes {
      get {
        List<IAttribute> attributes = new List<IAttribute>();

        foreach (Trait trait in traits.Values)
          attributes.Add(trait as IAttribute);
        foreach (State state in states.Values)
          attributes.Add(state as IAttribute);

        return attributes;
      }
    }

    public IEnumerable<Interaction> Interactions {
      get; set;
    }

    /* Actions */

    Dictionary<string, ICharacterAction> actions;

    public ICharacterAction GetAction(string id) {
      return actions.ContainsKey(id) ? actions[id] : null;
    }

    public void RegisterAction(ICharacterAction a) {
      actions[a.ID] = a;
    }

    public ICollection<string> GetActionIDs() {
      return actions.Keys;
    }

    public ICollection<InfluencedEffect> Effects { get; set; }

    public BrainRepository() {
      Effects = new HashSet<InfluencedEffect>();

      actions = new Dictionary<string, ICharacterAction>();

      traits = new Dictionary<Factor, Trait>();
      states = new Dictionary<string, State>();

      Interactions = new HashSet<Interaction>();
    }
  }
}
