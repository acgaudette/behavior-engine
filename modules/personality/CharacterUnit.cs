// CharacterUnit.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public interface ICharacterAction {
    void Perform();
  }

  public class CharacterUnit<A> where A : ICharacterAction {

    public Dictionary<string, A> actions;

    public CharacterUnit() {
      actions = new Dictionary<string, A>();
    }

    public bool Perform(string key) {
      if (!actions.ContainsKey(key))
        return false;

      actions[key].Perform();

      return true;
    }
  }
}
