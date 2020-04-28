using UnityEngine;

namespace YouSingStudio.MeshKit {
	public partial class MeshVertexSnapper
		:MeshVertexModifier_ByNormal
	{
		#region Fields

		public float origin=0.0f;
		public float distance=-1.0f;
		public float offset=0.0f;

		#endregion Fields

		#region Methods

		protected override void ModifyVertex(int i) {
			Vector3 v=m_Vertices[i];
			Vector3 n=m_Normals[i];
			RaycastHit hit;
			if(Physics.Raycast(
				v+n*origin,
				n*Mathf.Sign(distance),
				out hit,
				Mathf.Abs(distance))
			) {
				m_Vertices[i]=hit.point+hit.normal*offset;
			}
		}

		#endregion Methods
	}
}