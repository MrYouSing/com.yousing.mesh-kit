using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class AnimatedTransformCurve
		:MonoTransformCurve
	{
		#region Nested Types

		public enum ValueIndex {
			 None=-1
			,TX
			,TY
			,TZ
			,RX
			,RY
			,RZ
			,RW
			,SX
			,SY
			,SZ
			,Count
		}

		#endregion Nested Types

		#region Fields

		public bool isEulerR;
		public bool isUniformS;
		public float[] floats=new float[10]{
			 0.0f
			,0.0f
			,0.0f
			,0.0f
			,0.0f
			,0.0f
			,1.0f
			,1.0f
			,1.0f
			,1.0f
		};
		public AnimationCurve[] curves=new AnimationCurve[10] {
			 AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
			,AnimationCurve.Linear(0.0f,1.0f,1.0f,1.0f)
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
			return new Vector3(
				 EvaluateValue(ValueIndex.TX,t)
				,EvaluateValue(ValueIndex.TY,t)
				,EvaluateValue(ValueIndex.TZ,t)
			);
		}

		public override Quaternion Internal_GetRotation(float t) {
			if(isEulerR) {
				return Quaternion.Euler(new Vector3(
					 EvaluateValue(ValueIndex.RX,t)
					,EvaluateValue(ValueIndex.RY,t)
					,EvaluateValue(ValueIndex.RZ,t)
				));
			}else {
				return new Quaternion(
					 EvaluateValue(ValueIndex.RX,t)
					,EvaluateValue(ValueIndex.RY,t)
					,EvaluateValue(ValueIndex.RZ,t)
					,EvaluateValue(ValueIndex.RW,t)
				);
			}
		}

		public override Vector3 Internal_GetScale(float t) {
			if(isUniformS) {
				return Vector3.one*EvaluateValue(ValueIndex.SX,t);
			}else {
				return new Vector3(
					 EvaluateValue(ValueIndex.SX,t)
					,EvaluateValue(ValueIndex.SY,t)
					,EvaluateValue(ValueIndex.SZ,t)
				);
			}
		}

		#endregion Methods
	}
}
