using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Landfall.TABS;
using UnityEngine.Events;
using Landfall.TABS.GameState;

namespace MapMayhem {

    public class CannonBehavior : MonoBehaviour {

        public void Start() { startForward = transform.parent.InverseTransformDirection(transform.forward); }

        public void Update() {

            if (ServiceLocator.GetService<GameStateManager>().GameState != GameState.BattleState) { return; }
            counter += Time.deltaTime;
            if (!target || Vector3.Distance(transform.position, target.data.mainRig.position) > targetRange || target.data.Dead) { SetTargeterer(); return; }
            var vector = new Vector3(transform.parent.TransformDirection(startForward).x, 0f, transform.parent.TransformDirection(startForward).z);
            var normalized = new Vector3((target.data.mainRig.position - transform.position).normalized.x, 0f, (target.data.mainRig.position - transform.position).normalized.z);
            float time = Vector3.Angle(normalized, vector);
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, Vector3.Lerp(vector, normalized, rotationCurve.Evaluate(time)), rotationSpeed * Time.deltaTime));
            if (counter >= cooldown) {

                counter = 0f;
                fireEvent.Invoke();
            }
        }

        public void SetTargeterer() {

            var hits = Physics.SphereCastAll(transform.position, targetRange, Vector3.up, 0.1f, LayerMask.GetMask(new string[] { "MainRig" }));
            List<Unit> foundUnits = new List<Unit>();
            foreach (var hit in hits) {
                if (hit.transform.root.GetComponent<Unit>() && !foundUnits.Contains(hit.transform.root.GetComponent<Unit>())) { foundUnits.Add(hit.rigidbody.transform.root.GetComponent<Unit>()); }
            }
            Unit[] query
            = (
              from Unit unit
              in foundUnits
              where !unit.data.Dead
              orderby (unit.data.mainRig.transform.position - transform.position).magnitude
              select unit
            ).ToArray();
            if (query.Length > 0) { target = query[0]; }
        }

        private Unit target;

        private float counter;

        public UnityEvent fireEvent = new UnityEvent();

        public float cooldown;

        public float targetRange = 10f;

        public AnimationCurve rotationCurve = new AnimationCurve();

        public float rotationSpeed = 25f;

        private Vector3 startForward;
    }
}
