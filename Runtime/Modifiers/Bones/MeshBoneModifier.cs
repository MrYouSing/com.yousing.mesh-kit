using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshBoneModifier:MeshModifierBase {

		#region Fields

		public MeshSelectorBase selector;

		#endregion Fields

		#region Methods

		protected virtual void ModifyBone(ref BoneWeight value) {
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				List<int> selection=null;
				if(selector!=null) {
					List<int> triangles=new List<int>(GetTriangles(mesh));
					selection=selector.SelectVertices(mesh,triangles);
				}
				BoneWeight[] boneWeights=mesh.boneWeights;
				for(int i=0,imax=boneWeights?.Length??0;i<imax;++i) {
					if(selection==null||selection.IndexOf(i)>=0) {
						ModifyBone(ref boneWeights[i]);
					}
				}
				mesh.boneWeights=boneWeights;
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods

	}
}
