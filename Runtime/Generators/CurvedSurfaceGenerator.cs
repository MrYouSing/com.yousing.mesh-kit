using UnityEngine;
using UnityEngine.Animations;

namespace YouSingStudio.MeshKit {

	[System.Obsolete]
	public class CurvedSurfaceGenerator:MeshGeneratorBase {

		#region Nested Types

		public enum SourceType {
			 None=-1
			,Ellipse
			,Count
		}

		#endregion Nested Types

		#region Fields

		public SourceType sourceType=SourceType.Ellipse;
		public int numSamples=2;

		public Vector3 center;
		public Vector3 radius=Vector3.one*.5f;
		public Axis zAxis=Axis.Y;
		public Vector4 range=Vector4.zero;
		public Quaternion rotation=Quaternion.identity;
		public Vector4 uvMatrix=new Vector4(1.0f,0.0f,0.0f,1.0f);

		#endregion Fields

		#region Methods

#if UNITY_EDITOR

		protected virtual bool SourceTypeIsEllipse() {
			return sourceType==SourceType.Ellipse;
		}

#endif

		public virtual void GetCurvePointsByEllipse(Vector3[] points,int index,int numSamples,out float length) {
			length=0.0f;
			//
			Vector3 point=Vector3.zero;
			Vector3 prevPoint=Vector3.zero;
			float deltaAngle=(range.z-range.x)/(numSamples-1);
			float angle=range.x;
			for(int i=0;i<numSamples;++i) {
				point.Set(Mathf.Sin(angle*Mathf.Deg2Rad)*radius.x,
					0.0f,Mathf.Cos(angle*Mathf.Deg2Rad)*radius.y);
				point=center+rotation*point;
				points[index]=point;
				if(i>0){length+=(point-prevPoint).magnitude;}
				//
				prevPoint=point;
				++index;
				angle+=deltaAngle;
			}
		}

		protected override void OnPreBuildMesh() {
			//
			if((m_Vertices?.Length??0)/2!=numSamples) {
				m_Vertices=new Vector3[numSamples*2];
			}
			if((m_UVs?.Length??0)/2!=numSamples) {
				m_UVs=new Vector2[numSamples*2];
			}
			if((m_Colors?.Length??0)/2!=numSamples) {
				m_Colors=new Color[numSamples*2];
			}
			if(((m_Triangles?.Length??0)/6)+1!=numSamples) {
				m_Triangles=new int[(numSamples-1)*6];
			}
			//
			float length;
			float halfHeight=radius.z;
			//
			switch(zAxis){
				case Axis.Z:{
					Vector3 radius=this.radius;
						this.radius=radius+Vector3.one*halfHeight;
						GetCurvePointsByEllipse(m_Vertices,0,numSamples,out length);
						this.radius=radius-Vector3.one*halfHeight;
						GetCurvePointsByEllipse(m_Vertices,numSamples,numSamples,out length);
					this.radius=radius;
				}break;
				default:
				case Axis.Y:{
					Vector3 center=this.center;
						this.center=center+rotation*(Vector3.up*halfHeight);
						GetCurvePointsByEllipse(m_Vertices,0,numSamples,out length);
						this.center=center+rotation*(Vector3.down*halfHeight);
						GetCurvePointsByEllipse(m_Vertices,numSamples,numSamples,out length);
					this.center=center;
				}break;
			}
			//
			length=1.0f/numSamples;
			for(int i=0;i<numSamples;++i) {
				m_UVs[i]=UnityExtension.Matrix22_Mul(uvMatrix,new Vector2(i*length,1.0f));
				m_UVs[numSamples+i]=UnityExtension.Matrix22_Mul(uvMatrix,new Vector2(i*length,0.0f));
				m_Colors[i]=Color.white;
				m_Colors[numSamples+i]=Color.white;
			}
			//
			for(int i=0,imax=numSamples-1,j=0;i<imax;++i) {
				m_Triangles[j++]=i;
				m_Triangles[j++]=i+1;
				m_Triangles[j++]=numSamples+i;
				m_Triangles[j++]=i+1;
				m_Triangles[j++]=numSamples+i+1;
				m_Triangles[j++]=numSamples+i;
			}
		}

		#endregion Methods

	}
}
