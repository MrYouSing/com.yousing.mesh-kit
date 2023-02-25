using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public class MeshBoneFiller
		:MeshBoneModifier
	{
		#region Fields

		public Transform[] bones;
		public int[] indexes;
		[Range(0.0f,1.0f)]
		public float[] weights;
		[System.NonSerialized]protected BoneWeight m_BoneWeight;
		[System.NonSerialized]protected List<Matrix4x4> m_BindPoses;

		#endregion Fields

		#region Methods

		public virtual BoneWeight GetBoneWeight() {
			BoneWeight boneWeight=new BoneWeight();
			SkinnedMeshRenderer smr=null;Transform t,p;
			if(target!=null) {smr=target.GetComponent<SkinnedMeshRenderer>();}
			int i=0,imax=Mathf.Max(bones?.Length??0,indexes?.Length??0);
			float sum=weights.Sum();bool b=false;
			using(ListPool<Transform>.Get(out var list)) {
				if(smr!=null) {
					list.AddRange(smr.bones);
					p=smr.rootBone;if(p==null) {p=target;}
					for(i=0;i<imax;++i) {
						t=bones[i];if(t!=null&&list.IndexOf(t)<0) {
							list.Add(t);b=true;
							if(m_BindPoses==null) {m_BindPoses=new List<Matrix4x4>();}
							m_BindPoses.Add(t.worldToLocalMatrix*p.localToWorldMatrix);
						}
					}
				}
				for(i=0;i<imax;++i) {
					if(smr!=null) {boneWeight.SetBoneIndex(i,list.IndexOf(bones[i]));}
					else {boneWeight.SetBoneIndex(i,indexes[i]);}
					boneWeight.SetWeight(i,weights[i]/sum);
				}
				if(b) {
					smr.bones=list.ToArray();
#if UNITY_EDITOR
					UnityEditor.EditorUtility.SetDirty(smr);
#endif
				}
			}
			return boneWeight;
		}
		protected override void ModifyBone(ref BoneWeight value) {
			value=m_BoneWeight;
		}

		public override void Run() {
			m_BoneWeight=GetBoneWeight();
			base.Run();
		}

		protected override void EndModifyMesh(Mesh mesh) {
			if(m_BindPoses!=null) {
				mesh.bindposes=mesh.bindposes.Concat(m_BindPoses).ToArray();
			}
			m_BindPoses=null;
			base.EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
