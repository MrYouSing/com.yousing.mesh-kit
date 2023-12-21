using UnityEngine;
using UnityEngine.Rendering;

namespace YouSingStudio.MeshKit {
	public class MeshFaceCuller
		:MeshCullerBase_ByVertex
	{
		#region Fields

		[Header("Culling")]
		public Transform viewReference;
		public Vector3 viewPoint=Vector3.zero;
		public CullMode cullMode=CullMode.Back;

		#endregion Fields

		#region Methods

		protected override Mesh BeginModifyMesh() {
			if(viewReference!=null) {
				viewPoint=transform.InverseTransformPoint(viewReference.position);
			}
			return base.BeginModifyMesh();
		}

		public override bool CullTest(Vector3 p0,Vector3 p1,Vector3 p2) {
			p1-=p0;p2-=p0;
			p0=viewPoint-p0;
			//
			float dot=Vector3.Dot(p0,Vector3.Cross(p1,p2));
			//
			switch(cullMode){
				case CullMode.Back:
				return dot<=0.0f;
				case CullMode.Front:
				return dot>=0.0f;
			}
			return true;
		}

		#endregion Methods
	}
}
