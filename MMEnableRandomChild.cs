using UnityEngine;

namespace MapMayhem
{
    public class MMEnableRandomChild : MonoBehaviour
    {
        public void Start()
        {
            Randomize();
        }

        public void Randomize()
        {
            var chosen = transform.GetChild(Random.Range(0, transform.childCount - 1));
            chosen.gameObject.SetActive(true);
        }
    }
}
