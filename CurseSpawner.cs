using UnityEngine;
using Landfall.TABS;
using Landfall.TABS.GameState;

namespace MapMayhem {
    public class CurseSpawner : GameStateListener {

        void OnTriggerEnter(Collider col) {
            if (!spawned && col.transform.root.GetComponent<Unit>()) {

                spawned = true;
                col.transform.root.GetComponent<Unit>().data.healthHandler.Die();
                Instantiate(curse, transform.position, transform.rotation);
            }
        }

        public override void OnEnterPlacementState()
        {
            spawned = true;
        }

        public override void OnEnterBattleState()
        {
            spawned = false;
        }

        private bool spawned;

        public GameObject curse;
    }
}
