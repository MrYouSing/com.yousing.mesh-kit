/* <!-- Macro.Table Nearest
Vector2,uv0,UV,Uv,
Vector3,position,Vertices,Pos,
 Macro.End --> */
/* <!-- Macro.Call  Nearest
		protected virtual int $(Table.Name)Of{3}(int index,bool any=false) {{
			{0} u=m_{2}[index],v;int j=-1;float s,m=float.MaxValue;
			for(int i=0,imax=m_Src.Length;i<imax;++i) {{
				if(!any&&!IsValid(i)) {{continue;}}
				v=m_Src[i].{1};s=(v-u).sqrMagnitude;if(s<m) {{
					m=s;j=i;
				}}
			}}
			if(j>=0&&m>threshold.x*threshold.x) {{j=-1;}}
			return j;
		}}

 Macro.End --> */
/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static YouSingStudio.MeshKit.VertexHelper;

namespace YouSingStudio.MeshKit {
	public class MeshPropertySampler
		:MeshModifierBase
	{
// <!-- Macro.Patch AutoGen
		protected virtual int NearestOfUv(int index,bool any=false) {
			Vector2 u=m_UV[index],v;int j=-1;float s,m=float.MaxValue;
			for(int i=0,imax=m_Src.Length;i<imax;++i) {
				if(!any&&!IsValid(i)) {continue;}
				v=m_Src[i].uv0;s=(v-u).sqrMagnitude;if(s<m) {
					m=s;j=i;
				}
			}
			if(j>=0&&m>threshold.x*threshold.x) {j=-1;}
			return j;
		}

		protected virtual int NearestOfPos(int index,bool any=false) {
			Vector3 u=m_Vertices[index],v;int j=-1;float s,m=float.MaxValue;
			for(int i=0,imax=m_Src.Length;i<imax;++i) {
				if(!any&&!IsValid(i)) {continue;}
				v=m_Src[i].position;s=(v-u).sqrMagnitude;if(s<m) {
					m=s;j=i;
				}
			}
			if(j>=0&&m>threshold.x*threshold.x) {j=-1;}
			return j;
		}

// Macro.Patch -->
		#region Nested Types

		[System.Flags]
		public enum Feature {
			Debug=1<<0,
			Resample=1<<1,
			Reliable=1<<2,
			Reduce=1<<3,
		}

		#endregion Nested Types

		#region Fields

		[Header("Sampler")]
		public Mesh source;
		public MeshSelectorBase selector;
		public MeshSelectorBase safearea;
		public Feature features=(Feature)(-1);
		public Mask mask=Mask.All;
		public Vector4 threshold=new Vector4(0.01f,0.1f);
		[System.NonSerialized]protected Vertex[] m_Src;
		[System.NonSerialized]protected Vertex[] m_Dst;
		[System.NonSerialized]protected Vector2[] m_UV;
		[System.NonSerialized]protected Vector3[] m_Vertices;
		[System.NonSerialized]protected int[] m_Triangles;
		[System.NonSerialized]protected HashSet<int> m_MainFound=new HashSet<int>();
		[System.NonSerialized]protected HashSet<int> m_SubFound=new HashSet<int>();// For Un-Weld Mesh.
		[System.NonSerialized]protected List<Vector3> m_Points=new List<Vector3>();

		#endregion Fields

		#region Unity Messages

		protected virtual void OnDrawGizmosSelected() {
			if((features&Feature.Debug)==0) {return;}
			Color c=Gizmos.color;
			Matrix4x4 m=Gizmos.matrix;
				float r=0.009f,g=0.01f;
				for(int i=0,imax=Mathf.Min(m_Points.Count,1024*8);i<imax;) {
					Gizmos.color=Color.red;
					Gizmos.DrawSphere(m_Points[i],r);++i;
					Gizmos.color=Color.green;
					Gizmos.DrawWireSphere(m_Points[i],g);++i;
				}
			Gizmos.color=c;
			Gizmos.matrix=m;
		}

		#endregion Unity Messages

		#region Methods

		public override void Run(Mesh mesh) {
			if(source==null) {
				Mesh tmp=s_Mesh;s_Mesh=null;
					source=mesh;Run();source=null;
				s_Mesh=tmp;
			}else {
				base.Run(mesh);
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null&&source!=null) {
				Mask m=mask|Mask.Position|Mask.UV0;
				m_MainFound.Clear();m_SubFound.Clear();m_Points.Clear();
				m_Src=FromMesh(source,m);m_Dst=FromMesh(mesh,mask);
				m_UV=mesh.uv;m_Vertices=mesh.vertices;m_Triangles=mesh.triangles;
				//
				List<int> selection=null;
				if(selector!=null) {
					List<int> triangles=new List<int>(GetTriangles(mesh));
					selection=selector.SelectVertices(mesh,triangles);
				}
				//
				if(safearea!=null) {safearea.BeginSelect(source);}
					int i,imax=m_Dst?.Length??0;for(i=0;i<imax;++i) {
					if(selection==null||selection.IndexOf(i)>=0) {
						m_Dst[i]=Sample(i);
					}}
					if((features&Feature.Resample)!=0) {for(i=0;i<imax;++i) {
					if(selection==null||selection.IndexOf(i)>=0) {if(!m_SubFound.Contains(i)) {
						m_Dst[i]=Resample(i);
					}}}}
					ToMesh(mesh,m_Dst,mask);if((mask&Mask.BoneWeight)!=0) {mesh.bindposes=source.bindposes;}
					if((features&Feature.Resample)!=0) {SetTriangles(mesh,Reduce());}
				if(safearea!=null) {safearea.EndSelect(source);}
				//
				m_MainFound.Clear();m_SubFound.Clear();
				m_Src=m_Dst=null;
				m_UV=null;m_Vertices=null;m_Triangles=null;
			}
			EndModifyMesh(mesh);
		}

		protected virtual bool IsValid(int index) {
			if(index>=0) {
				if(safearea!=null) {return safearea.TestIndex(index);}
				else {return true;}
			}
			return false;
		}

		protected virtual bool IsValid(int index,Vertex vertex) {
			if(safearea!=null) {
				//index=NearestOfPos(index,true);print(index);
				//if(index<0||safearea.TestIndex(index)){
					return safearea.TestVertex(vertex.uv0);
				//}
			}
			return true;
		}

		protected virtual int NearestOf(int index) {
			if((m_UV?.Length??0)==0) {return NearestOfPos(index);}
			else {return NearestOfUv(index);}
		}

		protected virtual Vertex Sample(int index) {
			Vertex v=m_Dst[index];int s=NearestOf(index);
			if(s>=0){//IsValid(s)) {
				m_MainFound.Add(index);m_SubFound.Add(index);
				//
				m_Points.Add(m_Vertices[index]);
				v=m_Src[s];
				m_Points.Add(v.position);
			}
			return v;
		}

		protected virtual int IsTriangle(int[] a,int[] b) {
			int j=0;for(int i=0;i<3;++i) {
				if(System.Array.IndexOf(a,b[i])>=0) {++j;}
			}
			return j;
		}

		protected virtual int[] GetTriangle(int index,int[] triangle=null) {
			if(triangle==null) {triangle=new int[3];}
			for(int i=0,j=(index/3)*3;i<3;++i) {
				triangle[i]=m_Triangles[j+i];
			}
			System.Array.Sort(triangle);
			return triangle;
		}

		protected virtual bool AddTriangle(List<int> list,int index) {
			bool b=false;
			for(int i=0,j=(index/3)*3,k=index%3,t;i<3;++i) {
				if(i!=k) {t=m_Triangles[j+i];
					if((features&Feature.Reliable)!=0&&!m_MainFound.Contains(t)) {continue;}
					b=true;if(list.IndexOf(t)<0) {list.Add(t);}
				}
			}
			return b;
		}

		protected virtual bool AddQuad(List<int> list,int index) {
			bool b=false;
			int[] t=GetTriangle(index),s=GetTriangle(index);int r=m_Triangles[index];
			for(int i=0,imax=m_Triangles.Length;i<imax;i+=3) {
				GetTriangle(i,s);if(System.Array.IndexOf(s,r)<0&&IsTriangle(s,t)==2) {
					if(AddTriangle(list,i)&&!b) {b=true;}
				}
			}
			return b;
		}

		protected virtual int FindSiblings(int index,List<int> list) {
			int n=list.Count;
			for(int i=0,imax=m_Triangles.Length,j,t;i<imax;i+=3) {
				for(j=0;j<3;++j) {t=m_Triangles[i+j];
					if(t==index) {
						if(AddTriangle(list,i+j)) {}
						else if(AddQuad(list,i+j)) {}
						break;
					}
				}
			}
			return list.Count-n;
		}

		protected virtual int AddPoints(int index,List<int> list) {
			int n=list.Count;Vector3 v=m_Vertices[index];
			for(int i=0,imax=m_Vertices.Length;i<imax;++i) {
				if(i!=index&&(v-m_Vertices[i]).sqrMagnitude<1E-10) {
					list.Add(i);
				}
			}
			return list.Count-n;
		}

		protected virtual int RemovePoints(List<int> list) {
			int n=list.Count;int i,imax,j;
			using(ListPool<int>.Get(out var tmp)) {
				while(true) {
					for(i=0,imax=list.Count;i<imax;++i) {
						tmp.Clear();if(AddPoints(list[i],tmp)>0) {
							j=tmp.Count;while(j-->0) {list.Remove(tmp[j]);}
							if(tmp.Count!=imax) {break;}
						}
					}
					if(i>=imax) {break;}
				}
			}
			return n-list.Count;
		}

		protected virtual Vertex Sample(Vertex vertex,List<Vertex> list) {
			int i,imax=list.Count;
			float sum=0.0f,dis;Vertex v;float[] tmp=new float[imax];
			for(i=0;i<imax;++i) {
				v=list[i];dis=Distance(vertex,v);
				tmp[i]=dis;sum+=dis;
			}
			for(i=0;i<imax;++i) {tmp[i]/=sum;}
			vertex=VertexHelper.Sample(list,tmp,mask);
			return vertex;
		}

		protected virtual Vertex Resample(int index) {
			Vertex v=m_Dst[index];int i,imax;
			using(ListPool<int>.Get(out var vid)) {AddPoints(index,vid);
			using(ListPool<int>.Get(out var sid)) {FindSiblings(index,sid);
				for(i=0,imax=vid.Count;i<imax;++i) {FindSiblings(vid[i],sid);}
				imax=sid.Count;if(imax>0) {
					sid.Sort();//RemovePoints(sid);
					using(ListPool<Vertex>.Get(out var tmp)) {
						for(i=0;i<imax;++i) {tmp.Add(m_Dst[sid[i]]);}
						Regionalize(tmp,threshold.y);
						Vertex s=Sample(v,tmp);if(IsValid(index,s)) {v=s;}
					}
				}
				//
				m_SubFound.Add(index);
				for(i=0,imax=vid.Count;i<imax;++i) {m_SubFound.Add(vid[i]);m_Dst[vid[i]]=v;}
			}}
			return v;
		}

		protected virtual int[] Reduce() {
			using(ListPool<int>.Get(out var list)) {int a,b,c;
				for(int i=0,imax=m_Triangles?.Length??0;i<imax;i+=3) {
					a=m_Triangles[i+0];
					b=m_Triangles[i+1];
					c=m_Triangles[i+2];
					if(m_Dst[a].uv0.sqrMagnitude!=0.0f
					 &&m_Dst[b].uv0.sqrMagnitude!=0.0f
					 &&m_Dst[c].uv0.sqrMagnitude!=0.0f
					) {
						list.Add(a);
						list.Add(b);
						list.Add(c);
					}
				}
				return list.ToArray();
			}
		}

		#endregion Methods
	}
}
