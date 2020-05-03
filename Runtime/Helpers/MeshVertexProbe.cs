using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshVertexProbe
		:MonoTask
	{
		#region Fields

		public Mesh mesh;
		public new Collider collider;

		#endregion Fields

		#region Methods

		public override void Run() {
			if(mesh!=null) {
				Vector3[] vertices=mesh.vertices;
				for(int i=0,imax=vertices?.Length??0;i<imax;++i) {
					if(collider.OverlapPoint(vertices[i])) {
						print(i);
					}
				}
			}
		}

		#endregion Methods
	}
}
