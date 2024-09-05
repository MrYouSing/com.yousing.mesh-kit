using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshElementRipper
		:MeshModifierBase
	{
		#region Fields

		public int index;
		public Transform control;
		public bool inverse;
		public float threshold=0.0f;

		#endregion Fields

		#region Methods

		[ContextMenu("Bake Control")]
		protected override void Bake() {
			if(control==null) {
				control=transform.Find("Control");
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
			if(control!=null) {
				Mesh mesh=BeginModifyMesh();
				control.position=mesh.vertices[index];
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(control);
#endif
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				var triangles=GetTriangles(mesh);int i,imax=triangles.Length;
				bool[] flags=new bool[imax];var list=new List<int>(imax);var vertices=mesh.vertices;
				Process(vertices,triangles,flags,control!=null?control.position:vertices[index]);
				for(i=0;i<imax;++i) {if(flags[i]!=inverse) {list.Add(triangles[i]);}}
				SetTriangles(mesh,list.ToArray());
			}
			EndModifyMesh(mesh);
		}

		public virtual void Process(
			Vector3[] vertices,int[] triangles,
			bool[] flags,Vector3 point
		) {
			float sqr=threshold*threshold;
			for(int i=0,imax=triangles?.Length??0,j,k;i<imax;++i) {
				if(!flags[i]) {
				if((vertices[triangles[i]]-point).sqrMagnitude<=sqr) {
					i=Mathf.FloorToInt(i/3.0f)*3;
					j=i;k=3;while(k-->0) {
						flags[j]=true;
						++j;
					}
					j=i;k=3;while(k-->0) {
						Process(vertices,triangles,flags,vertices[triangles[j]]);
						++j;
					}
					i+=2;
				}}
			}
		}

		#endregion Methods
	}
}
