using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class BoneWeightPainter
		:BoneWeightModifier
	{
		#region Fields

		public int[] bones;
		public Texture2D texture;
		public Color color;
		[System.NonSerialized]protected Vector2[] m_UV;

		#endregion Fields

		#region Methods

		protected override BoneWeight[] BeginBoneWeight(Mesh mesh) {
			m_UV=mesh.uv;
			return base.BeginBoneWeight(mesh);
		}

		protected override BoneWeight GetBoneWeight(int index) {
			BoneWeight w=new BoneWeight();
			Vector2 uv=m_UV[index];int i,imax=bones?.Length??0;
			Color c=texture!=null?texture.GetPixelBilinear(uv.x,uv.y):color;
			float f=0.0f;for(i=0;i<imax;++i) {
				f+=c[i];
			}
			f=1.0f/f;for(i=0;i<imax;++i) {
				w.SetBoneIndex(i,bones[i]);
				w.SetWeight(i,c[i]*f);
			}
			return w;
		}

		#endregion Methods
	}
}