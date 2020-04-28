using System.Collections.Generic;
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

		public static void ForEach(IList<int> triangles,System.Action<int> action) {
			if(triangles==null||action==null) {
				return;
			}
			//
			HashSet<int> hs=new HashSet<int>();
			int t;
			for(int i=0,imax=triangles.Count;i<imax;++i) {
				t=triangles[i];
				if(!hs.Contains(t)) {
					hs.Add(t);
					action(t);
				}
			}
		}

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

		protected virtual void SetTriangles(Mesh mesh,int[] triangles) {
			if(mesh!=null) {
				if(submesh>=0) {
					mesh.SetIndices(triangles,MeshTopology.Triangles,submesh);
				}else {
					mesh.triangles=triangles;
				}
			}
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
			if(mesh.name.IndexOf("(Clone)")<0) {
				mesh=Object.Instantiate(mesh);
			}}}
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
