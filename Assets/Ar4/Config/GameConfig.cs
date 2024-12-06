using System;
using Ar4.Entities.Abilities;
using UnityEngine;

namespace Ar4.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Ar4/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] public Ability[] abilities;
        [SerializeField] public InputAbility[] playerInputAbility;

        public Ability GetAbility(int id)
        {
            return Array.Find(abilities, ability => ability.id == id);
        }

        public void GenerateIds()
        {
            var id = 0;
            foreach (var ability in abilities)
                ability.id = ++id;
            Debug.Log("Generated ids");
        }
    }

    [Serializable]
    public class InputAbility
    {
        public string inputAction;
        public Ability ability;
    }
}