using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract class MeshCullerBase:MeshModifierBase {
		#region Fields
		#endregion Fields

		#region Methods

		public abstract bool CullTest(Vector3 p0,Vector3 p1,Vector3 p2);

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				Vector3[] vertices=mesh.vertices;
				int[] triangles=GetTriangles(mesh);
				int i=0,imax=triangles.Length,t0,t1,t2;
				List<int> list=new List<int>(imax);
				for(imax/=3;i<imax;++i) {
					t0=triangles[3*i+0];
					t1=triangles[3*i+1];
					t2=triangles[3*i+2];
					//
					if(!CullTest(vertices[t0],vertices[t1],vertices[t2])) {
						list.Add(t0);
						list.Add(t1);
						list.Add(t2);
					}
				}
				SetTriangles(mesh,list.ToArray());
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
