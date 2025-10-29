using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace YouSingStudio.MeshKit {
	public class MeshTopologySelector
		:MeshSelectorBase
	{
		#region Fields

		public Mesh mesh;
		public int[] indexes;

		[System.NonSerialized]public bool fastMode;
		[System.NonSerialized]public List<Vector2Int> submeshes=null;
		[System.NonSerialized]public List<HashSet<int>> shapes=new List<HashSet<int>>();

		#endregion Fields

		#region Methods

		public static HashSet<int> Get(List<HashSet<int>> shapes,int index) {
			int i,imax=shapes.Count,j;
			for(i=0;i<imax;++i) {
				if(shapes[i].Contains(index)) {return shapes[i];}
			}
			return null;
		}

		public static bool Intersect(HashSet<int> x,HashSet<int> y) {
			if(x!=null&&y!=null&&x!=y) {
			foreach(int it in y) {
				if(x.Contains(it)) {return true;}
			}}
			return false;
		}

		public static void Trim(List<HashSet<int>> shapes,IList<Vector2Int> submeshes=null) {
			int i,imax=shapes.Count,j;
			for(i=0;i<imax;++i) {
			for(j=0;j<imax;++j) {
				if(i!=j&&Intersect(shapes[i],shapes[j])) {
					foreach(int it in shapes[j]) {shapes[i].Add(it);}
					shapes[j]=null;
				}
			}}
			//
			imax=submeshes?.Count??0;
			if(imax>0) {
				int k,c=0,s=0;Vector2Int v;
				for(i=0,j=0;i<imax;++i) {
					v=submeshes[i];
						k=v.y;s+=c;c=0;
						while(k-->0) {if(shapes[j]==null) {++c;}++j;}
						v.x-=s;v.y-=c;
					submeshes[i]=v;
				}
			}
			//
			shapes.RemoveAll(x=>x==null);
		}

		public static void Add(List<HashSet<int>> shapes,int a,int b,int c) {
			int i,imax=shapes.Count;
			HashSet<int> it,shape=null;
			for(i=0;i<imax;++i) {
				it=shapes[i];
				if(it.Contains(a)||it.Contains(b)||it.Contains(c)) {
					shape=it;break;
				}
			}
			if(shape==null) {
				shape=new HashSet<int>();shapes.Add(shape);
			}
			shape.Add(a);shape.Add(b);shape.Add(c);
		}

		public static void Bake(List<HashSet<int>> shapes,IList<int> triangles,IList<Vector2Int> submeshes=null) {
			int i=0,imax=(triangles?.Count??0)/3;System.Action a=null;
			shapes.Clear();int j=-1,jmax=submeshes?.Count??0,k=int.MaxValue;
			if(jmax>0) {a=()=>{
				if(j>=0&&j<jmax) {
					var v=submeshes[j];
						v.y=shapes.Count-v.x;
					submeshes[j]=v;
				}
				++j;
				if(j>=0&&j<jmax) {
					var v=submeshes[j];
						k+=v.y/3;v.x=shapes.Count;
					submeshes[j]=v;
				}
			};k=0;a();}
			//
			for(;i<imax;++i) {
				if(i>=k) {a();}
				//
				Add(shapes
					,triangles[3*i+0]
					,triangles[3*i+1]
					,triangles[3*i+2]
				);
			}
			a?.Invoke();
			//
			int cnt;
			do{cnt=shapes.Count;Trim(shapes,submeshes);}
			while(cnt!=shapes.Count);
		}

		public override void BeginSelect(Mesh mesh) {
			if(submeshes!=null) {
				submeshes.Clear();int i=0,imax=mesh.subMeshCount;
				if(imax>1) {SubMeshDescriptor it;
				for(;i<imax;++i) {
					it=mesh.GetSubMesh(i);
					submeshes.Add(new Vector2Int(it.indexStart,it.indexCount));
				}}
			}
		}

		public override void EndSelect(Mesh mesh) {
		}

		public override List<int> SelectVertices(Mesh mesh,IList<int> triangles,List<int> result=null) {
			if(!fastMode) {Bake(shapes,triangles,submeshes);}
			if(result==null) {result=new List<int>(triangles.Count);}
			HashSet<int> it;for(int i=0,imax=indexes?.Length??0;i<imax;++i) {
				it=Get(shapes,indexes[i]);
				if(it!=null) {foreach(int n in it) {
					if(result.IndexOf(n)<0) {result.Add(n);}
				}}
			}
			return result;
		}

		public override List<int> SelectTriangles(Mesh mesh,IList<int> triangles,List<int> result=null) {
			return null;
		}

		public virtual int GetSubMesh(int index) {
			Vector2Int it;for(int i=0,imax=submeshes?.Count??0;i<imax;++i) {
				it=submeshes[i];
				if(index>=it.x&&index<it.x+it.y) {return i;}
			}
			return -1;
		}

		[ContextMenu("Run")]
		public virtual void Run() {
			if(mesh!=null) {
				BeginSelect(mesh);
					List<int> result=SelectVertices(mesh,mesh.triangles);
					if((submeshes?.Count??0)>0) {
						Debug.Log(name+":\n"+string.Join('\n',submeshes.ConvertAll(x=>$"{x.x}~{x.x+x.y-1}")));
					}
				EndSelect(mesh);
			}
		}

		#endregion Methods
	}
}
