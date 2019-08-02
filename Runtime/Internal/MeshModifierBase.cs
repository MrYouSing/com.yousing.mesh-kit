using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshModifierBase:MonoTask {

		#region Fields

		[Header("Mesh Modifier")]
		public Transform target;
		public Mesh mesh;
		public int submesh=-1;
		public bool useClone=true;
		public bool autoApply=true;
		public MeshEvent onApply=new MeshEvent();

		#endregion Fields

		#region Methods

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			EndModifyMesh(mesh);
		}

		protected virtual int[] GetTriangles(Mesh mesh) {
			if(mesh!=null) {
				return (submesh>=0)
					?mesh.GetIndices(submesh)
					:mesh.triangles;
			}
			return null;
		}

		protected virtual Mesh BeginModifyMesh() {
			Mesh mesh=this.mesh;
			//
			if(mesh==null) {
			if(target!=null) {
				mesh=target.GetSharedMesh();
			}}
			//
			if(mesh!=null) {
			if(useClone) {
				Mesh copy=Object.Instantiate(mesh);
				mesh=copy;
			}}
			//
			return mesh;
		}

		protected virtual void EndModifyMesh(Mesh mesh) {
			if(mesh!=null) {
				if(autoApply) {
				if(target!=null) {
					target.SetSharedMesh(mesh);
				}}
				onApply?.Invoke(mesh);
			}
		}

		#endregion Methods

	}
}
