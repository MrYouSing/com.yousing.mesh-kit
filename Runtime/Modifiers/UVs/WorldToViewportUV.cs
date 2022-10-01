using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class WorldToViewportUV
		:MeshModifierBase
	{
		#region Fields

		public new Camera camera;
		public Transform model;
		/// <summary>
		/// <seealso cref="SpriteRenderer.flipY"/>
		/// </summary>
		public bool flipY;

		#endregion Fields

		#region Methods

		protected virtual Vector2 Calculate(Vector3 position) {
			Vector2 uv=camera.WorldToViewportPoint(position);
			if(flipY) {uv.y=1.0f-uv.y;}
			return uv;
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(camera==null) {camera=Camera.main;}
			if(camera!=null&&mesh!=null) {
				Matrix4x4 m=model!=null?model.localToWorldMatrix:Matrix4x4.identity;
				Vector3[] vertices=mesh.vertices;
				Vector2[] uv=mesh.uv;
				for(int i=0,imax=vertices.Length;i<imax;++i) {
					uv[i]=Calculate(m.MultiplyPoint(vertices[i]));
				}
				mesh.vertices=vertices;
				mesh.uv=uv;
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
