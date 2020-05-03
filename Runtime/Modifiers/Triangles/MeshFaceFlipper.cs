using UnityEngine;

namespace YouSingStudio.MeshKit {
	public partial class MeshFaceFlipper
		:MeshModifierBase
	{
		#region Fields

		[System.NonSerialized]protected Vector3[] m_Normals;

		#endregion Fields

		#region Methods

		protected virtual void ModifyNormal(int i) {
			m_Normals[i]=-m_Normals[i];
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				int[] triangles=GetTriangles(mesh);
				for(int i=0,imax=triangles?.Length??0,c;i<imax;i+=3) {
					c=triangles[i];
					triangles[i]=triangles[i+2];
					triangles[i+2]=c;
				}
				SetTriangles(mesh,triangles);
				//
				m_Normals=mesh.normals;
				ForEach(triangles,ModifyNormal);
				mesh.normals=m_Normals;m_Normals=null;
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}