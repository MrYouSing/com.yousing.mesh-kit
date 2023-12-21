using System.Collections.Generic;
using UnityEngine;
namespace YouSingStudio.MeshKit {

	public abstract class BoneWeightModifier
		:MeshModifierBase
	{
		#region Fields

		[Header("BoneWeight")]
		public MeshSelectorBase selection;

		#endregion Fields

		#region Methods

		protected abstract BoneWeight GetBoneWeight(int index);

		protected virtual BoneWeight[] BeginBoneWeight(Mesh mesh) {
			return mesh!=null?mesh.boneWeights:null;
		}

		protected virtual void EndBoneWeight(Mesh mesh,BoneWeight[] boneWeights) {
			if(mesh!=null&&boneWeights!=null) {
				mesh.boneWeights=boneWeights;
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
			var boneWeights=BeginBoneWeight(mesh);
				IList<int> ids=mesh.triangles;
				if(selection!=null) {
					ids=selection.SelectVertices(mesh,ids);
				}
				for(int i=0,imax=ids?.Count??0;i<imax;++i) {
					boneWeights[ids[i]]=GetBoneWeight(ids[i]);
				}
			EndBoneWeight(mesh,boneWeights);
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}