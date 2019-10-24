using UnityEngine;

namespace YouSingStudio.MeshKit {
	public interface ITransformCurve {
		Vector3 GetPosition(float t);
		Quaternion GetRotation(float t);
		Vector3 GetEulerAngles(float t);
		Vector3 GetScale(float t);
		Matrix4x4 GetMatrix(float t);
	}
}
