using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshVolumeCuller
		:MeshCullerBase
	{
		#region Fields

		public bool value=true;
		public Transform[] boxes=new Transform[0];

		#endregion Fields

		#region Methods

		public override bool CullTest(Vector3 p0,Vector3 p1,Vector3 p2) {
			Transform t;
			for(int i=0,imax=boxes?.Length??0;i<imax;++i) {
				t=boxes[i];
				if(CullTest(t,p0)) return value;
				if(CullTest(t,p1)) return value;
				if(CullTest(t,p2)) return value;
			}
			return !value;
		}

		public virtual bool CullTest(Transform t,Vector3 p) {
			p=t.InverseTransformPoint(p);
			return p.x>=-0.5f
				&& p.x<= 0.5f
				&& p.y>=-0.5f
				&& p.y<= 0.5f
				&& p.z>=-0.5f
				&& p.z<= 0.5f
			;
		}

		#endregion Methods
	}
}
