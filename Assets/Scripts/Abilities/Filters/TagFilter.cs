using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu (fileName = "Tag Filter", menuName = "RPG/Abilities/Filters/Tag", order = 0)]
    public class TagFilter : FilterStrategy
    {

        [SerializeField] TagType[] tagsToFilter;

        public override IEnumerable<GameObject> Filter (IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var tagToFilter in tagsToFilter)
            {
                foreach (var filterObject in objectsToFilter)
                {
                    if (filterObject.CompareTag (tagToFilter.ToString ()))
                    {
                        yield return filterObject;
                    }
                }
            }
        }
    }
}