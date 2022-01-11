using System;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Saving;
using GameDevTV.Utils;
using UnityEngine;


namespace RPG.Stats
{
    public class AttributeStore : MonoBehaviour, IModifierProvider, ISaveable, IPredicateEvaluator
    {

        [SerializeField] AttributeBonus[] bonusConfig;
        [System.Serializable]
        class AttributeBonus
        {
            public Attribute attribute;
            public Stat stat;
            public float additiveBonusPerPoint = 0;
            public float percentageBonusPerPoint = 0;
        }

        Dictionary<Attribute, int> stagedPoints = new Dictionary<Attribute, int>();   //Attribute Points that are proposed but not permenantly commited to the character
        Dictionary<Attribute, int> assignedPoints = new Dictionary<Attribute, int>(); // points that have been commited to the character

        Dictionary<Stat, Dictionary<Attribute, float>> additiveBonusCache;
        Dictionary<Stat, Dictionary<Attribute, float>> percentageBonusCache;
        GameObject player;
        BaseStats baseStats;


        bool hasCommited = false;

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            baseStats = player.GetComponent<BaseStats> ();
            additiveBonusCache = new Dictionary<Stat, Dictionary<Attribute, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Attribute, float>>();            

            BuildBonusaches();
        }

        

        public int GetProposedPoints(Attribute attribute)
        {
            return GetPoints(attribute) + GetStagedPoints(attribute);
        }

        public int GetPoints(Attribute attribue)
        {
            // if the dictionary contains the attribute, then return the (permenant)assigned points for the attribute. else return 0.
            return assignedPoints.ContainsKey(attribue) ? assignedPoints[attribue] : 0;
        }

        public int GetStagedPoints(Attribute attribue)
        {
            // if the dictionary contains the attribute, then return the (Staged)assigned points for the attribute. else return 0.
            return stagedPoints.ContainsKey(attribue) ? stagedPoints[attribue] : 0;
        }

        public void AssignPoints(Attribute attribute, int points)
        {
            if (!CanAssignPoints(attribute, points)) return;

            stagedPoints[attribute] = GetStagedPoints(attribute) + points;            
        }

        public bool CanAssignPoints(Attribute attribute, int points)
        {
            if (GetStagedPoints(attribute) + points < 0) return false;
            if (GetUnassignedPoints() < points && points > 0 ) return false;
            //if (hasCommited && unassignedPoints <= 0) return false;
            return true;
        }

        public int GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints(); 
        }

        public int GetTotalProposedPoints()
        {
            int total = 0;
            foreach (int points in assignedPoints.Values)
            {
                total += points;
            }
            foreach (int points in stagedPoints.Values)
            {
                total += points;
            }
            return total;
        }

        public bool GetHasCommited()
        {
            return hasCommited;
        }

        public void Commit()
        {
            Debug.Log("Commit was initiated"); // Set a notification Sound here
            foreach (Attribute attribute in stagedPoints.Keys)
            {
                assignedPoints[attribute] = GetProposedPoints(attribute);                
            }

            stagedPoints.Clear();
            
            hasCommited = true;
        }

        public int GetAssignablePoints()
        {
            return (int)baseStats.GetStat(Stat.TotatTraitPoints);
        }

        private void BuildBonusaches()
        {
            
            foreach (AttributeBonus bonus in bonusConfig)
            {
                if (!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Attribute, float>();
                }

                if (!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Attribute, float>();
                }

                additiveBonusCache[bonus.stat][bonus.attribute] = bonus.additiveBonusPerPoint;

                percentageBonusCache[bonus.stat][bonus.attribute] = bonus.percentageBonusPerPoint;                
            }
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (!additiveBonusCache.ContainsKey(stat)) yield break;

            foreach (Attribute attribute in additiveBonusCache[stat].Keys)
            {
                float bonus = additiveBonusCache[stat][attribute];
                yield return bonus * GetPoints(attribute);
            }
            
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!percentageBonusCache.ContainsKey(stat)) yield break;

            foreach (Attribute attribute in percentageBonusCache[stat].Keys)
            {
                float bonus = percentageBonusCache[stat][attribute];
                yield return bonus * GetPoints(attribute);
            }
        }
        public bool? Evaluate(Predicates predicate, string[] parameters, RequiredAttribute[] attributes)
        {
            if (predicate == Predicates.MinimumAttribute) //Checks to see if the player meets the required attributes for the equipment
            {
                if (attributes.Length == 0) return null;
                {
                    Attribute check;
                    
                    for (var i = 0; i < attributes.Length; i++)
                    {
                        check = attributes[i].GetAttribute();
                        
                        int requiredValue = attributes[i].GetRequiredValue();
                        if(GetPoints(check) < requiredValue) return false;
                        if(GetPoints(check) >= requiredValue) return true;
                    }            
                }                
            }
            return null;
        }

        public RPG.Stats.Attribute[] GetRequiredAttributes()
            {
                return null;
            }

        public float GetRequiredValue()
        {
            return 0f;
        }

        public object CaptureState()
        {
            return assignedPoints;
        }

        public void RestoreState(object state)
        {
            assignedPoints = new Dictionary<Attribute, int>((IDictionary<Attribute, int>)state);
        }

        
    }
}