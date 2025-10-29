/* <!-- Macro.Table Options
bool,protect_boundary,true,
bool,protect_detail,false,
bool,protect_symmetry,false,
bool,protect_normal,false,
bool,protect_shape,true,
bool,use_detail_map,false,
int,detail_boost,10,
 Macro.End --> */
/* <!-- Macro.Call  Options
		public {0} {1}={2};
 Macro.End --> */
/* <!-- Macro.Patch
,Field
 Macro.End --> */
/* <!-- Macro.Call  Options
			,int {1}
 Macro.End --> */
/* <!-- Macro.Patch
,Method
 Macro.End --> */
/* <!-- Macro.Call  Options
					,ToInt({1})
 Macro.End --> */
/* <!-- Macro.Replace
ToInt(detail_boost),detail_boost
 Macro.End --> */
/* <!-- Macro.Patch
,Call
 Macro.End --> */
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Pool;
#if !true
using API=MantisLOD.MantisLODSimpler;
#else
using API=YouSingStudio.MeshKit.MantisLODEditor;
#endif

namespace YouSingStudio.MeshKit {
	/// <summary>
	/// Taken from https://assetstore.unity.com/packages/tools/modeling/mantis-lod-editor-professional-edition-37086
	/// </summary>
	public class MantisLODEditor
		:MeshModifierBase
	{
		#region Fields

		public TextAsset text;
		[Range(0.0f,100.0f)]
		public float quality=100.0f;
		[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.UnityObjectConverter))]
		public MeshSelectorBase[] selectors;
		[Range(0.0f,100.0f)]
		public float[] qualities;
		[Header("Options")]
		public bool rename=false;
		public bool recalculate=false;
// <!-- Macro.Patch Field
		public bool protect_boundary=true;
		public bool protect_detail=false;
		public bool protect_symmetry=false;
		public bool protect_normal=false;
		public bool protect_shape=true;
		public bool use_detail_map=false;
		public int detail_boost=10;
// Macro.Patch -->

		[System.NonSerialized]protected int m_Index=-1;
		[System.NonSerialized]protected Vector3[] m_Vertices;
		[System.NonSerialized]protected Vector3[] m_Normals;
		[System.NonSerialized]protected Vector2[] m_UV;
		[System.NonSerialized]protected Color[] m_Colors;

		#endregion Fields

		#region Methods

		[DllImport("MantisLOD")]
		private static extern int create_progressive_mesh(Vector3[] vertex_array,int vertex_count,int[] triangle_array,int triangle_count,Vector3[] normal_array,int normal_count,Color[] color_array,int color_count,Vector2[] uv_array,int uv_count
// <!-- Macro.Patch Method
			,int protect_boundary
			,int protect_detail
			,int protect_symmetry
			,int protect_normal
			,int protect_shape
			,int use_detail_map
			,int detail_boost
// Macro.Patch -->
		);
		[DllImport("MantisLOD")]
		private static extern int get_triangle_list(int index,float goal,int[] triangles,ref int triangle_count);
		[DllImport("MantisLOD")]
		private static extern int delete_progressive_mesh(int index);

		public static int ToInt(bool x)=>x?1:0;

		public static void ToAPI(ref int[] triangles) {
			if(triangles!=null) {
				int cnt=triangles?.Length??0;int[] tmp=new int[cnt+1];
				System.Array.Copy(triangles,0,tmp,1,cnt);tmp[0]=cnt;
				triangles=tmp;
			}
		}

		public static void FromAPI(ref int[] triangles) {
			if(triangles!=null) {
				int cnt=triangles[0];System.Array.Copy(triangles,1,triangles,0,cnt);
				System.Array.Resize(ref triangles,cnt);
			}
		}

		protected virtual void Set(Mesh mesh) {
			if(mesh!=null) {
				m_Vertices=mesh.vertices;m_Normals=mesh.normals;
				m_UV=mesh.uv;m_Colors=mesh.colors;
			}else {
				m_Vertices=null;m_Normals=null;
				m_UV=null;m_Colors=null;
			}
		}

		protected virtual void Begin(int[] triangles) {
			ToAPI(ref triangles);
			m_Index=API.create_progressive_mesh(
				m_Vertices,m_Vertices?.Length??0,
				triangles,triangles?.Length??0,m_Normals,m_Normals?.Length??0,
				m_Colors,m_Colors?.Length??0,m_UV,m_UV?.Length??0
// <!-- Macro.Patch Call
					,ToInt(protect_boundary)
					,ToInt(protect_detail)
					,ToInt(protect_symmetry)
					,ToInt(protect_normal)
					,ToInt(protect_shape)
					,ToInt(use_detail_map)
					,detail_boost
// Macro.Patch -->
			);
		}

		protected virtual void Get(float value,ref int[] triangles) {
			int cnt=triangles?.Length??0;
			if(value<=0.0f||value>=100.0f) {cnt=-1;}
			if(cnt>0) {
				if(API.get_triangle_list(m_Index,value,triangles,ref cnt)>0) {
					FromAPI(ref triangles);
				}else {
					triangles=null;
				}
			}
		}

		protected virtual void End() {
			API.delete_progressive_mesh(m_Index);m_Index=-1;
		}

		protected virtual void Run(float value,ref int[] triangles) {
			Begin(triangles);
				Get(value,ref triangles);
			End();
		}

		protected virtual void LoadJson(string key) {
			if(text!=null) {
				Newtonsoft.Json.UnityObjectConverter.transform=transform;
					var all=Newtonsoft.Json.Linq.JObject.Parse(text.text);
					var it=all[key];//if(it==null) {it=all[name];}
					if(it!=null) {Newtonsoft.Json.JsonConvert.PopulateObject(it.ToString(),this);}
				Newtonsoft.Json.UnityObjectConverter.transform=null;
			}
		}

		public override void Run() {
			if(!this.IsActiveAndEnabled()) {return;}
			Mesh mesh=BeginModifyMesh();Set(mesh);
				if(mesh!=null) {using(ListPool<int>.Get(out var optimized)) {
					LoadJson(name);LoadJson(mesh.name);
					int i,imax=Mathf.Min(selectors?.Length??0,qualities?.Length??0);
					int[] triangles=GetTriangles(mesh);
					int prev=triangles.Length,next=0;
					// Sub Tasks
					if(imax>0) {
					using(ListPool<int>.Get(out var list)) {
						int[] tmp;bool[] opened=new bool[prev/3];
						for(i=0;i<imax;++i) {
							if(selectors[i]!=null) {
								list.Clear();selectors[i].SelectVertices(mesh,triangles,opened,list);
								if(list.Count>0) {
									tmp=list.ToArray();Run(qualities[i],ref tmp);
									if(tmp!=null) {optimized.AddRange(tmp);}
								}
							}
						}
						list.Clear();for(i=0,imax=prev/3;i<imax;++i) {
							if(!opened[i]) {
								list.Add(triangles[3*i+0]);
								list.Add(triangles[3*i+1]);
								list.Add(triangles[3*i+2]);
							}
						}
						triangles=list.ToArray();
					}}
					// Main Task
					Run(quality,ref triangles);
					if(triangles!=null) {optimized.AddRange(triangles);}
					next=optimized.Count;if(next>0&&next<prev) {
						SetTriangles(mesh,optimized.ToArray());
						if(rename) {mesh.name=mesh.name+"_Quality_"+(int)quality;}
						if(recalculate) {mesh.RecalculateNormals();}
						Debug.LogFormat("{0}:{1}->{2}({3:P2})",mesh.name,prev/3,next/3,next/(float)prev);
						// TODO: Missing blend shapes????
					}
				}}
			Set(null);EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}