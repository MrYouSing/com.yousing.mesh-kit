using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshUvSelector
		:MeshSelectorBase
	{
		#region Fields

		public Texture2D texture;
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
				return texture.GetPixel(
					(int)(texture.width*Mathf.Repeat(v.x,1.0f)),
					(int)(texture.height*Mathf.Repeat(v.y,1.0f))
				);
			}
			return Color.black;
		}

		public override bool TestIndex(int index) {
			return GetColor(uv[index]).a>cutoff;
		}

		#endregion Methods
	}
}
