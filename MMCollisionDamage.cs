using System;
using Landfall.TABS;
using UnityEngine;

namespace MapMayhem
{
	public class CollisionDamage : MonoBehaviour
	{
		public void OnCollisionEnter(Collision col)
		{
			if (!col.collider || !col.collider.attachedRigidbody || !col.collider.attachedRigidbody.transform.root.GetComponent<Unit>() || onCooldown)
			{
				return;
			}
			col.collider.attachedRigidbody.transform.root.GetComponent<Unit>().data.healthHandler.TakeDamage(damage, Vector3.zero, null, DamageType.Default);
			onCooldown = true;
		}

		public void Update()
		{
			if (onCooldown)
			{
				countering += Time.deltaTime;
			}
			if (countering >= cooldown)
			{
				onCooldown = false;
				countering = 0f;
			}
		}

		public bool onCooldown;

		public float cooldown = 0.1f;

		public float countering;

		public float damage = 200f;
	}
}
