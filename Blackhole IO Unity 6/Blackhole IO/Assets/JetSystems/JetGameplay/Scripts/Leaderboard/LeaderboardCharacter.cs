using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JetSystems
{
    [RequireComponent(typeof(JetCharacter))]
    public class LeaderboardCharacter : MonoBehaviour, IComparable
    {
        private JetCharacter.CharacterType characterType;

        private void Start()
        {
            characterType = GetComponent<JetCharacter>().GetCharacterType();
        }

        public bool IsPlayer()
        {
            return characterType == JetCharacter.CharacterType.Player;
        }

        public int CompareTo(object obj)
        {
            LeaderboardCharacter otherPlayer = (LeaderboardCharacter)obj;

            switch(Leaderboard.instance.GetComparisonType())
            {
                case Leaderboard.ComparisonType.ZPosition:
                    return otherPlayer.transform.position.z.CompareTo(transform.position.z);

                case Leaderboard.ComparisonType.Size:
                    return otherPlayer.GetComponent<Player>().GetSize().CompareTo(GetComponent<Player>().GetSize());

                default:
                    return otherPlayer.transform.position.z.CompareTo(transform.position.z);
            }
        }
    }
}