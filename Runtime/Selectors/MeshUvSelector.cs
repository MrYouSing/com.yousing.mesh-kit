using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshUvSelector
		:MeshSelectorBase
	{
		#region Fields

		public bool strict;
		public Texture2D texture;
		public bool bilinear=true;
		public int channel=3;
		[Range(0.0f,1.0f)]
		public float cutoff=0.5f;
		[System.NonSerialized]public Vector2[] uv;

		#endregion Fields

		#region Methods

		public override void BeginSelect(Mesh mesh) {
			base.BeginSelect(mesh);
			//
			uv=mesh.uv;
		}

		public override void EndSelect(Mesh mesh) {
			base.EndSelect(mesh);
			//
			uv=null;
		}

		public virtual Color GetColor(Vector2 v) {
			if(texture!=null) {
				if(bilinear) {
					return texture.GetPixelBilinear(v.x,v.y);
				}else {
					return texture.GetPixel(
						(int)(texture.width*Mathf.Repeat(v.x,1.0f)),
						(int)(texture.height*Mathf.Repeat(v.y,1.0f))
					);
				}
			}
			return Color.black;
		}

		public override bool TestIndex(int index) {
			return GetColor(uv[index])[channel]>cutoff;
		}

		public override bool TestTriangle(int a, int b, int c) {
			bool ret=base.TestTriangle(a,b,c);
			if(strict&&!ret) {ret=GetColor((uv[a]+uv[b]+uv[c])/3.0f)[channel]>cutoff;}
			return ret;
		}

		#endregion Methods
	}
}
