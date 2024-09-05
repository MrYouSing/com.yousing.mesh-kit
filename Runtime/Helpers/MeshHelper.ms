// <!-- Macro.Copy -->
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public static partial class $(FileName) {
// <!-- Macro.End -->

/* <!-- Macro.Define Declare
		public static Mesh GetSharedMesh(this {0} thiz,ref MeshFilter mf,ref SkinnedMeshRenderer smr,int searchDir=0) {{
			Mesh mesh=null;
			//
			thiz.CheckComponent<MeshFilter>(ref mf,searchDir);
			if(mf!=null) {{
				mesh=mf.sharedMesh;
			}}else {{
				thiz.CheckComponent<SkinnedMeshRenderer>(ref smr,searchDir);
				if(smr!=null) {{
					mesh=smr.sharedMesh;
				}}
			}}
			//
			return mesh;
		}}

		public static void SetSharedMesh(this {0} thiz,Mesh mesh,ref MeshFilter mf,ref SkinnedMeshRenderer smr,int searchDir=0) {{
			thiz.CheckComponent<MeshFilter>(ref mf,searchDir);
			if(mf!=null) {{
				mf.sharedMesh=mesh;
			}}else {{
				thiz.CheckComponent<SkinnedMeshRenderer>(ref smr,searchDir);
				if(smr!=null) {{
					smr.sharedMesh=mesh;
				}}
			}}
		}}

		public static Mesh GetSharedMesh(this {0} thiz,int searchDir=0) {{
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			return GetSharedMesh(thiz,ref mf,ref smr,searchDir);
		}}

		public static void SetSharedMesh(this {0} thiz,Mesh mesh,int searchDir=0) {{
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			SetSharedMesh(thiz,mesh,ref mf,ref smr,searchDir);
		}}

		public static void BakeMesh(this {0} thiz,MeshFilter dest,int searchDir=0) {{
			if(thiz!=null) {{
				MeshFilter mf=null;
				SkinnedMeshRenderer smr=null;
				Mesh mesh=GetSharedMesh(thiz,ref mf,ref smr,searchDir);
				if(smr!=null) {{
					mesh=Object.Instantiate<Mesh>(mesh);// TODO : 
					smr.BakeMesh(mesh);
				}}
				dest.sharedMesh=mesh;
			}}
		}}

		public static Mesh GetInstancedMesh(this {0} thiz,int searchDir=0) {{
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			Mesh mesh=GetSharedMesh(thiz,ref mf,ref smr,searchDir);
			if(smr!=null&&mesh!=null) {{
			if(mesh.name.IndexOf("(Clone)")<0) {{
				mesh=Mesh.Instantiate(mesh);smr.BakeMesh(mesh,false);
			}}}}
			return mesh;
		}}
 Macro.End --> */

/* <!-- Macro.Call Declare
GameObject
Component
 Macro.End --> */
/* <!-- Macro.Define GetUVs
					case {0}:return thiz.uv{0};
 Macro.End --> */
// <!-- Macro.Copy -->

		public static Vector2[] GetUVs(this Mesh thiz,int channel) {
			if(thiz!=null) {
				switch(channel+1) {
// <!-- Macro.End -->
/* <!-- Macro.Call GetUVs
2,
3,
4,
5,
6,
7,
8,
 Macro.End --> */
// <!-- Macro.Copy -->
					default:return thiz.uv;
				}
			}
			return null;
		}
// <!-- Macro.End -->
// <!-- Macro.Copy -->
	}
}
// <!-- Macro.End -->
