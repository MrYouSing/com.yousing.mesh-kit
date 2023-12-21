using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshTopologySelector
		:MeshSelectorBase
	{
		#region Fields

		public int[] indexes;
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

		public static void Trim(List<HashSet<int>> shapes) {
			int i,imax=shapes.Count,j;
			for(i=0;i<imax;++i) {
			for(j=0;j<imax;++j) {
				if(i!=j&&Intersect(shapes[i],shapes[j])) {
					foreach(int it in shapes[j]) {shapes[i].Add(it);}
					shapes[j]=null;
				}
			}}
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

		public static void Bake(List<HashSet<int>> shapes,IList<int> triangles) {
			shapes.Clear();
			for(int i=0,imax=(triangles?.Count??0)/3;i<imax;++i) {
				Add(shapes
					,triangles[3*i+0]
					,triangles[3*i+1]
					,triangles[3*i+2]
				);
			}
			int cnt;
			do{cnt=shapes.Count;Trim(shapes);}
			while(cnt==shapes.Count);
		}

		public override List<int> SelectVertices(Mesh mesh,IList<int> triangles,List<int> result = null) {
			Bake(shapes,triangles);
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

		public Mesh mesh;
		[ContextMenu("Run")]
		public virtual void Run() {
			List<int> result=SelectVertices(mesh,mesh.triangles);
		}

		#endregion Methods
	}
}
