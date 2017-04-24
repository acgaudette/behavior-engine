// CharacterUnit.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public class CharacterUnit {

    public interface IAction {
      void Perform();
    }

    public HashSet<IAction> actions;

    public CharacterUnit() {
      actions = new HashSet<IAction>();
    }

    public bool Perform(IAction a) {
      if (!actions.Contains(a))
        return false;

      a.Perform();

      return true;
    }
  }
}
