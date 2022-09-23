/* <!-- Macro.Define Declare
		[System.NonSerialized]public IList<{0}> {1}s;
		public static {0} Get{2}(Pose pose)=>pose.{1};
		IEnumerator<{0}> IEnumerable<{0}>.GetEnumerator()
			=>GetItems(ref {1}s,Get{2})?.GetEnumerator()??null;

 Macro.End --> */
/* <!-- Macro.Call Declare
Vector3,position,Position,
Quaternion,rotation,Rotation,
 Macro.End --> */
/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */
using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class PoseSet
		:StructSet<Pose>
		,IEnumerable<Vector3>
		,IEnumerable<Quaternion>
	{
// <!-- Macro.Patch AutoGen
		[System.NonSerialized]public IList<Vector3> positions;
		public static Vector3 GetPosition(Pose pose)=>pose.position;
		IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator()
			=>GetItems(ref positions,GetPosition)?.GetEnumerator()??null;

		[System.NonSerialized]public IList<Quaternion> rotations;
		public static Quaternion GetRotation(Pose pose)=>pose.rotation;
		IEnumerator<Quaternion> IEnumerable<Quaternion>.GetEnumerator()
			=>GetItems(ref rotations,GetRotation)?.GetEnumerator()??null;

// Macro.Patch -->
		#region Nested Types
#if UNITY_EDITOR
		[UnityEditor.CustomEditor(typeof(PoseSet),true)]
		public class PoseEditor:StructEditor{}
#endif
		#endregion Nested Types

		#region Methods

#if UNITY_EDITOR
		public override bool OnSceneGUI(int index,ref Pose item) {
			PoseEditor.current.DrawLabel(item.position,name+"["+index+"]");
			Pose old=item;
				item.position=UnityEditor.Handles.DoPositionHandle(item.position,Quaternion.identity);
			return item!=old;
		}
#endif

		public override Pose Multiply(Pose item) {
			return new Pose(
				m_Matrix.MultiplyPoint3x4(item.position),
				m_Matrix.rotation*item.rotation
			);
		}

		public virtual IList<T> GetItems<T>(ref IList<T> list,System.Func<Pose,T> func) {
			var poses=GetItems();int i=0,imax=poses?.Count??0;
			if(imax>0) {
				Fill(ref list,poses,func);
				return list;
			}
			return null;
		}

		#endregion Methods
	}
}
