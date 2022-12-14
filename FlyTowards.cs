using UnityEngine;
using Landfall.TABS;
using System.Collections.Generic;
using System.Linq;

namespace MapMayhem {
    class FlyTowards : MonoBehaviour {
		void Start() {
			rig = GetComponent<Rigidbody>();
			SetTarget();
        }

		void Update() {
			if (target && !target.data.Dead) {
				rig.AddForce((target.data.mainRig.position - transform.position).normalized * forwardForce * Time.deltaTime);
			}
			else {
				SetTarget();
			}
		}

		void SetTarget() {
			var hits = Physics.SphereCastAll(transform.position, 80f, Vector3.up, 0.1f, LayerMask.GetMask(new string[] { "MainRig" }));
			List<Unit> foundUnits = new List<Unit>();
			foreach (var hit in hits)
			{
				if (hit.transform.root.GetComponent<Unit>() && !foundUnits.Contains(hit.transform.root.GetComponent<Unit>()))
				{
					foundUnits.Add(hit.rigidbody.transform.root.GetComponent<Unit>());
				}
			}
			Unit[] query
			= (
			  from Unit unit
			  in foundUnits
			  where !unit.data.Dead
			  orderby (unit.data.mainRig.transform.position - transform.position).magnitude
			  select unit
			).ToArray();
			if (query.Length != 0)
			{
				target = query[0];
			}
		}

		public float forwardForce = 20000f;

		private Unit target;

		private Rigidbody rig;
	}
}
