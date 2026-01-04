using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshMorphGenerator
		:MeshModifierBase
	{
		#region Fields

		[Header("Morph")]
		public MeshSelectorBase selector;
		public Mesh morph;
		public bool normals=true;
		public bool tangents=true;
		protected Vector2[] m_UV0;
		protected Vector2[] m_UV1;
		protected BlendShapeFrame m_Src;
		protected BlendShapeFrame m_Dst;

		#endregion Fields

		#region Methods

		public override void Run(Mesh mesh) {
			if(morph==null) {
				Mesh tmp=s_Mesh;s_Mesh=null;
					morph=mesh;Run();morph=null;
				s_Mesh=tmp;
			}else {
				base.Run(mesh);
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null&&morph!=null){
				List<int> selection=null;
				if(selector!=null) {
					List<int> triangles=new List<int>(GetTriangles(mesh));
					selection=selector.SelectVertices(mesh,triangles);
				}
				//
				int i=0,imax=mesh.vertexCount;
				BlendShapeFrame f=new BlendShapeFrame(imax);
				m_Src=new BlendShapeFrame(mesh);m_Dst=new BlendShapeFrame(morph);
				m_UV0=mesh.uv;m_UV1=morph.uv;
				for(;i<imax;++i) {
					if(selection==null||selection.IndexOf(i)>=0) {
						Morph(f,i);
					}
				}
				f.AddTo(mesh,name);
				m_Src=m_Dst=null;
				m_UV0=m_UV1=null;
			}
			EndModifyMesh(mesh);
		}

		protected virtual int Find(int index) {
			Vector2 u=m_UV0[index],v;int j=-1;float s,m=float.MaxValue;
			for(int i=0,imax=m_UV1.Length;i<imax;++i) {
				v=m_UV1[i];s=(v-u).sqrMagnitude;if(s<m) {
					m=s;j=i;
				}
			}
			return j;
		}

		protected virtual void Morph(BlendShapeFrame frame,int index) {
			int dst=Find(index);
			if(dst>=0) {
				frame.vertices[index]=m_Dst.vertices[dst]-m_Src.vertices[index];
				if(normals&&(m_Src.normals?.Length??0)>0)
				frame.normals[index]=m_Dst.normals[dst]-m_Src.normals[index];
				if(tangents&&(m_Src.tangents?.Length??0)>0)
				frame.tangents[index]=m_Dst.tangents[dst]-m_Src.tangents[index];
			}
		}

		#endregion Methods
	}
}
