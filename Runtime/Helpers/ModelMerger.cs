using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class ModelMerger
		:MonoTask
	{
		#region Fields

		public string path="Models";
		public GameObject model;
		public GameObject[] models;

		#endregion Fields

		#region Methods

		public override void Run() {
			GameObject go=model.GetInstance(),it;Transform src,dst=go.transform;
			Transform container=null;
			for(int i=0,imax=models?.Length??0;i<imax;++i) {
				it=models[i].GetInstance();
				if(it!=null) {
					src=it.transform;
					var list=it.GetComponentsInChildren<SkinnedMeshRenderer>();
					if((list?.Length??0)>0) {
						if(container==null) {
							if(string.IsNullOrEmpty(path)) {
								container=dst;
							}else {
								container=new GameObject(path).transform;
								container.SetParent(dst,false);
							}
						}
						foreach(var r in list) {
							var tmp=r.transform;
							tmp.SetParent(container,false);
							r.RemapBones(src,dst);
						}
						it.Destroy();
					}else {
						it.transform.SetParent(dst,false);
					}
				}
			}
		}

		#endregion Methods
	}
}