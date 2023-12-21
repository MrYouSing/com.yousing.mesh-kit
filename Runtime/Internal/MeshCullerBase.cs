using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract class MeshCullerBase:MeshModifierBase {
		#region Fields

		public bool optimize;

		#endregion Fields

		#region Methods

		public abstract void BeginCullMesh(Mesh mesh);
		public abstract void EndCullMesh(Mesh mesh);
		public abstract bool OnCullMesh(int t0,int t1,int t2);

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
			BeginCullMesh(mesh);
				int[] triangles=GetTriangles(mesh);
				int i=0,imax=triangles.Length,t0,t1,t2;
				List<int> list=new List<int>(imax);
				for(imax/=3;i<imax;++i) {
					t0=triangles[3*i+0];
					t1=triangles[3*i+1];
					t2=triangles[3*i+2];
					//
					if(!OnCullMesh(t0,t1,t2)) {
						list.Add(t0);
						list.Add(t1);
						list.Add(t2);
					}
				}
				SetTriangles(mesh,list.ToArray());
				if(optimize) {mesh.Optimize();}
			EndCullMesh(mesh);
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}

	public abstract class MeshCullerBase_ByVertex:MeshCullerBase {
		[System.NonSerialized]public Vector3[] vertices;
		public abstract bool CullTest(Vector3 p0,Vector3 p1,Vector3 p2);

		public override void BeginCullMesh(Mesh mesh) {
			vertices=mesh.vertices;
		}

		public override void EndCullMesh(Mesh mesh) {
			vertices=null;
		}

		public override bool OnCullMesh(int a,int b,int c) {
			return CullTest(vertices[a],vertices[b],vertices[c]);
		}
	}
}
