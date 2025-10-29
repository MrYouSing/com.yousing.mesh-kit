using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class BoneWeightCopier
		:BoneWeightModifier
	{
		#region Fields

		public Mesh source;
		[System.NonSerialized]protected Vector3[] m_Src;
		[System.NonSerialized]protected Vector3[] m_Dst;
		[System.NonSerialized]protected BoneWeight[] m_Val;

		#endregion Fields

		#region Methods

		protected override BoneWeight[] BeginBoneWeight(Mesh mesh) {
			if(source!=null) {
				m_Src=source.vertices;m_Dst=mesh.vertices;
				m_Val=source.boneWeights;
				mesh.bindposes=source.bindposes;
			}
			return base.BeginBoneWeight(mesh);
		}

		protected override BoneWeight GetBoneWeight(int index) {
			if(source!=null) {
				Vector3 v=m_Dst[index];int idx=-1;
				float min=float.MaxValue,dis;
				for(int i=0,imax=m_Src?.Length??0;i<imax;++i) {
					dis=(m_Src[i]-v).sqrMagnitude;
					if(dis<min) {min=dis;idx=i;}
				}
				if(idx>=0) {index=idx;}
			}
			return m_Val[index];
		}

		protected override void EndBoneWeight(Mesh mesh,BoneWeight[] boneWeights) {
			m_Src=m_Dst=null;m_Val=null;
			base.EndBoneWeight(mesh,boneWeights);
		}

		#endregion Methods
	}
}
