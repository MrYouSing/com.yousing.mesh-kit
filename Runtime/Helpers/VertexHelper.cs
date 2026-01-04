/* <!-- Macro.Table Vertex
Vector3,position,vertices,
Vector3,normal,normals,
Vector4,tangent,tangents,
Color,color,colors,
Vector2,uv0,uv,
Vector2,uv1,uv2,
Vector2,uv2,uv3,
Vector2,uv3,uv4,
Vector2,uv4,uv5,
Vector2,uv5,uv6,
Vector2,uv6,uv7,
Vector2,uv7,uv8,
BoneWeight,boneWeight,boneWeights,
 Macro.End --> */
/* <!-- Macro.Table UV
,
,
,
,
,
,
,
,
 Macro.End --> */
/* <!-- Macro.Table Replace
@p,P
@n,N
@t,T
@c,C
@uv,UV
@b,B
 Macro.End --> */
/* <!-- Macro.Copy
		[System.Flags]
		public enum Mask {
 Macro.End --> */
/* <!-- Macro.Call  Vertex
			@{1}=1<<$(Table.Row),
 Macro.End --> */
/* <!-- Macro.Call  UV
			TexCoord$(Table.Row)=UV$(Table.Row),
 Macro.End --> */
/* <!-- Macro.Copy
			BlendWeight=BoneWeight,
			All=-1
		}

		public struct Vertex {
 Macro.End --> */
/* <!-- Macro.Call  Vertex
			public {0} {1};
 Macro.End --> */
/* <!-- Macro.Copy
		}

 Macro.End --> */
/* <!-- Macro.Table Types
Vector2,
Vector3,
Vector4,
Color,
 Macro.End --> */
/* <!-- Macro.Call  Types
		public static {0} Sample(IList<{0}> v,IList<float> w) {{
			{0} r=default;
			for(int i=0,imax=Mathf.Min(v?.Count??0,w?.Count??0);i<imax;++i) {{
				r+=v[i]*w[i];//{0}
			}}
			return r;
		}}

 Macro.End --> */
/* <!-- Macro.Copy
		public static Vertex Sample(IList<Vertex> vertices,IList<float> weights,Mask mask=Mask.All) {
			Vertex r=new Vertex();
			int i,imax=Mathf.Min(vertices?.Count??0,weights?.Count??0);
 Macro.End --> */
/* <!-- Macro.Call  Vertex
			if((mask&Mask.@{1})!=0) {{
			using(ListPool<{0}>.Get(out var tmp)) {{
				for(i=0;i<imax;++i) {{tmp.Add(vertices[i].{1});}}
				r.{1}=Sample(tmp,weights);
			}}}}
 Macro.End --> */
/* <!-- Macro.Copy
			return r;
		}

 Macro.End --> */
/* <!-- Macro.Copy
		public static Vertex[] FromMesh(Mesh mesh,Mask mask=Mask.All) {
			Vertex[] data=null;
			if(mesh!=null) {
				data=new Vertex[mesh.vertexCount];
				int i,imax=mesh.vertexCount;Vertex v;
 Macro.End --> */
/* <!-- Macro.Call  Vertex
				if((mask&Mask.@{1})!=0) {{
					var tmp=mesh.{2};
					if((tmp?.Length??0)>0) {{
					for(i=0;i<imax;++i) {{
						v=data[i];
							v.{1}=tmp[i];
						data[i]=v;
					}}}}
				}}
 Macro.End --> */
/* <!-- Macro.Copy
			}
			return data;
		}

 Macro.End --> */
/* <!-- Macro.Copy
		public static void ToMesh(Mesh mesh,Vertex[] data,Mask mask=Mask.All) {
			if(mesh!=null) {
				int i,imax=mesh.vertexCount;
 Macro.End --> */
/* <!-- Macro.Call  Vertex
				if((mask&Mask.@{1})!=0) {{
					var tmp=new {0}[imax];
					for(i=0;i<imax;++i) {{
						tmp[i]=data[i].{1};
					}}
					mesh.{2}=tmp;
				}}
 Macro.End --> */
/* <!-- Macro.Copy
			}
		}

 Macro.End --> */
/* <!-- Macro.Table Distance
,m,
Sqr,sqrM,
 Macro.End --> */
/* <!-- Macro.Call  Distance
		public static float {0}Distance(Vertex a,Vertex b) {{
			if(a.uv0.sqrMagnitude==0.0f) return (b.position-a.position).{1}agnitude;
			else return (b.uv0-a.uv0).{1}agnitude;
		}}

 Macro.End --> */
/* <!-- Macro.Replace Replace
 Macro.End --> */
