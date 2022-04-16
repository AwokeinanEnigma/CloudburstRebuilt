using UnityEngine;

namespace RoR2
{
	public class ObjectScaleCurve : MonoBehaviour
	{
		public bool useOverallCurveOnly;
		public AnimationCurve curveX;
		public AnimationCurve curveY;
		public AnimationCurve curveZ;
		public AnimationCurve overallCurve;
		public float timeMax;
	}
}
