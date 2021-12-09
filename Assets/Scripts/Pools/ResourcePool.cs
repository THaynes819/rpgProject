using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;

namespace RPG.Pools
{
    public class ResourcePool : MonoBehaviour, ISaveable
    {
        
        

        Stat currenMaxPool;
        PlayerClass playerClass;

        LazyValue<float> resourcePoints;
        LazyValue<float> regenRate;

        void Awake ()
        {
            resourcePoints = new LazyValue<float> (SetInitialMaxResource);
            regenRate = new LazyValue<float>(GetRegenRate);

        }
        private float SetInitialMaxResource () //This is for the LazyValue to initialize
        {
            return GetComponent<BaseStats> ().GetStat (currenMaxPool);
        }

        private float GetRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ResourceRegenRate);
        }

        void Start ()
        {
            InitialPool ();
            resourcePoints.ForceInit ();
        }

        public bool UseResource(float pointsChange)
        {
            if (pointsChange > resourcePoints.value)
            {
                return false;
            }

            resourcePoints.value -= pointsChange;
            return true;
        }

        public void AddResource(float pointsChange)
        {
            if (resourcePoints.value < GetCurrentMaxPool())
            {
                resourcePoints.value += pointsChange;
            }
            if ( resourcePoints.value >= GetCurrentMaxPool())
            {
                resourcePoints.value = GetCurrentMaxPool();
            }
        }

        
        private Stat InitialPool ()
        {
            playerClass = GetComponent<BaseStats>().GetPlayerClass();
            if (playerClass == PlayerClass.Fighter)
            {
                currenMaxPool = Stat.Rage;
                return currenMaxPool;
            }
            if (playerClass == PlayerClass.Caster)
            {
                currenMaxPool = Stat.Mana;
                return currenMaxPool;
            }
            if (playerClass == PlayerClass.Archer)
            {
                currenMaxPool = Stat.Fixation;
                return currenMaxPool;
            }
            currenMaxPool = Stat.None;

            return currenMaxPool;
        }
        public Stat GetCurrentPoolType ()
        {
            return InitialPool ();
        }

        public float GetCurrentResourcePoints ()
        {
            if ( resourcePoints.value >= GetCurrentMaxPool())
            {
                resourcePoints.value = GetCurrentMaxPool();
            }
            return resourcePoints.value;
        }

        public float GetCurrentMaxPool()
        {
            return GetComponent<BaseStats> ().GetStat (currenMaxPool);
        }

        public float GetPoolFraction ()
        {
            return GetCurrentResourcePoints() / GetCurrentMaxPool();
        }


        void Update ()
        {
            RegenerateResource ();
        }

        

        private void RegenerateResource ()
        {
            float maxPool = GetComponent<BaseStats> ().GetStat (currenMaxPool);
            if (resourcePoints.value < GetCurrentMaxPool())
            {
                resourcePoints.value += (GetRegenRate() * Time.deltaTime);
                if (resourcePoints.value > GetCurrentMaxPool())
                {
                    resourcePoints.value = GetCurrentMaxPool();
                }
            }
        }

        public object CaptureState()
        {
            return resourcePoints.value;
        }

        public void RestoreState(object state)
        {
            resourcePoints.value = (float) state;
        }
    }
}