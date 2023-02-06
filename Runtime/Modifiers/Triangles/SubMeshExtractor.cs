using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class SubMeshExtractor
		:MeshModifierBase
	{
		#region Fields

		public GameObject prefab;
		public List<SubMeshModifier> subModifiers=new List<SubMeshModifier>();
		[System.NonSerialized]protected Renderer m_Renderer;

		#endregion Fields

		#region Methods

		public virtual SubMeshModifier NewSubModifier(int index,SubMeshModifier modifier=null) {
			if(modifier!=null&&!modifier.IsPrefab()) {
				return modifier;
			}
			Transform t=(target!=null&&index<target.childCount)?target.GetChild(index):null;
			if(t==null) {
				GameObject go=modifier!=null?modifier.gameObject:prefab;
				go=go!=null?GameObject.Instantiate(go):new GameObject();
				go.name=name+" ("+index+")";
				t=go.transform;t.SetParent(target,false);
				if(m_Renderer is MeshRenderer mr) {
					t.AddMissingComponent<MeshRenderer>();
				}else if(m_Renderer is SkinnedMeshRenderer smr) {
					var sub=t.AddMissingComponent<SkinnedMeshRenderer>();
					sub.localBounds=smr.localBounds;
					sub.rootBone=smr.rootBone;
					sub.bones=smr.bones;
				}
				if(true) {
					var sub=t.AddMissingComponent<SubMeshModifier>();
					sub.runType=RunType.Manual;sub.target=t;return sub;
				}
			}
			return t.AddMissingComponent<SubMeshModifier>();
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				m_Renderer=target!=null?target.GetComponent<Renderer>():null;
				Renderer r=m_Renderer;
				Material[] materials=r!=null?r.sharedMaterials:null;
				SubMeshModifier it;
				int i,icnt=subModifiers.Count,imax=mesh.subMeshCount;
				for(i=0;i<imax;++i) {
					if(i<icnt) {subModifiers[i]=NewSubModifier(i,subModifiers[i]);}
					else {subModifiers.Add(NewSubModifier(i));}
				}
				for(i=0;i<imax;++i) {
					it=subModifiers[i];
					if(it!=null) {
						r=it.target!=null?it.target.GetComponent<Renderer>():null;
						if(r!=null) {r.sharedMaterials=materials;}
						it.submesh=i;it.Run(mesh);
					}
				}
			}
			EndModifyMesh(mesh);
		}

		protected override void EndModifyMesh(Mesh mesh) {
			if(mesh!=null) {
				if(autoApply) {
					if(m_Renderer!=null) {
						m_Renderer.enabled=false;m_Renderer.SetSharedMesh(null);// TODO Destroy????
					}
				}
				onApply?.Invoke(mesh);
			}
		}

		#endregion Methods
	}
}
