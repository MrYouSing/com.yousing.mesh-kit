using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class BlendShapeRemover
		:MeshModifierBase
	{
		#region Methods

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				mesh.ClearBlendShapes();
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
