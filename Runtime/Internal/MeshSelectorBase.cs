/* <!-- Macro.Table Test
int,Index,
Vector3,Vertex,
 Macro.End --> */
/* <!-- Macro.Call  Test
		public virtual bool Test{1}({0} x)=>false;

		public virtual bool TestTriangle({0} a,{0} b,{0} c) {{
			int i=TestFunc(Test{1},a,b,c);
			return triangular?(i==3):(i>=1);
		}}

 Macro.End --> */
/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */

/* <!-- Macro.Copy
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0,j=0,a,b,c;
				if(result==null&&imax>0) {result=new List<int>(imax);}
				for(imax/=3;i<imax;++i) {
					a=triangles[j];++j;
					b=triangles[j];++j;
					c=triangles[j];++j;
					if(TestTriangle(a,b,c)) {
 Macro.End --> */
/* <!-- Macro.Patch
,BeginSelect
 Macro.End --> */
/* <!-- Macro.Copy
					}
				}
			EndSelect(mesh);
			return result;
 Macro.End --> */
/* <!-- Macro.Patch
,EndSelect
 Macro.End --> */
using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshSelectorBase
		:MonoBehaviour
	{
// <!-- Macro.Patch AutoGen
		public virtual bool TestIndex(int x)=>false;

		public virtual bool TestTriangle(int a,int b,int c) {
			int i=TestFunc(TestIndex,a,b,c);
			return triangular?(i==3):(i>=1);
		}

		public virtual bool TestVertex(Vector3 x)=>false;

		public virtual bool TestTriangle(Vector3 a,Vector3 b,Vector3 c) {
			int i=TestFunc(TestVertex,a,b,c);
			return triangular?(i==3):(i>=1);
		}

// Macro.Patch -->
		#region Fields

		[UnityEngine.Serialization.FormerlySerializedAs("all")]
		public bool triangular;

		#endregion Fields

		#region Methods

		public static int TestFunc<T>(System.Func<T,bool> func,T a,T b,T c) {
			int n=0;
			if(func!=null) {
				if(func(a)) {++n;}if(func(b)) {++n;}if(func(c)) {++n;}
			}
			return n;
		}

		public virtual void BeginSelect(Mesh mesh) {
		}

		public virtual void EndSelect(Mesh mesh) {
		}

		public virtual List<int> SelectTriangles(Mesh mesh,IList<int> triangles,List<int> result=null) {
// <!-- Macro.Patch BeginSelect
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0,j=0,a,b,c;
				if(result==null&&imax>0) {result=new List<int>(imax);}
				for(imax/=3;i<imax;++i) {
					a=triangles[j];++j;
					b=triangles[j];++j;
					c=triangles[j];++j;
					if(TestTriangle(a,b,c)) {
// Macro.Patch -->
						result.Add(3*i);
// <!-- Macro.Patch EndSelect
					}
				}
			EndSelect(mesh);
			return result;
// Macro.Patch -->
		}

		public virtual List<int> SelectVertices(Mesh mesh,IList<int> triangles,List<int> result=null) {
// <!-- Macro.Patch BeginSelect
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0,j=0,a,b,c;
				if(result==null&&imax>0) {result=new List<int>(imax);}
				for(imax/=3;i<imax;++i) {
					a=triangles[j];++j;
					b=triangles[j];++j;
					c=triangles[j];++j;
					if(TestTriangle(a,b,c)) {
// Macro.Patch -->
						result.Add(a);result.Add(b);result.Add(c);
// <!-- Macro.Patch EndSelect
					}
				}
			EndSelect(mesh);
			return result;
// Macro.Patch -->
		}

		public virtual List<int> SelectVertices(Mesh mesh,IList<int> triangles,bool[] opened,List<int> result=null) {
// <!-- Macro.Patch BeginSelect
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0,j=0,a,b,c;
				if(result==null&&imax>0) {result=new List<int>(imax);}
				for(imax/=3;i<imax;++i) {
					a=triangles[j];++j;
					b=triangles[j];++j;
					c=triangles[j];++j;
					if(TestTriangle(a,b,c)) {
// Macro.Patch -->
						result.Add(a);result.Add(b);result.Add(c);
						opened[i]=true;
// <!-- Macro.Patch EndSelect
					}
				}
			EndSelect(mesh);
			return result;
// Macro.Patch -->
		}

		#endregion Methods
	}
}
