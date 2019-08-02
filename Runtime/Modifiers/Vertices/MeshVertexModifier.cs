using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshVertexModifier:MeshModifierBase {

		#region Fields

		public Transform reference;

		#endregion Fields

		#region Methods

		protected virtual Vector3 ModifyVertex(Vector3 vertex) {
			return (reference!=null)?reference.TransformPoint(vertex):vertex;
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				Vector3[] vertices=mesh.vertices;
				for(int i=0,imax=vertices?.Length??0;i<imax;++i) {
					vertices[i]=ModifyVertex(vertices[i]);
				}
				mesh.vertices=vertices;
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods

	}
}
