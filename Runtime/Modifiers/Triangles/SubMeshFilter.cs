using System.Collections;
using UnityEngine;
namespace YouSingStudio.MeshKit {
	public class SubMeshFilter
		:MeshModifierBase
	{
		#region Fields

		public static readonly int[] s_Empty=new int[0];
		public bool optimize;

		#endregion Fields

		#region Methods

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				int j=-1;
				for(int i=0,imax=mesh.subMeshCount;i<imax;++i) {
					if((submesh&(1<<i))==0) {
						mesh.SetTriangles(s_Empty,i);
					}else {
						if(j<0) {j=i;}
					}
				}
				if(target!=null&&j>=0) {//mesh.subMeshCount=1;
					Renderer r=target.GetComponent<Renderer>();
					if(r!=null&&(r.sharedMaterials?.Length??0)!=1) {
						r.sharedMaterials=new Material[]{r.sharedMaterials[j]};
					}
				}
				if(optimize) {mesh.Optimize();}
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods
	}
}