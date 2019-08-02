using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshVertexRemapper:MeshVertexModifier {

		#region Fields

		public BoxCollider from;
		public BoxCollider to;

		#endregion Fields

		#region Methods

		protected override Vector3 ModifyVertex(Vector3 vertex) {
			vertex=base.ModifyVertex(vertex);
			//
			if(from==null||to==null) {
				return vertex;
			}
			//
			Vector3 backup=vertex;
			vertex=from.transform.InverseTransformPoint(vertex)-from.center;
			vertex=UnityExtension.Division(vertex,from.size);
			if(vertex.x>=-0.5f&&vertex.x<= 0.5f&&
				vertex.y>=-0.5f&&vertex.y<= 0.5f&&
				vertex.z>=-0.5f&&vertex.z<= 0.5f
			) {
				vertex=to.transform.TransformPoint(to.center+Vector3.Scale(vertex,to.size));
			}else {
				vertex=backup;
			}
			//
			return vertex;
		}

		#endregion Methods
	}
}
