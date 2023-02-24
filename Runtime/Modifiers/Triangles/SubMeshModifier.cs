using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class SubMeshModifier
		:MeshModifierBase
	{
		#region Fields

		public bool optimize=true;

		#endregion Fields

		#region Methods

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				mesh.triangles=mesh.GetTriangles(submesh);
				if(optimize) {mesh.Optimize();}
				//
				if(target!=null) {
					Renderer r=target.GetComponent<Renderer>();
					if(r!=null) {
						Material[] tmp=r.sharedMaterials;
						if(submesh<(tmp?.Length??0)) {tmp=new Material[]{tmp[submesh]};}
						r.sharedMaterials=tmp;
					}
				}
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
