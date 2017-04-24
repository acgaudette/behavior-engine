// CharacterUnit.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class CharacterUnit<T> where T:ICharacterAction {

    public HashSet<T> actions;

    public CharacterUnit() {
      actions = new HashSet<T>();
    }

    public bool Perform(T a) {
      if (!actions.Contains(a))
        return false;

      a.Perform();

      return true;
    }
  }
}
