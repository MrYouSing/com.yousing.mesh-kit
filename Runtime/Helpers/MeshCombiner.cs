using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshCombiner:MonoTask {

		#region Statics

		public static List<Transform> s_Transforms=new List<Transform>();
		public static List<CombineInstance> s_CombineInstances=new List<CombineInstance>();

		public static List<CombineInstance> BeginCombineMeshes() {
			return s_CombineInstances;
		}

		public static void DoCombineMeshes(List<CombineInstance> combines,Mesh mesh,Matrix4x4 transform) {
			for(int i=0,imax=mesh.subMeshCount;i<imax;++i) {
				combines.Add(new CombineInstance{
					mesh=mesh,
					subMeshIndex=i,
					transform=transform,
				});
			}
		}

		public static Mesh EndCombineMeshes(List<CombineInstance> combines) {
			Mesh mesh=new Mesh();
			mesh.CombineMeshes(combines.ToArray());
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			//
			s_CombineInstances.Clear();
			return mesh;
		}

		// References
		// 1) https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
		public static Mesh CombineMeshes(IList<MeshFilter> meshFilters,Matrix4x4 worldToLocalMatrix) {
			var combines=BeginCombineMeshes();
			MeshFilter mf;
			Renderer r;
			for(int i=0,imax=meshFilters?.Count??0;i<imax;++i) {
				mf=meshFilters[i];
				r=(mf==null)?null:mf.GetComponent<Renderer>();
				if(mf!=null&&(r==null||r.IsActiveAndEnabled())) {
					DoCombineMeshes(
						combines,mf.sharedMesh,
						worldToLocalMatrix*mf.transform.localToWorldMatrix
					);
				}
			}
			return EndCombineMeshes(combines);
		}

		// References
		// 1) http://www.oschina.net/code/snippet_2272150_45935
		public static Mesh CombineMeshes(IList<SkinnedMeshRenderer> skinnedMeshRenderers,Matrix4x4 worldToLocalMatrix,out Transform[] bones) {
			var combines=BeginCombineMeshes();
			var list=s_Transforms;
			//
			SkinnedMeshRenderer smr;
			for(int i=0,imax=skinnedMeshRenderers?.Count??0;i<imax;++i) {
				smr=skinnedMeshRenderers[i];
				if(smr!=null&&smr.IsActiveAndEnabled()) {
					DoCombineMeshes(
						combines,smr.sharedMesh,
						worldToLocalMatrix*smr.transform.localToWorldMatrix
					);
					list.AddRange(smr.bones);
				}
			}
			bones=list.ToArray();
			//
			s_Transforms.Clear();
			return EndCombineMeshes(combines);
		}

		public static void CombineTo(IList<MeshFilter> meshFilters,MeshFilter meshFilter) {
			Mesh mesh=CombineMeshes(meshFilters,meshFilter.transform.worldToLocalMatrix);
			mesh.name=meshFilter.name;
			meshFilter.sharedMesh=mesh;
		}

		public static void CombineTo(IList<SkinnedMeshRenderer> skinnedMeshRenderers,SkinnedMeshRenderer skinnedMeshRenderer) {
			Transform[] bones;
			Mesh mesh=CombineMeshes(skinnedMeshRenderers,skinnedMeshRenderer.transform.worldToLocalMatrix,out bones);
			mesh.name=skinnedMeshRenderer.name;
			skinnedMeshRenderer.sharedMesh=mesh;
			skinnedMeshRenderer.bones=bones;
		}

		#endregion Statics

		#region Fields

		public MeshFilter meshFilter;
		public MeshFilter[] meshFilters=new MeshFilter[0];
		public SkinnedMeshRenderer skinnedMeshRenderer;
		public SkinnedMeshRenderer[] skinnedMeshRenderers=new SkinnedMeshRenderer[0];

		#endregion Fields

		#region Methods

#if UNITY_EDITOR
		[ContextMenu("Enable Sub Renderers")]
		protected virtual void EnableSubRenderers()
			=>SetSubRenderersEnabled(true);

		[ContextMenu("Disable Sub Renderers")]
		protected virtual void DisableSubRenderers()
			=>SetSubRenderersEnabled(false);
#endif

		public virtual void SetSubRenderersEnabled(bool value) {
			int i;
			Renderer r;
			MeshFilter mf;
			//
			i=meshFilters?.Length??0;
			while(i-->0) {
				mf=meshFilters[i];
				if(mf!=null) {
					r=mf.GetComponent<Renderer>();
					if(r!=null) {
						r.enabled=value;
					}
				}
			}
			//
			i=skinnedMeshRenderers?.Length??0;
			while(i-->0) {
				r=skinnedMeshRenderers[i];
				if(r!=null) {
					r.enabled=value;
				}
			}
		}

		public override void Run() {
			int imax;
			//
			imax=meshFilters.Length;
			if(imax==0) {
				meshFilters=this.GetComponentsInChildren<MeshFilter>(false);
				imax=meshFilters.Length;
				if(imax>0) {
					if(meshFilters[0].transform==transform) {
						meshFilters[0]=null;
					}
				}
			}
			if(imax>0) {
				if(meshFilter==null) {
					meshFilter=this.AddMissingComponent<MeshFilter>();
				}
				CombineTo(meshFilters,meshFilter);
			}
			//
			imax=skinnedMeshRenderers.Length;
			if(imax==0) {
				skinnedMeshRenderers=this.GetComponentsInChildren<SkinnedMeshRenderer>(false);
				imax=skinnedMeshRenderers.Length;
				if(imax>0) {
					if(skinnedMeshRenderers[0].transform==transform) {
						skinnedMeshRenderers[0]=null;
					}
				}
			}
			if(imax>0) {
				if(skinnedMeshRenderer==null) {
					skinnedMeshRenderer=this.AddMissingComponent<SkinnedMeshRenderer>();
				}
				CombineTo(skinnedMeshRenderers,skinnedMeshRenderer);
			}
			SetSubRenderersEnabled(false);
		}

		#endregion Methods

	}

}
