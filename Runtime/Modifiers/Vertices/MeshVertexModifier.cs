using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshVertexModifier:MeshModifierBase {

		#region Fields

		public bool recalculate=true;
		public Transform reference;
		public MeshSelectorBase selector;

		#endregion Fields

		#region Methods

		protected virtual Vector3 ModifyVertex(Vector3 vertex) {
			return (reference!=null)?reference.TransformPoint(vertex):vertex;
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				List<int> selection=null;
				if(selector!=null) {
					List<int> triangles=new List<int>(GetTriangles(mesh));
					selection=selector.SelectVertices(mesh,triangles);
				}
				Vector3[] vertices=mesh.vertices;
				for(int i=0,imax=vertices?.Length??0;i<imax;++i) {
					if(selection==null||selection.IndexOf(i)>=0) {
						vertices[i]=ModifyVertex(vertices[i]);
					}
				}
				mesh.vertices=vertices;
				if(recalculate) {
					mesh.RecalculateBounds();
					mesh.RecalculateNormals();
				}
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods

	}
}
