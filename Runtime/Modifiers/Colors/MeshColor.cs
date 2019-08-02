using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshColor:MeshModifierBase {

		#region Fields

		public Color color=Color.white;

		#endregion Fields

		#region Methods

		public override void Run() {
			ApplyColor(color);
		}

		public virtual void ApplyColor(Color c) {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				//
				int i=mesh.vertexCount;
				Color[] colors=new Color[i];
				while(i-->0) {
					colors[i]=c;
				}
				mesh.colors=colors;
			}
			EndModifyMesh(mesh);
		}

		public virtual void ApplyColor(int i) {
			ApplyColor(i.ToColor());
		}

		#endregion Methods

	}
}
