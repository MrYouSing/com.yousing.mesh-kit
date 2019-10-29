using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract class MonoTransformCurve
		:MonoBehaviour
		,ITransformCurve
	{
		#region Fields

		public Transform reference;
		public float time=1.0f;
		public float deltaTime=1.0f/60.0f;

		#endregion Fields

		#region Unity Messages

		protected virtual void Reset() {
		}

		#endregion Unity Messages

		#region Methods

		public abstract Vector3 Internal_GetPosition(float t);
		public abstract Quaternion Internal_GetRotation(float t);
		public abstract Vector3 Internal_GetScale(float t);

		public virtual Vector3 GetPosition(float t) {
			Vector3 v=Internal_GetPosition(t);
			return (reference!=null)?reference.TransformPoint(v):v;
		}

		public virtual Quaternion GetRotation(float t) {
			Quaternion q=Internal_GetRotation(t);
			return (reference!=null)?reference.rotation*q:q;
		}

		public virtual Vector3 GetEulerAngles(float t) {
			return GetRotation(t).eulerAngles;
		}

		public virtual Vector3 GetScale(float t) {
			Vector3 v=Internal_GetScale(t);
			return (reference!=null)?reference.TransformVector(v):v;
		}

		public virtual Matrix4x4 GetMatrix(float t) {
			return Matrix4x4.TRS(GetPosition(t),GetRotation(t),GetScale(t));
		}

		#endregion Methods

	}
}
