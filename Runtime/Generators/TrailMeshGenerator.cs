using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class TrailMeshGenerator
		:MeshGeneratorBase
	{
		#region Fields

		public float threshold=-1.0f;
		public List<Vector3Set> frames;

		[System.NonSerialized]protected float m_SqrThreshold;

		#endregion Fields

		#region Methods

		protected virtual void AddFrame(ref int index,Vector3Set frame,float texcoord) {
			IList<Vector3> points=(frame!=null)?frame.GetItems():null;
			int i=0,imax=points?.Count??0;
			float dv=1.0f/(imax-1);
			for(;i<imax;++i,++index) {
				m_Vertices[index]=points[i];
				m_UVs[index]=new Vector2(texcoord,i*dv);
			}
		}

		protected virtual bool IsTriangle(Vector3 v0,Vector3 v1,Vector3 v2) {
			return (v1-v0).sqrMagnitude>=m_SqrThreshold
				&&(v2-v0).sqrMagnitude>=m_SqrThreshold
				&&(v2-v1).sqrMagnitude>=m_SqrThreshold;
		}

		protected virtual void AddTriangle(ref int index,int t0,int t1,int t2) {
			if(threshold<0.0f||IsTriangle(m_Vertices[t0],m_Vertices[t1],m_Vertices[t2])) {
				m_Triangles[index++]=t0;
				m_Triangles[index++]=t1;
				m_Triangles[index++]=t2;
			}
		}

		protected virtual void AddStrip(ref int index,int lhs,int rhs) {
			for(int i=0,imax=Mathf.Abs(rhs-lhs)-1;i<imax;++i,++lhs,++rhs) {
				AddTriangle(ref index,lhs,rhs,lhs+1);
				AddTriangle(ref index,lhs+1,rhs,rhs+1);
			}
		}

		protected override void OnPreBuildMesh() {
			int i=0,imax=frames?.Count??0;
			if(imax>=2) {
				int jmax=frames[i].GetItems().Count;
				if(jmax>=2) {
					m_SqrThreshold=threshold*threshold;
					EnsureSize(ref m_Triangles,(imax-1)*(jmax-1)*6);
					float du=1.0f/(imax-1);jmax*=imax;
					EnsureSize(ref m_Vertices,jmax);
					EnsureSize(ref m_UVs,jmax);
					int o,p=0,q=0,t=0;
					for(;i<imax;++i) {
						o=p;AddFrame(ref p,frames[i],i*du);
						if(i>0) {AddStrip(ref t,q,o);q=o;}
					}
					System.Array.Resize(ref m_Triangles,t);
				}
			}
		}

		#endregion Methods
	}
}
