using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class LoftedMeshGenerator
		:MeshGeneratorBase
	{
		#region Fields

		[Header("Shape")]
		[SerializeField]protected Object m_Curve;
		public ITransformCurve curve;

		public Matrix2D uvMatrix=Matrix2D.identity;
		public LineRenderer line;
		public Vector3[] points=new Vector3[0];

		[Header("Sample")]
		public Vector2 clipTime=Vector2.up;
		public int numSamples=100;

		#endregion Fields

		#region Methods

		protected override void OnPreBuildMesh() {
			base.OnPreBuildMesh();
			//
			curve=m_Curve as ITransformCurve;
			if(curve==null) {
				return;
			}
			if(line!=null) {
				points=new Vector3[line.positionCount];
				line.GetPositions(points);
			}
			if((points?.Length??0)==0) {
				return;
			}
			//
			int j,jmax=points.Length;
			j=numSamples*jmax;
			EnsureSize(ref m_Vertices,j);
			EnsureSize(ref m_UVs,j);
			j=(numSamples-1)*(jmax-1)*6;
			EnsureSize(ref m_Triangles,j);
			//
			float t=clipTime.x,dt=(clipTime.y-t)/(numSamples-1);
			Matrix4x4 m;
			int p=0,q=0;
			for(int i=0,imax=numSamples;i<imax;++i) {
				m=curve.GetMatrix(t);
				//
				for(j=0;j<jmax;++j) {
					m_Vertices[p]=m.MultiplyPoint3x4(points[j]);
					m_UVs[p]=uvMatrix.MultiplyPoint(
						new Vector2((float)j/(jmax-1),i/(float)(imax-1)));
					++p;
					if(i>0) {
					if(j>0) {
						m_Triangles[q++]=jmax*(i-1)+(j-1);// 0
						m_Triangles[q++]=jmax*(i-1)+(j+0);// 1
						m_Triangles[q++]=jmax*(i+0)+(j-1);// 2
							
						m_Triangles[q++]=jmax*(i-1)+(j+0);// 1
						m_Triangles[q++]=jmax*(i+0)+(j+0);// 3
						m_Triangles[q++]=jmax*(i+0)+(j-1);// 2
					}}
				}
				//
				t+=dt;
			}
		}

		#endregion Methods
	}
}
