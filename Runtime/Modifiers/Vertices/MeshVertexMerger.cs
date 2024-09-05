using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshVertexMerger
		:MeshVertexModifier
	{
		#region Fields

		public float threshold=0.01f;
		public Mesh other;
		[System.NonSerialized]protected int m_Count;
		[System.NonSerialized]protected IList<Vector3> m_Destinations;

		#endregion Fields

		#region Methods

		protected virtual Vector3 Merge(Vector3 vertex,Vector3 destination,ref float sqrDis) {
			Vector3 v=(target!=null)?target.TransformPoint(vertex):vertex;
			Vector3 d=(reference!=null)?reference.TransformPoint(destination):destination;
			sqrDis=(v-d).sqrMagnitude;
			if(sqrDis<=threshold*threshold) {
				vertex=(target!=null)?target.InverseTransformPoint(d):d;
			}else {
				sqrDis=float.NaN;
			}
			return vertex;
		}

		protected override Vector3 ModifyVertex(Vector3 vertex) {
			int i=0,imax=m_Destinations?.Count??0;
			float sqrDis=0.0f,sqrMin=float.MaxValue;
			Vector3 best=vertex,tmp;
			for(;i<imax;++i) {
				tmp=Merge(vertex,m_Destinations[i],ref sqrDis);
				if(!float.IsNaN(sqrDis)) {
				if(sqrDis<sqrMin) {
					sqrMin=sqrDis;best=tmp;
				}}
			}
			if(sqrMin<float.MaxValue) {++m_Count;}
			return best;
		}

		public override void Run() {
			//
			if(other==null) {
				other=reference.GetInstancedMesh();
				if(other==null) {return;}
			}
			//
			m_Count=0;
			m_Destinations=other.vertices;
				base.Run();
			Debug.Log("Merge "+m_Count+" vertices.");
		}

		#endregion Methods
	}
}
