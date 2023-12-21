using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshSimpleCuller
		:MeshCullerBase
	{
		#region Fields

		public MeshSelectorBase selection;
		public bool inverse;
		[System.NonSerialized]public IList<int> triangles;

		#endregion Fields

		#region Methods

		public override void BeginCullMesh(Mesh mesh) {
			triangles=GetTriangles(mesh);
			if(selection!=null) {triangles=selection.SelectVertices(mesh,triangles);}
		}

		public override void EndCullMesh(Mesh mesh) {
			triangles=null;
		}

		public override bool OnCullMesh(int a,int b,int c) {
			if(triangles.IndexOf(a)>=0
			 &&triangles.IndexOf(b)>=0
			 &&triangles.IndexOf(c)>=0
			) {
				return !inverse;
			}
			return inverse;
		}

		#endregion Methods
	}
}
