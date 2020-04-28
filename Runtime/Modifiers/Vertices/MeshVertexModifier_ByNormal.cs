using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract partial class MeshVertexModifier_ByNormal
		:MeshModifierBase
	{
		#region Fields

		[System.NonSerialized]protected Vector3[] m_Vertices;
		[System.NonSerialized]protected Vector3[] m_Normals;

		#endregion Fields

		#region Methods

		protected abstract void ModifyVertex(int i);

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				m_Vertices=mesh.vertices;
				m_Normals=mesh.normals;
				//
				ForEach(GetTriangles(mesh),ModifyVertex);
				//
				mesh.vertices=m_Vertices;m_Vertices=null;
				m_Normals=null;
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}