using UnityEngine;
namespace YouSingStudio.MeshKit {

	public class CircularSectorGenerator
		:MeshGeneratorBase
	{
		#region Fields
		
		public Vector3 center;
		public Vector3 normal=Vector3.up;
		public Vector3 dimensions=new Vector3(0.0f,90.0f,1.0f);
		public int segments=10;

		#endregion Fields

		#region Methods

		public virtual void Set(int i,Vector3 pos,Vector2 uv) {
			m_Triangles[i]=i;
			m_Vertices[i]=pos;
			m_UVs[i]=uv;
		}

		protected override void OnPreBuildMesh() {
			int n=segments*3;
			this.EnsureSize(ref m_Vertices,n);
			this.EnsureSize(ref m_UVs,n);
			this.EnsureSize(ref m_Triangles,n);
			//
			float du=1.0f/segments;
			float da=(dimensions.y-dimensions.x)/segments;
			Vector3 fwd=Vector3.forward*dimensions.z;
			Quaternion q=Quaternion.AngleAxis(dimensions.x,normal);
			n=0;
			for(int i=0;i<segments;++i) {
				Set(n,center,new Vector2((i+0.5f)*du,0.0f));++n;
				Set(n,center+q*fwd,new Vector2((i+0.0f)*du,1.0f));++n;
				q=Quaternion.AngleAxis(dimensions.x+(i+1.0f)*da,normal);
				Set(n,center+q*fwd,new Vector2((i+1.0f)*du,1.0f));++n;
			}
			//
			base.OnPreBuildMesh();
		}

		#endregion Methods
	}
}