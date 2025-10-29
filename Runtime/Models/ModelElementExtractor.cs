using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class ModelElementExtractor
		:MonoTask
	{
		#region Fields

		public Transform target;
		public MeshTopologySelector selector;
		public int submeshes=-1;
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
					prefab.name="Mesh";
					DestroyImmediate(prefab.GetComponent<BoxCollider>());
					var tmp=prefab.AddComponent<MeshSimpleCuller>();
					tmp.target=prefab.transform;
					tmp.inverse=true;
				}
				//
				if(submeshes!=-1&&selector.submeshes==null) {
					selector.submeshes=new List<Vector2Int>();
				}
				selector.Run();
				selector.fastMode=true;
					InternalRun();
				selector.fastMode=false;
				if(submeshes!=-1) {
					selector.submeshes=null;
				}
			}
		}

		protected virtual void InternalRun() {
			selector.indexes=new int[1];
			int i=0,imax=selector.shapes.Count;
			for(;i<imax;++i) {
				selector.shapes[i].CopyTo(selector.indexes,0,1);
				InternalRun(i);
			}
		}

		protected virtual void InternalRun(int index) {
			GameObject go;if(prefab.scene.IsValid()&&prefab.name!="Mesh") {
				go=prefab;
			}else {
				go=Instantiate(prefab);
				go.GetComponentInChildren<Renderer>().name=string.Format(format,index);
				go.transform.SetParent(target,false);
			}
			var tmp=go.GetComponent<MeshSimpleCuller>();
			tmp.selection=selector;
			tmp.mesh=selector.mesh;
			tmp.Run();
		}

		#endregion Methods
	}
}
