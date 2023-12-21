/* <!-- Macro.Table Shapes
Box,boxes,
Capsule,capsules,
Sphere,spheres,
 Macro.End --> */

/* <!-- Macro.Call  Shapes
		public Transform[] {1}=new Transform[0];
 Macro.End --> */
/* <!-- Macro.Patch
,Fields
 Macro.End --> */
/* <!-- Macro.Call  Shapes
				if({1}!=null) {{list.AddRange(System.Array.ConvertAll({1},GetGameObject));}}
 Macro.End --> */
/* <!-- Macro.Patch
,Select
 Macro.End --> */
/* <!-- Macro.Call  Shapes
			if(ForEach({1},p0,p1,p2,{0}Test)) {{return value;}}
 Macro.End --> */
/* <!-- Macro.Patch
,Test
 Macro.End --> */
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshVolumeCuller
		:MeshCullerBase_ByVertex
	{
		#region Fields

		public bool value=true;
// <!-- Macro.Patch Fields
		public Transform[] boxes=new Transform[0];
		public Transform[] capsules=new Transform[0];
		public Transform[] spheres=new Transform[0];
// Macro.Patch -->

		#endregion Fields

		#region Methods

#if UNITY_EDITOR
		[ContextMenu("Select")]
		protected virtual void Select() {
			using(UnityEngine.Pool.ListPool<GameObject>.Get(out var list)) {
// <!-- Macro.Patch Select
				if(boxes!=null) {list.AddRange(System.Array.ConvertAll(boxes,GetGameObject));}
				if(capsules!=null) {list.AddRange(System.Array.ConvertAll(capsules,GetGameObject));}
				if(spheres!=null) {list.AddRange(System.Array.ConvertAll(spheres,GetGameObject));}
// Macro.Patch -->
				UnityEditor.Selection.objects=list.ToArray();
			}
		}
#endif

		public static bool ForEach(Transform[] transforms,Vector3 p0,Vector3 p1,Vector3 p2,System.Func<Transform,Vector3,bool> func) {
			if(func!=null) {
				Transform t;
				for(int i=0,imax=transforms?.Length??0;i<imax;++i) {
					t=transforms[i];if(t==null) {continue;}
					if(func(t,p0)||func(t,p1)||func(t,p2)) {return true;}
				}
			}
			return false;
		}

		public static GameObject GetGameObject(Transform t) {
			return t!=null?t.gameObject:null;
		}

		public override bool CullTest(Vector3 p0,Vector3 p1,Vector3 p2) {
			Transform t;
// <!-- Macro.Patch Test
			if(ForEach(boxes,p0,p1,p2,BoxTest)) {return value;}
			if(ForEach(capsules,p0,p1,p2,CapsuleTest)) {return value;}
			if(ForEach(spheres,p0,p1,p2,SphereTest)) {return value;}
// Macro.Patch -->
			return !value;
		}

		public virtual bool BoxTest(Transform t,Vector3 p) {
			p=t.InverseTransformPoint(p);
			return p.x>=-0.5f
				&& p.x<= 0.5f
				&& p.y>=-0.5f
				&& p.y<= 0.5f
				&& p.z>=-0.5f
				&& p.z<= 0.5f
			;
		}

		public virtual bool CapsuleTest(Transform t,Vector3 p) {
			return false;
		}

		public virtual bool SphereTest(Transform t,Vector3 p) {
			p=t.InverseTransformPoint(p);
			return p.sqrMagnitude<=0.25f;
		}

		#endregion Methods
	}
}
