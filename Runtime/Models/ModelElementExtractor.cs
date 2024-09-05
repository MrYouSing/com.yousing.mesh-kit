using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class ModelElementExtractor
		:MonoTask
	{
		#region Fields

		public Transform target;
		public MeshTopologySelector selector;
		public GameObject prefab;
		public string format="{0:D2}";

		#endregion Fields

		#region Methods

		public override void Run() {
			if(selector!=null) {
				//
				GameObject prefab=this.prefab;
				if(prefab==null) {
					prefab=GameObject.CreatePrimitive(PrimitiveType.Cube);
					DestroyImmediate(prefab.GetComponent<BoxCollider>());
					var tmp=prefab.AddComponent<MeshSimpleCuller>();
					tmp.target=prefab.transform;
					tmp.inverse=true;
				}
				//
				selector.Run();
				selector.fastMode=true;selector.indexes=new int[1];
				int i=0,imax=selector.shapes.Count;
				GameObject go;for(;i<imax;++i) {
					selector.shapes[i].CopyTo(selector.indexes,0,1);
					//
					go=Instantiate(prefab);
					go.GetComponentInChildren<Renderer>().name=string.Format(format,i);
					go.transform.SetParent(target,false);
					var tmp=go.GetComponent<MeshSimpleCuller>();
					tmp.selection=selector;
					tmp.mesh=selector.mesh;
					tmp.Run();
				}
				selector.fastMode=false;
			}
		}

		#endregion Methods
	}
}
