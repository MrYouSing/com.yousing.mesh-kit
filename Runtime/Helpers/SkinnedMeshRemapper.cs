using UnityEngine;

namespace YouSingStudio.MeshKit {

	/// <summary>
	/// 
	/// </summary>
	public class SkinnedMeshRemapper:MonoTask {

		#region Fields

		public SkinnedMeshRenderer target;
		public string root="../";
		[System.NonSerialized]protected Transform m_Root;

		public string rootBonePath="";
		public string[] bonesPath=new string[0];

		#endregion Fields

		#region Methods

		public override void Run()=>Apply();

		public virtual bool EnsureMembers() {
			m_Root=transform.FindEx(root);
			if(target==null) {
				target=GetComponent<SkinnedMeshRenderer>();
			}
			return target!=null;
		}

		[ContextMenu("Bake")]
		public virtual void Bake() {
			if(!EnsureMembers()) {
				return;
			}
			//
			rootBonePath=target.rootBone.GetPath(m_Root);
			//
			Transform[] bones=target.bones;
			int i=0,imax=bones.Length;
			bonesPath=new string[imax];
			for(;i<imax;++i) {
				bonesPath[i]=bones[i].GetPath(m_Root);
			}
		}
		
		[ContextMenu("Apply")]
		public virtual void Apply() {
			if(!EnsureMembers()) {
				return;
			}
#if UNITY_EDITOR
			if(!UnityEditor.EditorApplication.isPlaying) {
				UnityEditor.Undo.RecordObject(target,"Apply");
			}
#endif
			//
			target.rootBone=m_Root.FindEx(rootBonePath);
			int i=0,imax=bonesPath.Length;
			Transform[] bones=new Transform[imax];
			for(;i<imax;++i) {
				bones[i]=m_Root.FindEx(bonesPath[i]);
			}
			target.bones=bones;
		}

		#endregion Methods

	}

}
