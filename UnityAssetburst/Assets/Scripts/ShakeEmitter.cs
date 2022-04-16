using UnityEngine;

namespace RoR2
{
	public class ShakeEmitter : MonoBehaviour
	{
		public bool shakeOnStart;
		public bool shakeOnEnable;
		public Wave wave;
		public float duration;
		public float radius;
		public bool scaleShakeRadiusWithLocalScale;
		public bool amplitudeTimeDecay;
	}
}
