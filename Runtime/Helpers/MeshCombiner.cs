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
			//
			combines.Clear();
			// TODO: Missing blend shapes????
			return mesh;
		}

		// References
		// 1) https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
		public static Mesh CombineMeshes(IList<MeshFilter> meshFilters,Matrix4x4 worldToLocalMatrix) {
			var combines=BeginCombineMeshes();
			MeshFilter mf;
			Renderer r;
			//
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
			//
			return EndCombineMeshes(combines);
		}

		// References
		// 1) http://www.oschina.net/code/snippet_2272150_45935
		public static Mesh CombineMeshes(IList<SkinnedMeshRenderer> skinnedMeshRenderers,Matrix4x4 worldToLocalMatrix,out Transform[] bones) {
			var combines=BeginCombineMeshes();
			var list=s_Transforms;
			SkinnedMeshRenderer smr;
			//
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
			list.Clear();
			return EndCombineMeshes(combines);
		}

		public static Mesh CombineTo(IList<MeshFilter> meshFilters,MeshFilter meshFilter) {
			Mesh mesh=CombineMeshes(meshFilters,meshFilter.transform.worldToLocalMatrix);
			mesh.name=meshFilter.name;
			meshFilter.sharedMesh=mesh;
			//
			return mesh;
		}

		public static Mesh CombineTo(IList<SkinnedMeshRenderer> skinnedMeshRenderers,SkinnedMeshRenderer skinnedMeshRenderer) {
			Transform[] bones;
			Mesh mesh=CombineMeshes(skinnedMeshRenderers,skinnedMeshRenderer.transform.worldToLocalMatrix,out bones);
			mesh.name=skinnedMeshRenderer.name;
			skinnedMeshRenderer.sharedMesh=mesh;
			skinnedMeshRenderer.bones=bones;
			//
			return mesh;
		}

		public static void Run(Transform root,Material material) {
			if(root!=null) {
				var mc=root.AddMissingComponent<MeshCombiner>();
				mc.skinnedMeshRenderer=root.GetComponent<SkinnedMeshRenderer>();
				mc.meshFilter=root.GetComponent<MeshFilter>();
				if(material!=null) {
					Renderer r=root.GetComponent<Renderer>();
					if(r!=null) {r.sharedMaterials=new Material[]{material};}
				}
				mc.reduceBones=true;mc.runType=RunType.Manual;
				mc.Run();
			}
		}

		#endregion Statics

		#region Fields

		public MeshFilter meshFilter;
		public List<MeshFilter> meshFilters;
		public SkinnedMeshRenderer skinnedMeshRenderer;
		public List<SkinnedMeshRenderer> skinnedMeshRenderers;

		public bool includeInactive=false;
		public bool fixTransforms=false;
		public bool reduceBones=false;
		public MonoTask task;

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
		protected virtual void SetMainRendererEnabled(bool value) {
			Renderer r=(meshFilter!=null)?meshFilter.GetComponent<Renderer>():null;
			if(r!=null) {r.enabled=value;}
			r=skinnedMeshRenderer;
			if(r!=null) {r.enabled=value;}
		}

		public virtual void SetSubRenderersEnabled(bool value) {
			if(value) {SetMainRendererEnabled(!value);}
			int i;
			Renderer r;
			MeshFilter mf;
			//
			i=meshFilters?.Count??0;
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
			i=skinnedMeshRenderers?.Count??0;
			while(i-->0) {
				r=skinnedMeshRenderers[i];
				if(r!=null) {
					r.enabled=value;
				}
			}
			//
			if(!value) {SetMainRendererEnabled(!value);}
		}

		protected virtual void CombineRenderers<T>(ref List<T> source,ref T destination,System.Func<IList<T>,T,Mesh> func) where T:Component {
			int imax;
			Mesh mesh=null;
			//
			imax=source?.Count??0;
			if(imax==0) {
				if(source==null) {source=new List<T>();}
				else {source.Clear();}
				GetComponentsInChildren(includeInactive,source);
				imax=source.Count;
				if(imax>0) {
					if(source[0].transform==transform) {source[0]=null;--imax;}
				}
			}
			if(imax>0) {
				if(destination==null) {destination=this.AddMissingComponent<T>();}
				if(fixTransforms) {
					Transform t=destination.transform;
					T it;
					for(int i=0;i<imax;++i) {
						it=source[i];
						if(it!=null) {
							it.transform.SetParent(t,false);
						}
					}
				}
				if(func!=null) {mesh=func(source,destination);}
				MonoTask.Run(task,mesh);
			}
		}

		public override void Run() {
			CombineRenderers(ref meshFilters,ref meshFilter,CombineTo);
			CombineRenderers(ref skinnedMeshRenderers,ref skinnedMeshRenderer,CombineTo);
			var smr=skinnedMeshRenderer;if(smr!=null) {
				if(smr.localBounds.size.sqrMagnitude==0.0f) {
					smr.SetBounds(skinnedMeshRenderers);
				}
				if(reduceBones) {smr.ReduceBones();}
			}
			SetSubRenderersEnabled(false);
		}

		#endregion Methods

	}

}
