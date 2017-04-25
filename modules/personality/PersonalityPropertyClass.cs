using System;
using System.Collections.Generic;
namespace BehaviorEngine.Personality {
  public class PersonalityPropertyClass : Class {
    public Dictionary<string, PersonalityProperty> personalityProperties;

    public PersonalityPropertyClass(Dictionary<string, 
        BehaviorEngine.Attribute<float>.InitializeState> delegates = null):
        base() {
      Random r = new Random();

      personalityProperties = new Dictionary<string, PersonalityProperty> ();

      foreach (string property in SharedData.PersonalityPropertyNames) {
        if (delegates == null ||
          !delegates.ContainsKey (property)) {
          float val = ((float)r.NextDouble ());
          personalityProperties[property] =
              new PersonalityProperty(property, () => val);
        } else {
          var del = delegates[property];
          personalityProperties[property] =
              new PersonalityProperty(property, del);
        }
        attributes.Add(personalityProperties[property]);
      }
    }
  }
}