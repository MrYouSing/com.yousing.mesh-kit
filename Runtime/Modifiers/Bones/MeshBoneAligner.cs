using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public class MeshBoneAligner
		:MonoTask
	{
		#region Fields

		public int mask=3;
		public Transform[] sources;
		public Transform[] destinations;
		public SkinnedMeshRenderer[] renderers;

		#endregion Fields

		#region Methods

		public static void Flush(SkinnedMeshRenderer renderer,Mesh mesh) {
			if(renderer==null||mesh==null) {return;}
			//
			Transform root=renderer.transform;Transform[] bones=renderer.bones;
			Matrix4x4 m=root!=null?root.localToWorldMatrix:Matrix4x4.identity;
			int i=0,imax=bones?.Length??0;Matrix4x4[] bindposes=new Matrix4x4[imax];
			for(;i<imax;++i) {
				bindposes[i]=bones[i].worldToLocalMatrix*m;
			}
			mesh.bindposes=bindposes;
		}

		public static void Align(Transform src,Transform dst,int mask) {
			if(src!=null) {if(dst!=null) {dst.SetParent(null,true);}
			using(ListPool<Transform>.Get(out var list)) {
				int i,imax;Transform it;
				for(i=0,imax=src.childCount;i<imax;++i) {
					it=src.GetChild(0);
					if(it!=null) {it.SetParent(null,true);list.Add(it);}
				}
				if((mask&0x1)!=0) {src.position=(dst!=null)?dst.position:Vector3.zero;}
				if((mask&0x2)!=0) {src.rotation=(dst!=null)?dst.rotation:Quaternion.identity;}
				for(i=0,imax=list.Count;i<imax;++i) {
					list[i].SetParent(src,true);
				}
			}}
		}

		public virtual void Align(SkinnedMeshRenderer renderer) {
			if(renderer==null) {return;}
			//
			Mesh mesh=Mesh.Instantiate(renderer.sharedMesh);
			Flush(renderer,mesh);renderer.sharedMesh=mesh;
		}

		public override void Run() {
			int i,imax;
			for(i=0,imax=sources?.Length??0;i<imax;++i) {Align(sources[i],destinations[i],mask);}
			for(i=0,imax=renderers?.Length??0;i<imax;++i) {Align(renderers[i]);}
		}

		#endregion Methods
	}
}
