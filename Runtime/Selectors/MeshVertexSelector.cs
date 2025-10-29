/* <!-- Macro.Table ToList
Position,vertices,,
Normal,normals,,
Tangent,,mesh.tangents;break;//,
Color,colors,,
TexCoord0,uv,,
TexCoord1,uv2,,
TexCoord2,uv3,,
TexCoord3,uv4,,
TexCoord4,uv5,,
TexCoord5,uv6,,
TexCoord6,uv7,,
TexCoord7,uv8,,
 Macro.End --> */
/* <!-- Macro.Call  ToList
					case VertexAttribute.{0}:src={2}System.Array.ConvertAll(mesh.{1},x=>(Vector4)x);break;
 Macro.End --> */
/* <!-- Macro.Patch
,ToList
 Macro.End --> */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace YouSingStudio.MeshKit {
	public class MeshVertexSelector
		:MeshSelectorBase
	{
		#region Fields

		public VertexAttribute vertex;
		public Object[] meshes;
		public int[] submeshes;

		[System.NonSerialized]protected List<Vector4> m_List;

		#endregion Fields

		#region Methods

		protected virtual IList<Vector4> ToList(Mesh mesh,int submesh) {
			if(mesh!=null) {
				IList<Vector4> src=null;
				switch(vertex) {
// <!-- Macro.Patch ToList
					case VertexAttribute.Position:src=System.Array.ConvertAll(mesh.vertices,x=>(Vector4)x);break;
					case VertexAttribute.Normal:src=System.Array.ConvertAll(mesh.normals,x=>(Vector4)x);break;
					case VertexAttribute.Tangent:src=mesh.tangents;break;//System.Array.ConvertAll(mesh.,x=>(Vector4)x);break;
					case VertexAttribute.Color:src=System.Array.ConvertAll(mesh.colors,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord0:src=System.Array.ConvertAll(mesh.uv,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord1:src=System.Array.ConvertAll(mesh.uv2,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord2:src=System.Array.ConvertAll(mesh.uv3,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord3:src=System.Array.ConvertAll(mesh.uv4,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord4:src=System.Array.ConvertAll(mesh.uv5,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord5:src=System.Array.ConvertAll(mesh.uv6,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord6:src=System.Array.ConvertAll(mesh.uv7,x=>(Vector4)x);break;
					case VertexAttribute.TexCoord7:src=System.Array.ConvertAll(mesh.uv8,x=>(Vector4)x);break;
// Macro.Patch -->
				}
				if(src!=null) {
					int[] idx=submesh<0?mesh.triangles:mesh.GetTriangles(submesh);
					int i=0,imax=src.Count;List<Vector4> dst=new List<Vector4>(imax);
					for(;i<imax;++i) {
						if(System.Array.IndexOf(idx,i)>=0) {dst.Add(src[i]);}
					}
					return dst;
				}
			}
			return null;
		}

		protected virtual void BakeList(List<Vector4> list,Object mesh,int submesh) {
			//
			Mesh m;
			if(mesh is GameObject go) {m=go.GetSharedMesh();}
			else if(mesh is Component comp) {m=comp.GetSharedMesh();}
			else {m=mesh as Mesh;}
			//
			if(m!=null) {
				var tmp=ToList(m,submesh);Vector4 v;
				for(int i=0,imax=tmp?.Count??0;i<imax;++i) {
					v=tmp[i];if(list.IndexOf(v)<0) {list.Add(v);}
				}
			}
		}

		protected virtual void BakeList() {
			if(m_List!=null) {m_List.Clear();}
			else {m_List=new List<Vector4>();}
			//
			int i=0,imax=Mathf.Min(meshes?.Length??0,submeshes?.Length??0);
			for(;i<imax;++i) {BakeList(m_List,meshes[i],submeshes[i]);}
		}

		protected virtual bool TestVertex(Vector4 x,Vector4 y) {
			return (x-y).sqrMagnitude<Vector3.kEpsilon;
		}

		public override bool TestVertex(Vector3 x) {
			int i=0,imax=m_List?.Count??0;
			if(imax==0) {BakeList();imax=m_List?.Count??0;}
			//
			for(;i<imax;++i) {
				if(TestVertex(m_List[i],x)) {return true;}
			}
			return false;
		}

		#endregion Methods
	}
}