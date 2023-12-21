using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class BoneWeightFiller
		:BoneWeightModifier
	{
		#region Fields
		
		public int[] bones;
		[Range(0.0f,1.0f)]public float[] weights;
		[System.NonSerialized]protected BoneWeight m_BoneWeight;

		#endregion Fields

		#region Methods

		protected override BoneWeight[] BeginBoneWeight(Mesh mesh) {
			m_BoneWeight=new BoneWeight();
			for(int i=0,imax=bones?.Length??0;i<imax;++i) {
				m_BoneWeight.SetBoneIndex(i,bones[i]);
				m_BoneWeight.SetWeight(i,weights[i]);
			}
			return base.BeginBoneWeight(mesh);
		}

		protected override BoneWeight GetBoneWeight(int index) {
			return m_BoneWeight;
		}

		#endregion Methods
	}
}