using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class CircularCurve:MonoTransformCurve {
		#region Fields

		public enum ValueIndex {
			 RadiusX
			,RadiusY
			,Height
			,Twist
			,Scale
		}

		[Header("Time")]
		public float time=1.0f;
		public float deltaTime=0.1f;
		public float speed=360.0f;
		public bool isRotationLock=true;

		[Header("Value")]

		public float[] floats=new float[5]{
			 0.5f
			,0.5f
			,0.0f
			,0.0f
			,1.0f
		};
		public AnimationCurve[] curves=new AnimationCurve[5] {
			 AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
		};

		#endregion Fields

		#region Methods

		public virtual float EvaluateValue(ValueIndex idx,float t) {
			int i=(int)idx;
			t/=time;// Normalize time.
			return floats[i]*curves[i].Evaluate(t);
		}

		public override Vector3 Internal_GetPosition(float t) {
			float a=speed*t*Mathf.Deg2Rad;
			return new Vector3(
				 EvaluateValue(ValueIndex.RadiusX,t)*Mathf.Cos(a)
				,EvaluateValue(ValueIndex.RadiusY,t)*Mathf.Sin(a)
				,EvaluateValue(ValueIndex.Height,t)
			);
		}

		public override Quaternion Internal_GetRotation(float t) {
			Quaternion q;
			if(isRotationLock) {
				q=Quaternion.AngleAxis(speed*t,Vector3.forward);
			}else {
				q=Quaternion.LookRotation(
					Internal_GetPosition(t+deltaTime)-Internal_GetPosition(t)
				,Vector3.back);
			}
			return q*Quaternion.AngleAxis(EvaluateValue(ValueIndex.Twist,t),Vector3.forward);
		}

		public override Vector3 Internal_GetScale(float t) {
			return Vector3.one*EvaluateValue(ValueIndex.Scale,t);
		}

		#endregion Methods
	}
}