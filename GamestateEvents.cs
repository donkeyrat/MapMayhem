using UnityEngine;
using UnityEngine.Events;
using Landfall.TABS.GameState;

namespace MapMayhem
{
    public class GamestateEvents : GameStateListener
    {
        public override void OnEnterBattleState()
        {
            battleEvent.Invoke();
        }

        public override void OnEnterPlacementState()
        {
            placementEvent.Invoke();
        }

        public UnityEvent battleEvent;

        public UnityEvent placementEvent;
    }
}
