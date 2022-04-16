using UnityEngine;

namespace ThreeEyedGames
{
	public class Decal : MonoBehaviour
	{
		public enum DecalRenderMode
		{
			Deferred = 0,
			Unlit = 1,
			Invalid = 2,
		}

		public DecalRenderMode RenderMode;
		public Material Material;
		public float Fade;
		public GameObject LimitTo;
		public bool DrawAlbedo;
		public bool UseLightProbes;
		public bool DrawNormalAndGloss;
		public bool HighQualityBlending;
	}
}
