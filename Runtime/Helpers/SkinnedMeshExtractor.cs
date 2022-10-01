using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public class SkinnedMeshExtractor
		:MonoTask
	{
		#region Fields

		public Transform root;
		public SkinnedMeshRenderer[] renderers;

		[System.NonSerialized]protected List<Transform> m_Bones=new List<Transform>();

		#endregion Fields

		#region Methods

		public virtual void TryAdd(Transform t) {
			if(m_Bones.IndexOf(t)<0) {
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
#if UNITY_EDITOR
			if(!UnityEditor.EditorApplication.isPlaying) {
				GameObject.DestroyImmediate(t.gameObject);
			}else
#endif
			GameObject.Destroy(t.gameObject);
		}

		public override void Run() {
			m_Bones.Clear();
			int i=0,imax=renderers?.Length??0;
			SkinnedMeshRenderer it;for(;i<imax;++i) {
				it=renderers[i];
				if(it!=null) {
					TryAdd(it.transform);TryAdd(it.rootBone);
					System.Array.ForEach(it.bones,TryAdd);
				}
			}
			if(root==null) {root=transform;}
			using(ListPool<Transform>.Get(out var list)) {
				root.GetComponentsInChildren(true,list);
				list.ForEach(TryDestroy);
			}
		}

		#endregion Methods
	}
}
