using UnityEngine;

namespace YouSingStudio.MeshKit {
	public static partial class ColliderHelper
	{
		public static bool OverlapPoint(this BoxCollider thiz,Vector3 point) {
			if(thiz!=null) {
				point=thiz.transform.InverseTransformPoint(point);
				point-=thiz.center;
				Vector3 s=thiz.size*0.5f;
				//
				point=Vector3.Scale(point,point);
				s=Vector3.Scale(s,s);
				return point.x<=s.x
					&&point.y<=s.y
					&&point.z<=s.z
				;
			}
			return false;
		}

		public static bool OverlapPoint(this SphereCollider thiz,Vector3 point) {
			if(thiz!=null) {
				point=thiz.transform.InverseTransformPoint(point);
				point-=thiz.center;
				float r=thiz.radius;
				//
				return point.sqrMagnitude<=r*r;
			}
			return false;
		}


		public static bool OverlapPoint(this CapsuleCollider thiz,Vector3 point) {
			return false;
		}


		public static bool OverlapPoint(this MeshCollider thiz,Vector3 point) {
			return false;
		}

		public static bool OverlapPoint(this Collider thiz,Vector3 point) {
			if(thiz!=null) {
				//
				var bc=thiz as BoxCollider;
				if(bc!=null) return bc.OverlapPoint(point);
				//
				var sc=thiz as SphereCollider;
				if(sc!=null) return sc.OverlapPoint(point);
				//
				var cc=thiz as CapsuleCollider;
				if(cc!=null) return cc.OverlapPoint(point);
				//
				var mc=thiz as MeshCollider;
				if(mc!=null) return mc.OverlapPoint(point);
			}
			return false;
		}
	}
}
