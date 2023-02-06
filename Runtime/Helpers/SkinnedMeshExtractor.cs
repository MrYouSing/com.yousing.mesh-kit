using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public class SkinnedMeshExtractor
		:MonoTask
	{
		#region Fields

		public Transform root;
		public Transform parent;
		public string[] paths;
		public SkinnedMeshRenderer[] renderers;
		public bool reduceBones;
		public string[] reserveBones;

		[System.NonSerialized]protected List<Transform> m_Bones=new List<Transform>();

		#endregion Fields

		#region Methods

		public virtual void TryAdd(Transform t) {
			if(t!=null&&m_Bones.IndexOf(t)<0) {
				m_Bones.Add(t);
			}
		}

		public virtual void TryDestroy(Transform t) {
			if(t==null) {return;}
			//
			if(m_Bones.IndexOf(t)>=0) {return;}
			for(int i=0,imax=m_Bones.Count;i<imax;++i) {
				if(m_Bones[i].IsChildOf(t)) {return;}
			}
			//
			t.gameObject.Destroy();
		}

		public override void Run() {
			m_Bones.Clear();
			if(root==null) {root=transform;}
			//
			if((paths?.Length??0)>0) {
				renderers=System.Array.ConvertAll(paths,x=>root.FindEx(x).GetComponent<SkinnedMeshRenderer>());
			}
			Transform top=null,rb;
			int i=0,imax=renderers?.Length??0;
			SkinnedMeshRenderer it;for(;i<imax;++i) {
				it=renderers[i];
				if(it!=null) {
					if(reduceBones) {it.ReduceBones(Vector3.kEpsilon,reserveBones);}
					TryAdd(it.transform);TryAdd(rb=it.rootBone);
					System.Array.ForEach(it.bones,TryAdd);
					if(top==null||rb.GetDepth()<top.GetDepth()) {top=rb;}
				}
			
			if(parent!=null) {
				parent.SetParent(top!=null?top:root,false);
				parent.SetAsFirstSibling();
				parent.localPosition=Vector3.zero;
				parent.localRotation=Quaternion.identity;
			}}
			using(ListPool<Transform>.Get(out var list)) {
				root.GetComponentsInChildren(true,list);
				list.ForEach(TryDestroy);
			}
		}

		#endregion Methods
	}
}
