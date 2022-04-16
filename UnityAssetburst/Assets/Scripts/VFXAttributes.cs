using UnityEngine;

namespace RoR2
{
	public class VFXAttributes : MonoBehaviour
	{
		public enum VFXPriority
		{
			Low = 0,
			Medium = 1,
			Always = 2,
		}

		public enum VFXIntensity
		{
			Low = 0,
			Medium = 1,
			High = 2,
		}

		public VFXPriority vfxPriority;
		public VFXIntensity vfxIntensity;
		public Light[] optionalLights;
		public ParticleSystem[] secondaryParticleSystem;
	}
}
