using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public partial class MeshFaceAppender
		:MeshModifierBase
	{
		#region Fields

		public int mode=GL.TRIANGLES;
		public bool isOverride;
		public int[] data=new int[0];

		#endregion Fields

		#region Methods

		public virtual void Add(List<int> triangles,int t0,int t1,int t2) {
			if(t0<0||t1<0||t2<0) {return;}
			//
			triangles.Add(t0);
			triangles.Add(t1);
			triangles.Add(t2);
		}

		public virtual void Add(List<int> triangles,int t0,int t1,int t2,int t3) {
			if(t0<0||t1<0||t2<0||t3<0) {return;}
			//
			triangles.Add(t0);
			triangles.Add(t1);
			triangles.Add(t2);
			triangles.Add(t1);
			triangles.Add(t3);
			triangles.Add(t2);
		}

		public virtual void AddTriangles(List<int> triangles,int[] data) {
			for(int i=0,imax=data?.Length??0;i<imax;i+=3) {
				Add(triangles,data[i+0],data[i+1],data[i+2]);
			}
		}

		public virtual void AddTriangleStrip(List<int> triangles,int[] data) {
			for(int i=0,imax=(data?.Length??0)-2;i<imax;++i) {
				Add(triangles,data[i+0],data[i+1],data[i+2]);
			}
		}

		public virtual void AddQuads(List<int> triangles,int[] data) {
			for(int i=0,imax=data?.Length??0;i<imax;i+=4) {
				Add(triangles,data[i+0],data[i+1],data[i+2],data[i+3]);
			}
		}

		public virtual void AddQuadStrip(List<int> triangles,int[] data) {
			for(int i=0,imax=(data?.Length??0)-3;i<imax;i+=2) {
				Add(triangles,data[i+0],data[i+1],data[i+2],data[i+3]);
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				List<int> triangles=new List<int>(GetTriangles(mesh));
				if(isOverride){triangles.Clear();}
				switch(mode) {
					case GL.TRIANGLES:
						AddTriangles(triangles,data);
					break;
					case GL.TRIANGLE_STRIP:
						AddTriangleStrip(triangles,data);
					break;
					case GL.QUADS:
						AddQuads(triangles,data);
					break;
					case GL.QUADS+1:
						AddQuadStrip(triangles,data);
					break;
				}
				SetTriangles(mesh,triangles.ToArray());
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}