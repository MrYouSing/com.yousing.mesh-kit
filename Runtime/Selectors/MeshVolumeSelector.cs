using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshVolumeSelector
		:MeshSelectorBase
	{
		#region Fields

		public new Collider collider;
		public bool inverse;
		[System.NonSerialized]public Vector3[] vertices;

		#endregion Fields

		#region Methods

		public override void BeginSelect(Mesh mesh) {
			if(mesh!=null) {
				vertices=mesh.vertices;
			}
		}

		public override void EndSelect(Mesh mesh) {
			vertices=null;
		}

		public override bool TestIndex(int index) {
			bool b=false;
			if(vertices!=null) {b=collider.OverlapPoint(vertices[index]);}
			return inverse?!b:b;
		}

		#endregion Methods
	}
}
