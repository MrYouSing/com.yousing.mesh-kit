using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshSelectorBase
		:MonoBehaviour
	{
		#region Fields
		#endregion Fields

		#region Methods

		public virtual void BeginSelect(Mesh mesh) {
		}

		public virtual void EndSelect(Mesh mesh) {
		}

		public virtual bool TestIndex(int index)=>false;

		public virtual bool TestTriangle(int a,int b,int c) {
			return TestIndex(a)||TestIndex(b)||TestIndex(c);
		}

		public virtual List<int> SelectTriangles(Mesh mesh,IList<int> triangles,List<int> result=null) {
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0;
				int a,b,c;
				if(result==null) {
					result=new List<int>(imax);
				}
				for(imax/=3;i<imax;++i) {
					a=triangles[3*i+0];
					b=triangles[3*i+1];
					c=triangles[3*i+2];
					if(TestTriangle(a,b,c)) {
						result.Add(3*i);
					}
				}
			EndSelect(mesh);
			return result;
		}

		public virtual List<int> SelectVertices(Mesh mesh,IList<int> triangles,List<int> result=null) {
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0;
				int a,b,c;
				if(result==null) {
					result=new List<int>(imax);
				}
				for(imax/=3;i<imax;++i) {
					a=triangles[3*i+0];
					b=triangles[3*i+1];
					c=triangles[3*i+2];
					if(TestTriangle(a,b,c)) {
						result.Add(a);result.Add(b);result.Add(c);
					}
				}
			EndSelect(mesh);
			return result;
		}

		#endregion Methods
	}
}
