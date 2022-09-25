using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class SubMeshModifier
		:MeshModifierBase
	{
		#region Fields
		#endregion Fields

		#region Methods

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				mesh.triangles=mesh.GetTriangles(submesh);
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