/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public static class VertexHelper
	{
// <!-- Macro.Patch AutoGen
		[System.Flags]
		public enum Mask {
			Position=1<<0,
			Normal=1<<1,
			Tangent=1<<2,
			Color=1<<3,
			UV0=1<<4,
			UV1=1<<5,
			UV2=1<<6,
			UV3=1<<7,
			UV4=1<<8,
			UV5=1<<9,
			UV6=1<<10,
			UV7=1<<11,
			BoneWeight=1<<12,
			TexCoord0=UV0,
			TexCoord1=UV1,
			TexCoord2=UV2,
			TexCoord3=UV3,
			TexCoord4=UV4,
			TexCoord5=UV5,
			TexCoord6=UV6,
			TexCoord7=UV7,
			BlendWeight=BoneWeight,
			All=-1
		}

		public struct Vertex {
			public Vector3 position;
			public Vector3 normal;
			public Vector4 tangent;
			public Color color;
			public Vector2 uv0;
			public Vector2 uv1;
			public Vector2 uv2;
			public Vector2 uv3;
			public Vector2 uv4;
			public Vector2 uv5;
			public Vector2 uv6;
			public Vector2 uv7;
			public BoneWeight boneWeight;
		}

		public static Vector2 Sample(IList<Vector2> v,IList<float> w) {
			Vector2 r=default;
			for(int i=0,imax=Mathf.Min(v?.Count??0,w?.Count??0);i<imax;++i) {
				r+=v[i]*w[i];//Vector2
			}
			return r;
		}

		public static Vector3 Sample(IList<Vector3> v,IList<float> w) {
			Vector3 r=default;
			for(int i=0,imax=Mathf.Min(v?.Count??0,w?.Count??0);i<imax;++i) {
				r+=v[i]*w[i];//Vector3
			}
			return r;
		}

		public static Vector4 Sample(IList<Vector4> v,IList<float> w) {
			Vector4 r=default;
			for(int i=0,imax=Mathf.Min(v?.Count??0,w?.Count??0);i<imax;++i) {
				r+=v[i]*w[i];//Vector4
			}
			return r;
		}

		public static Color Sample(IList<Color> v,IList<float> w) {
			Color r=default;
			for(int i=0,imax=Mathf.Min(v?.Count??0,w?.Count??0);i<imax;++i) {
				r+=v[i]*w[i];//Color
			}
			return r;
		}

		public static Vertex Sample(IList<Vertex> vertices,IList<float> weights,Mask mask=Mask.All) {
			Vertex r=new Vertex();
			int i,imax=Mathf.Min(vertices?.Count??0,weights?.Count??0);
			if((mask&Mask.Position)!=0) {
			using(ListPool<Vector3>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].position);}
				r.position=Sample(tmp,weights);
			}}
			if((mask&Mask.Normal)!=0) {
			using(ListPool<Vector3>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].normal);}
				r.normal=Sample(tmp,weights);
			}}
			if((mask&Mask.Tangent)!=0) {
			using(ListPool<Vector4>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].tangent);}
				r.tangent=Sample(tmp,weights);
			}}
			if((mask&Mask.Color)!=0) {
			using(ListPool<Color>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].color);}
				r.color=Sample(tmp,weights);
			}}
			if((mask&Mask.UV0)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv0);}
				r.uv0=Sample(tmp,weights);
			}}
			if((mask&Mask.UV1)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv1);}
				r.uv1=Sample(tmp,weights);
			}}
			if((mask&Mask.UV2)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv2);}
				r.uv2=Sample(tmp,weights);
			}}
			if((mask&Mask.UV3)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv3);}
				r.uv3=Sample(tmp,weights);
			}}
			if((mask&Mask.UV4)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv4);}
				r.uv4=Sample(tmp,weights);
			}}
			if((mask&Mask.UV5)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv5);}
				r.uv5=Sample(tmp,weights);
			}}
			if((mask&Mask.UV6)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv6);}
				r.uv6=Sample(tmp,weights);
			}}
			if((mask&Mask.UV7)!=0) {
			using(ListPool<Vector2>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].uv7);}
				r.uv7=Sample(tmp,weights);
			}}
			if((mask&Mask.BoneWeight)!=0) {
			using(ListPool<BoneWeight>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {tmp.Add(vertices[i].boneWeight);}
				r.boneWeight=Sample(tmp,weights);
			}}
			return r;
		}

		public static Vertex[] FromMesh(Mesh mesh,Mask mask=Mask.All) {
			Vertex[] data=null;
			if(mesh!=null) {
				data=new Vertex[mesh.vertexCount];
				int i,imax=mesh.vertexCount;Vertex v;
				if((mask&Mask.Position)!=0) {
					var tmp=mesh.vertices;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.position=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.Normal)!=0) {
					var tmp=mesh.normals;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.normal=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.Tangent)!=0) {
					var tmp=mesh.tangents;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.tangent=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.Color)!=0) {
					var tmp=mesh.colors;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.color=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV0)!=0) {
					var tmp=mesh.uv;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv0=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV1)!=0) {
					var tmp=mesh.uv2;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv1=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV2)!=0) {
					var tmp=mesh.uv3;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv2=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV3)!=0) {
					var tmp=mesh.uv4;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv3=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV4)!=0) {
					var tmp=mesh.uv5;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv4=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV5)!=0) {
					var tmp=mesh.uv6;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv5=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV6)!=0) {
					var tmp=mesh.uv7;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv6=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.UV7)!=0) {
					var tmp=mesh.uv8;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.uv7=tmp[i];
						data[i]=v;
					}}
				}
				if((mask&Mask.BoneWeight)!=0) {
					var tmp=mesh.boneWeights;
					if((tmp?.Length??0)>0) {
					for(i=0;i<imax;++i) {
						v=data[i];
							v.boneWeight=tmp[i];
						data[i]=v;
					}}
				}
			}
			return data;
		}

		public static void ToMesh(Mesh mesh,Vertex[] data,Mask mask=Mask.All) {
			if(mesh!=null) {
				int i,imax=mesh.vertexCount;
				if((mask&Mask.Position)!=0) {
					var tmp=new Vector3[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].position;
					}
					mesh.vertices=tmp;
				}
				if((mask&Mask.Normal)!=0) {
					var tmp=new Vector3[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].normal;
					}
					mesh.normals=tmp;
				}
				if((mask&Mask.Tangent)!=0) {
					var tmp=new Vector4[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].tangent;
					}
					mesh.tangents=tmp;
				}
				if((mask&Mask.Color)!=0) {
					var tmp=new Color[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].color;
					}
					mesh.colors=tmp;
				}
				if((mask&Mask.UV0)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv0;
					}
					mesh.uv=tmp;
				}
				if((mask&Mask.UV1)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv1;
					}
					mesh.uv2=tmp;
				}
				if((mask&Mask.UV2)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv2;
					}
					mesh.uv3=tmp;
				}
				if((mask&Mask.UV3)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv3;
					}
					mesh.uv4=tmp;
				}
				if((mask&Mask.UV4)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv4;
					}
					mesh.uv5=tmp;
				}
				if((mask&Mask.UV5)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv5;
					}
					mesh.uv6=tmp;
				}
				if((mask&Mask.UV6)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv6;
					}
					mesh.uv7=tmp;
				}
				if((mask&Mask.UV7)!=0) {
					var tmp=new Vector2[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].uv7;
					}
					mesh.uv8=tmp;
				}
				if((mask&Mask.BoneWeight)!=0) {
					var tmp=new BoneWeight[imax];
					for(i=0;i<imax;++i) {
						tmp[i]=data[i].boneWeight;
					}
					mesh.boneWeights=tmp;
				}
			}
		}

		public static float Distance(Vertex a,Vertex b) {
			if(a.uv0.sqrMagnitude==0.0f) return (b.position-a.position).magnitude;
			else return (b.uv0-a.uv0).magnitude;
		}

		public static float SqrDistance(Vertex a,Vertex b) {
			if(a.uv0.sqrMagnitude==0.0f) return (b.position-a.position).sqrMagnitude;
			else return (b.uv0-a.uv0).sqrMagnitude;
		}

