using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshNormalFlipper
		:MeshModifierBase
	{
		#region Fields

		public MeshSelectorBase selection;
		[System.NonSerialized]protected Vector3[] m_Normals;

		#endregion Fields

		#region Methods

		protected virtual void FlipNormal(int index) {
			m_Normals[index]=-m_Normals[index];
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				m_Normals=mesh.normals;
				IList<int> triangles=GetTriangles(mesh);
				if(selection!=null) {triangles=selection.SelectVertices(mesh,triangles);}
				ForEach(triangles,FlipNormal);
				mesh.normals=m_Normals;m_Normals=null;
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
