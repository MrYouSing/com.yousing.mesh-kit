using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshPivotModifier
		:MeshVertexModifier
	{
		#region Fields

		public bool pose;
		public bool rename;
		public string path;

		#endregion Fields

		#region Methods

		public virtual string GetName() {
			return reference.name;
		}

		protected override Vector3 ModifyVertex(Vector3 vertex) {
			return (reference!=null)?reference.InverseTransformPoint(vertex):vertex;
		}

		public override void Run() {
			if(target!=null) {
				if(!string.IsNullOrEmpty(path)) {
					Transform t=target.parent;
					target.SetParent(target.FindEx(path));
					t.gameObject.Destroy();
				}
			}
			base.Run();
			if(target!=null) {
				if(pose&&reference!=null) {target.SetPositionAndRotation(reference.position,reference.rotation);}
				if(rename) {target.name=GetName();}
			}
		}

		#endregion Methods
	}
}
