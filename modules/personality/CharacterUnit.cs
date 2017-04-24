// CharacterUnit.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class CharacterUnit<A> where A : CharacterUnit<A>.IAction {

    public interface IAction {
      void Perform();
    }

    public HashSet<A> actions;

    public CharacterUnit() {
      actions = new HashSet<A>();
    }

    public bool Perform(A a) {
      if (!actions.Contains(a))
        return false;

      a.Perform();

      return true;
    }
  }
}
