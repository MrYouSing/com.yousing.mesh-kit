using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshSkinnerBase
		:MeshModifierBase
	{
		#region Fields

		public static List<Transform> s_TmpBones=new List<Transform>(4);
		public static Dictionary<Transform,Collider[]> s_TmpColliders=new Dictionary<Transform,Collider[]>();

		public Transform rootBone;
		public Transform[] bones=new Transform[0];

		#endregion Fields

		#region Methods
		
		protected virtual bool ContainVertex(Transform t,Vector3 v) {
			if(t!=null) {
				Collider[] colliders;
				if(s_TmpColliders.TryGetValue(t,out colliders)
					&&colliders!=null&&colliders.Length>0
				) {
					foreach(Collider c in colliders) {
						if(c.OverlapPoint(v)) {
							return true;
						}
					}
				}
			}
			return false;
		}

		protected virtual void CalculateWeight(ref BoneWeight weight,List<Transform> bones) {
			int i=0,imax=Mathf.Min(bones.Count,4);
			float f=1.0f/imax;
			for(;i<imax;++i){
				BoneWeightHelper.SetBoneIndex(ref weight,i,System.Array.IndexOf(this.bones,bones[i]));
				BoneWeightHelper.SetWeight(ref weight,i,f);
			}
			for(;i<4;++i){
				BoneWeightHelper.SetBoneIndex(ref weight,i,0);
				BoneWeightHelper.SetWeight(ref weight,i,0.0f);
			}
		}

		protected virtual void BakeWeight(ref BoneWeight weight,Mesh mesh,int idx) {
			Vector3 v=mesh.vertices[idx];
			s_TmpBones.Clear();
			Transform it;
			for(int i=0,imax=bones.Length;i<imax;++i) {
				it=bones[i];
				if(ContainVertex(it,v)) {
					s_TmpBones.Add(it);
				}
			}
			//
			CalculateWeight(ref weight,s_TmpBones);
		}
		
		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			int i,imax;
			s_TmpColliders.Clear();
			//
			imax=bones.Length;
			Matrix4x4 m=((rootBone!=null)?rootBone:transform).localToWorldMatrix;
			Matrix4x4[] bindposes=new Matrix4x4[imax];
			Transform it;
			for(i=0;i<imax;++i) {
				it=bones[i];
				if(it!=null) {
					bindposes[i]=it.worldToLocalMatrix*m;
					s_TmpColliders[it]=it.GetComponentsInChildren<Collider>();
				}else {
					bindposes[i]=m;
				}
			}
			mesh.bindposes=bindposes;
			//
			imax=mesh.vertexCount;
			BoneWeight[] boneWeights=new BoneWeight[imax];
			for(i=0;i<imax;++i) {
				BakeWeight(ref boneWeights[i],mesh,i);
			}
			mesh.boneWeights=boneWeights;
			//
			EndModifyMesh(mesh);
		}

		protected override void EndModifyMesh(Mesh mesh) {
			if(mesh!=null) {
			if(autoApply) {
			if(target!=null) {
				SkinnedMeshRenderer smr=target.GetComponent<SkinnedMeshRenderer>();
				if(smr!=null) {
					smr.rootBone=rootBone;
					smr.bones=bones;
					smr.sharedMesh=mesh;
				}
			}}}
			//
			base.EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
