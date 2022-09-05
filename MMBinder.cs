using System.Collections;
using UnityEngine;

namespace MapMayhem
{
    public class MMBinder : MonoBehaviour
    {

        public static void UnitGlad()
        {
            if (!instance)
            {
                instance = new GameObject
                {
                    hideFlags = HideFlags.HideAndDontSave
                }.AddComponent<MMBinder>();
            }
            instance.StartCoroutine(StartUnitgradLate());
        }

        private static IEnumerator StartUnitgradLate()
        {
            yield return new WaitUntil(() => FindObjectOfType<ServiceLocator>() != null);
            yield return new WaitUntil(() => ServiceLocator.GetService<ISaveLoaderService>() != null);
            new MMMain();
            yield break;
        }

        private static MMBinder instance;
    }
}