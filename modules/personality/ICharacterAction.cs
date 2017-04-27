// ICharacterAction.cs
// Created by Aaron C Gaudette on 24.04.17

using System.Collections.Generic;

namespace BehaviorEngine.Personality {

  public interface ICharacterAction {
    string ID { get; set; }
    void Perform(CharacterActionInfo info);
  }

  public struct CharacterActionInfo {

    public Person person;
    public ICollection<IEntity> targets;

    public CharacterActionInfo(
      Person person, ICollection<IEntity> targets
    ) {
      this.person = person;
      this.targets = targets;
    }
  }
}
