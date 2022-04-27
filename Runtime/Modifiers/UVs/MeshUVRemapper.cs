using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshUVRemapper
		:MeshModifierBase
	{
		#region Fields

		public Transform box;
		public int channel;

		#endregion Fields

		#region Methods
		

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null&&box!=null) {
				Vector3[] vertices=mesh.vertices;
				var uv=mesh.GetUVs(channel);
				var m=box.worldToLocalMatrix;
				for(int i=0,imax=vertices?.Length??0;i<imax;++i) {
					uv[i]=m.MultiplyPoint3x4(vertices[i]);
				}
				mesh.SetUVs(channel,uv);
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
