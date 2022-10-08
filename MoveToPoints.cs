using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapMayhem
{
	public class MoveToPoints : MonoBehaviour
	{
		public void Awake()
        {
			for (int i = 0; i < transformPositions.Count; i++) {
				positions.Add(transformPositions[i].position);
			}

		}
		public void Init()
		{
			bool flag = !positions.Any<Vector3>();
			if (flag)
			{
				UnityEngine.Object.Destroy(this);
			}
			StartCoroutine(ChangePos());
		}

		private IEnumerator ChangePos()
		{
			for (; ; )
			{
				Vector3 prevPos = currentPos;
				currentIndex++;
				bool flag = currentIndex >= positions.Count;
				if (flag)
				{
					currentIndex = 0;
				}
				currentPos = positions[currentIndex];
				float distance = Vector3.Distance(prevPos, currentPos);
				StartCoroutine(MoveToPos(currentPos, distance / speed));
				yield return new WaitForSeconds(delay + distance / speed);
				prevPos = default(Vector3);
			}
			yield break;
		}

		private IEnumerator MoveToPos(Vector3 pos, float sec)
		{
			float i = 0f;
			float updateCounter = 0f;
			float updateLimit = Mathf.Clamp(0.3f / speed, 0.05f, float.PositiveInfinity);
			Vector3 startPos = transform.position;
			while (i < sec)
			{
				i += Time.deltaTime;
				updateCounter += Time.deltaTime;
				Vector3 nextPos = Vector3.Lerp(startPos, pos, i / sec);
				transform.position = nextPos;
				bool flag = updateCounter >= updateLimit && AstarPath.active;
				if (flag)
				{
					updateCounter = 0f;
					ReScan();
				}
				yield return null;
				nextPos = default(Vector3);
			}
			ReScan();
			yield break;
		}

		private void ReScan()
		{
			//AstarPath.active.UpdateGraphs(gameObject.GetComponent<Collider>().bounds);
		}

		private List<Vector3> positions = new List<Vector3>();

		public List<Transform> transformPositions = new List<Transform>();

		public float speed = 60f;

		public float delay = 0.05f;

		private int currentIndex;

		private Vector3 currentPos;
	}
}
