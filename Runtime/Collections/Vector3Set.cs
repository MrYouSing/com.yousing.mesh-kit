using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class Vector3Set
		:StructSet<Vector3>
	{
		#region Nested Types
#if UNITY_EDITOR
		[UnityEditor.CustomEditor(typeof(Vector3Set),true)]
		public class Vector3Editor:StructEditor{}
#endif
		#endregion Nested Types

		#region Methods

#if UNITY_EDITOR
		public override bool OnSceneGUI(int index,ref Vector3 item) {
			Vector3Editor.current.DrawLabel(item,name+"["+index+"]");
			Vector3 old=item;
				item=UnityEditor.Handles.DoPositionHandle(item,Quaternion.identity);
			return item!=old;
		}
#endif

		public override Vector3 Multiply(Vector3 item) {
			return m_Matrix.MultiplyPoint3x4(item);
		}

		#endregion Methods
	}
}
