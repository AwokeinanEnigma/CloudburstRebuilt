using UnityEngine;
using ThreeEyedGames;

namespace RoR2
{
	public class AnimateShaderAlpha : MonoBehaviour
	{
		public AnimationCurve alphaCurve;
		public float timeMax;
		public Decal decal;
		public bool destroyOnEnd;
		public bool disableOnEnd;
		public float time;
	}
}
