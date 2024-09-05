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
		public string[] messages;

		#endregion Fields

		#region Methods

		public override void Run() {
			GameObject go=model.GetInstance(),it;Transform src,dst=go.transform;
			Transform container=null;
			int i=0,imax=models?.Length??0,imsg=messages?.Length??0;
			for(;i<imax;++i) {
				it=models[i].GetInstance();
				if(it!=null) {
					if(i<imsg) {it.SendMessageEx(messages[i]);}
					//
					src=it.transform;
					var list=it.GetComponentsInChildren<SkinnedMeshRenderer>();
					if((list?.Length??0)>0) {
						if(container==null) {container=dst.FindOrCreate(path);}
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