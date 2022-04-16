using UnityEngine;
using UnityEngine.Events;

namespace RoR2
{
	public class Tracer : MonoBehaviour
	{
		public Transform startTransform;
		public GameObject beamObject;
		public float beamDensity;
		public float speed;
		public Transform headTransform;
		public Transform tailTransform;
		public float length;
		public bool reverse;
		public UnityEvent onTailReachedDestination;
	}
}
