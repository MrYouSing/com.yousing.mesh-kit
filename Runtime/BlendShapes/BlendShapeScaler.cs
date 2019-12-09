using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class BlendShapeScaler
		:BlendShapeModifierBase
	{
		#region Nested Types

		[System.Serializable]
		public class Entry {
			public Transform transform;
			public float scale;
		}

		#endregion Nested Types

		#region Fields

		public Entry[] entries=new Entry[0];

		#endregion Fields

		#region Methods

		protected virtual bool InBox(Transform t,Vector3 v) {
			if(t!=null) {
				v=t.InverseTransformPoint(v);
				return v.x>=-0.5f&&v.x<= 0.5f
					&&v.y>=-0.5f&&v.y<= 0.5f
					&&v.z>=-0.5f&&v.z<= 0.5f;
			}
			return false;
		}

		protected override void OnModifyBlendShape(BlendShapeFrame raw,BlendShapeFrame src,BlendShapeFrame dst) {
			float f;
			for(int i=0,imax=raw.vertices.Length,j,jmax=entries?.Length??0;i<imax;++i) {
				f=1.0f;
				for(j=0;j<jmax;++j) {
					if(InBox(entries[j].transform,raw.vertices[i])) {
						f=entries[j].scale;
						break;
					}
				}
				//
				dst.vertices[i]=src.vertices[i]*f;
				dst.normals[i]=src.normals[i]*f;
				dst.tangents[i]=src.tangents[i]*f;
			}
		}

		#endregion Methods
	}
}
