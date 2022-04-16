using UnityEngine;

namespace RoR2
{
	public class EffectComponent : MonoBehaviour
	{
		public EffectIndex effectIndex;
		public bool positionAtReferencedTransform;
		public bool parentToReferencedTransform;
		public bool applyScale;
		public string soundName;
		public bool disregardZScale;
	}
}
