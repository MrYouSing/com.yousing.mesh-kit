using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshBoundary
		:MonoTask
	{
		#region Fields
		
		public new Transform renderer;
		public Mesh mesh;
		public Transform target;
		public string path;

		#endregion Fields

		#region Methods

		public virtual Mesh GetMesh() {
			if(mesh!=null) {return mesh;}
			else {return renderer.GetInstancedMesh();}
		}

		public virtual Bounds GetBounds(Mesh mesh,Matrix4x4 matrix) {
			Bounds tmp=new Bounds();
			Vector3[] verts=mesh.vertices;
			int[] tris=mesh.triangles;
			tmp.center=matrix.MultiplyPoint3x4(verts[tris[0]]);
			for(int i=1,imax=tris.Length;i<imax;++i) {
				tmp.Encapsulate(matrix.MultiplyPoint3x4(verts[tris[i]]));
			}
			return tmp;
		}

		public virtual Matrix4x4 GetMatrix() {
			Matrix4x4 m=Matrix4x4.identity;
			if(renderer!=null) {
				m=renderer.localToWorldMatrix;
			}
			if(target!=null) {
				Transform p=target.parent;
				if(p!=null) {m=p.worldToLocalMatrix*m;}
			}
			return m;
		}

		public override void Run() {
			Mesh tmp=GetMesh();
			if(tmp!=null) {
				Bounds b=GetBounds(tmp,GetMatrix());
				target.localPosition=b.center;
				target.localScale=b.size;
				//
				if(!string.IsNullOrEmpty(path)) {
					var t=target.parent;
					target.SetParent(target.FindEx(path));
					DestroyImmediate(t.gameObject);
				}
			}
		}

		#endregion Methods
	}
}
