using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu (menuName = ("RPG/Inventory/Drop Library"))]
    public class DropLibrary : ScriptableObject
    {

        [SerializeField]
        DropConfig[] potentialDrops;

        [Tooltip("Each element corresponds to the drop chance relative to enemy Level")]
        [Range(1, 100)][SerializeField] float[] anyDropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;
            public int GetRandomNumber (int level) //Returns a random int between min and max drops
            {
                if (!item.IsStackable ())
                {
                    return 1;
                }
                int min = GetByLevel (minNumber, level);
                int max = GetByLevel (maxNumber, level);
                return UnityEngine.Random.Range (min, max + 1);
            }
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops (int level) //Random Dropper calls this function to know what to drop
        {
            if (!ShouldRandomDrop (level)) // Nothing Drops if ShouldRandomDrop returns false
            {
                yield break;
            }
            for (int i = 0; i < GetRandomNumberOfDrops (level); i++)
            {
                yield return GetRandomDrop (level);
            }
        }

        bool ShouldRandomDrop (int level) // returns true if the random number is less than the dropChance roll from GetByLevel
        {
            return Random.Range (0, 100) < GetByLevel (anyDropChancePercentage, level);
        }

        int GetRandomNumberOfDrops (int level) // returns an appropriate number of drops for the level
        {
            int min = GetByLevel (minDrops, level);
            int max = GetByLevel (maxDrops, level);
            return Random.Range (min, max);

        }

        Dropped GetRandomDrop (int level)
        {
            var drop = SelectRandomItem (level);
            var result = new Dropped ();
            result.item = drop.item;
            result.number = drop.GetRandomNumber (level);
            return result;

        }

        DropConfig SelectRandomItem (int level) //chooses the (first or all?) random item that fits the criteria
        {
            float totalChance = GetTotalChance (level);
            float randomRoll = Random.Range (0, totalChance);
            float chanceTotal = 0; 
            foreach (var drop in potentialDrops) 
            {
                chanceTotal += GetByLevel (drop.relativeChance, level);
                if (chanceTotal > randomRoll) // if roll is Lower that chanceTotal, the drop occurs
                {
                    return drop;
                }
            }
            return null;
        }

        float GetTotalChance (int level) //Return a Random float for SelectRandomItem
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel (drop.relativeChance, level);
            }
            return total; // Sum of all the drops relative chances for the player's level
        }

        static T GetByLevel<T> (T[] values, int level) // returns random from provided array if the player level is greater than the array length, else returns the value that coincides with the level
        {
            if (values.Length == 0)
            {
                return default;
            }
            if (level > values.Length)
            {
                return values[values.Length - 1];
            }
            if (level <= 0)
            {
                return default;
            }
            return values[level - 1];
        }
    }
}