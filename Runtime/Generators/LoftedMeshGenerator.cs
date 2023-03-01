using System.Collections;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class LoftedMeshGenerator
		:MeshGeneratorBase
	{
		#region Nested Types

		[System.Flags]
		public enum Style {
			Loop=0x1,
			Reverse=0x2,
		}

		#endregion Nested Types

		#region Fields

		[Header("Shape")]
		[SerializeField]protected Object m_Curve;
		public ITransformCurve curve;

		public Matrix2D uvMatrix=Matrix2D.identity;
		public LineRenderer line;
		public Style style;
		public Vector3[] points=new Vector3[0];

		[Header("Sample")]
		public Vector2 clipTime=Vector2.up;
		public int numSamples=100;

		#endregion Fields

		#region Methods
#if UNITY_EDITOR
		[System.NonSerialized]protected Vector3[] m_Points;
		protected virtual void DrawPoints() {
			if(line==null) {
				for(int i=0,imax=(m_Points?.Length??0)-1;i<imax;++i) {
					Gizmos.DrawLine(m_Points[i],m_Points[i+1]);
				}
			}
		}

		protected override void InternalDrawGizmos(bool selected) {
			if(selected) {
				m_Points=GetPoints();
				curve=m_Curve as ITransformCurve;
				if(curve==null) {Gizmos.matrix=Matrix4x4.identity;DrawPoints();}
				else {
					Matrix4x4 m=Gizmos.matrix;
					float t=clipTime.x,dt=(clipTime.y-t)/(numSamples-1);
					for(int i=0,imax=numSamples;i<imax;++i) {
						Gizmos.matrix=curve.GetMatrix(t)*m;DrawPoints();
						t+=dt;
					}
				}
			}
			m_Points=null;
		}
#endif

		public virtual Vector3[] GetPoints() {
			Vector3[] pts=null;
			int num=points?.Length??0;
			if(line!=null) {
				pts=new Vector3[line.positionCount];
				line.GetPositions(pts);
			}else if((style&Style.Loop)!=0&&num>=3) {
				pts=new Vector3[num+1];
				System.Array.Copy(points,pts,num);pts[num]=pts[0];
			}else {
				pts=(Vector3[])points.Clone();
			}
			if((style&Style.Reverse)!=0) {System.Array.Reverse(pts);}
			return pts;
		}

		public virtual void SetPoints(IEnumerable value) {
			using(UnityEngine.Pool.ListPool<Vector3>.Get(out var list)) {
				if(value!=null) {
					foreach(var it in value) {list.Add((Vector3)it);}
				}
				points=list.ToArray();
			}
			points.Clockwise();
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		protected override void OnPreBuildMesh() {
			base.OnPreBuildMesh();
			//
			curve=m_Curve as ITransformCurve;
			if(curve==null) {
				return;
			}
			Vector3[] points=GetPoints();
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
