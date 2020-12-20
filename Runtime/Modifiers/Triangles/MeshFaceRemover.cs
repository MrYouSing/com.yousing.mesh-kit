using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshFaceRemover
		:MeshModifierBase
	{
		#region Fields

		public MeshSelectorBase selector;
		public bool value=false;

		#endregion Fields

		#region Methods

		public override void Run() {
			if(selector==null) {
				selector=GetComponent<MeshSelectorBase>();
				if(selector==null) {
					return;
				}
			}
			//
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				List<int> triangles=new List<int>(GetTriangles(mesh));
				List<int> selection=selector.SelectTriangles(mesh,triangles);
				if(value) {
					var tmp=new List<int>();
					for(int i=0,imax=selection?.Count??0,j;i<imax;++i) {
						j=selection[i];
						tmp.Add(triangles[j+0]);
						tmp.Add(triangles[j+1]);
						tmp.Add(triangles[j+2]);
					}
					triangles=tmp;
				}else {
					for(int i=(selection?.Count??0)-1;i>=0;--i) {
						triangles.RemoveRange(selection[i],3);
					}
				}
				SetTriangles(mesh,triangles.ToArray());
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}
