using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshVolumeSelector
		:MeshSelectorBase
	{
		#region Fields

		public bool strict;
		public bool inverse;
		public Collider[] colliders;
		[System.NonSerialized]public Vector3[] vertices;

		#endregion Fields

		#region Methods

		public override void BeginSelect(Mesh mesh) {
			if(mesh!=null) {
				vertices=mesh.vertices;
			}
		}

		public override void EndSelect(Mesh mesh) {
			vertices=null;
		}

		public override bool TestIndex(int index) {
			bool b=false;
			if(vertices!=null) {
			for(int i=0,imax=colliders?.Length??0;i<imax;++i) {
			if(colliders[i]!=null) {
				b=colliders[i].OverlapPoint(vertices[index]);
				if(b) {break;}
			}}}
			return inverse?!b:b;
		}

		public override List<int> SelectVertices(Mesh mesh,IList<int> triangles,List<int> result=null) {
			if(!strict) {
				return base.SelectVertices(mesh,triangles,result);
			}
			BeginSelect(mesh);
				int i=0,imax=triangles?.Count??0,t;
				if(result==null&&imax>0) {result=new List<int>(imax);}
				for(;i<imax;++i) {
					t=triangles[i];
					if(result.IndexOf(t)<0&&TestIndex(t)) {result.Add(t);}
				}
			EndSelect(mesh);
			return result;
		}

		#endregion Methods
	}
}