// Macro.Patch -->
		#region Fields
		#endregion Fields

		#region Methods

		public static bool Faraway(IList<Vertex> list,int idx,float error=0.25f) {
			int i=0,imax=list.Count;error*=error;
			for(;i<imax;++i) {
				if(i!=idx) {
				if(SqrDistance(list[idx],list[i])<error) {
					return false;
				}}
			}
			return imax>1;
		}

		public static void Regionalize(IList<Vertex> list,float error=0.25f) {
			int i,imax;
			while(true) {
				for(i=0,imax=list.Count;i<imax;++i) {
					if(Faraway(list,i,error)) {list.RemoveAt(i);break;}
				}
				if(i>=imax) {return;}
			}
		}

		public static void Sample(IDictionary<int,float> d,BoneWeight v,float w) {
			int n;float f;
			for(int i=0;i<4;++i) {
				n=v.GetBoneIndex(i);f=v.GetWeight(i)*w;
				if(f==0.0f) {continue;}
				if(d.ContainsKey(n)) {d[n]+=f;}
				else {d[n]=f;}
			}
		}

		public static void Remove(IDictionary<int,float> d) {
			int i=-1;float w,m=float.MaxValue;
			foreach(var it in d) {
				w=it.Value;if(w<m) {
					m=w;i=it.Key;
				}
			}
			if(i>=0) {d.Remove(i);}
		}

		public static void Normalize(IDictionary<int,float> d) {
			float sum=0.0f;
			foreach(var it in d.Values) {
				sum+=it;
			}
			foreach(var it in d.Keys.ToArray<int>()) {
				d[it]/=sum;
			}
		}

		public static BoneWeight Sample(IList<BoneWeight> v,IList<float> w) {
			BoneWeight r=new BoneWeight();
			int i,imax=Mathf.Min(v?.Count??0,w?.Count??0);
			using(DictionaryPool<int,float>.Get(out var tmp)) {
				for(i=0;i<imax;++i) {
					Sample(tmp,v[i],w[i]);
				}
				if(tmp.Count>4) {
					while(tmp.Count>4) {Remove(tmp);}
					Normalize(tmp);
				}
				i=0;foreach(var it in tmp) {
					r.SetBoneIndex(i,it.Key);r.SetWeight(i,it.Value);
					++i;if(i>=4) {break;}
				}
			}
			return r;
		}

		#endregion Methods
	}
}
