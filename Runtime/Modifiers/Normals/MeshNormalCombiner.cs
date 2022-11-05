using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshNormalCombiner
		:MeshModifierBase
	{
		#region Fields

		public float threshold=0.001f;
		public Mesh[] meshes;

		[System.NonSerialized]protected List<Vector3> m_Vertices=new List<Vector3>();
		[System.NonSerialized]protected List<Vector3> m_Normals=new List<Vector3>();

		#endregion Fields

		#region Methods

		public virtual Vector3 Combine(Vector3 position,Vector3 normal) {
			Vector3 sum=normal;int cnt=1;float sqr=threshold*threshold;
			for(int i=0,imax=m_Vertices?.Count??0;i<imax;++i) {
				if((m_Vertices[i]-position).sqrMagnitude<=sqr) {
					sum+=m_Normals[i];++cnt;
				}
			}
			return cnt>1?(sum/cnt):normal;
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				//
				Mesh it;
				for(int i=0,imax=meshes?.Length??0;i<imax;++i) {
					it=meshes[i];
					if(it!=null) {
						m_Vertices.AddRange(it.vertices);m_Normals.AddRange(it.normals);
					}
				}
				//
				Vector3[] vertices=mesh.vertices;Vector3[] normals=mesh.normals;
				for(int i=0,imax=vertices?.Length??0;i<imax;++i) {
					normals[i]=Combine(vertices[i],normals[i]);
				}
				//
				mesh.normals=normals;
				m_Vertices.Clear();m_Normals.Clear();
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
