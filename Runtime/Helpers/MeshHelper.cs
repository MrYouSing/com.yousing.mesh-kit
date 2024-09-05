// Generated automatically by MacroCodeGenerator (from "Packages/com.yousing.mesh-kit/Runtime/Helpers/MeshHelper.ms")

using UnityEngine;

namespace YouSingStudio.MeshKit {

	public static partial class MeshHelper {
		public static Mesh GetSharedMesh(this GameObject thiz,ref MeshFilter mf,ref SkinnedMeshRenderer smr,int searchDir=0) {
			Mesh mesh=null;
			//
			thiz.CheckComponent<MeshFilter>(ref mf,searchDir);
			if(mf!=null) {
				mesh=mf.sharedMesh;
			}else {
				thiz.CheckComponent<SkinnedMeshRenderer>(ref smr,searchDir);
				if(smr!=null) {
					mesh=smr.sharedMesh;
				}
			}
			//
			return mesh;
		}

		public static void SetSharedMesh(this GameObject thiz,Mesh mesh,ref MeshFilter mf,ref SkinnedMeshRenderer smr,int searchDir=0) {
			thiz.CheckComponent<MeshFilter>(ref mf,searchDir);
			if(mf!=null) {
				mf.sharedMesh=mesh;
			}else {
				thiz.CheckComponent<SkinnedMeshRenderer>(ref smr,searchDir);
				if(smr!=null) {
					smr.sharedMesh=mesh;
				}
			}
		}

		public static Mesh GetSharedMesh(this GameObject thiz,int searchDir=0) {
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			return GetSharedMesh(thiz,ref mf,ref smr,searchDir);
		}

		public static void SetSharedMesh(this GameObject thiz,Mesh mesh,int searchDir=0) {
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			SetSharedMesh(thiz,mesh,ref mf,ref smr,searchDir);
		}

		public static void BakeMesh(this GameObject thiz,MeshFilter dest,int searchDir=0) {
			if(thiz!=null) {
				MeshFilter mf=null;
				SkinnedMeshRenderer smr=null;
				Mesh mesh=GetSharedMesh(thiz,ref mf,ref smr,searchDir);
				if(smr!=null) {
					mesh=Object.Instantiate<Mesh>(mesh);// TODO : 
					smr.BakeMesh(mesh);
				}
				dest.sharedMesh=mesh;
			}
		}

		public static Mesh GetInstancedMesh(this GameObject thiz,int searchDir=0) {
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			Mesh mesh=GetSharedMesh(thiz,ref mf,ref smr,searchDir);
			if(smr!=null&&mesh!=null) {
			if(mesh.name.IndexOf("(Clone)")<0) {
				mesh=Mesh.Instantiate(mesh);smr.BakeMesh(mesh,false);
			}}
			return mesh;
		}
		public static Mesh GetSharedMesh(this Component thiz,ref MeshFilter mf,ref SkinnedMeshRenderer smr,int searchDir=0) {
			Mesh mesh=null;
			//
			thiz.CheckComponent<MeshFilter>(ref mf,searchDir);
			if(mf!=null) {
				mesh=mf.sharedMesh;
			}else {
				thiz.CheckComponent<SkinnedMeshRenderer>(ref smr,searchDir);
				if(smr!=null) {
					mesh=smr.sharedMesh;
				}
			}
			//
			return mesh;
		}

		public static void SetSharedMesh(this Component thiz,Mesh mesh,ref MeshFilter mf,ref SkinnedMeshRenderer smr,int searchDir=0) {
			thiz.CheckComponent<MeshFilter>(ref mf,searchDir);
			if(mf!=null) {
				mf.sharedMesh=mesh;
			}else {
				thiz.CheckComponent<SkinnedMeshRenderer>(ref smr,searchDir);
				if(smr!=null) {
					smr.sharedMesh=mesh;
				}
			}
		}

		public static Mesh GetSharedMesh(this Component thiz,int searchDir=0) {
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			return GetSharedMesh(thiz,ref mf,ref smr,searchDir);
		}

		public static void SetSharedMesh(this Component thiz,Mesh mesh,int searchDir=0) {
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			SetSharedMesh(thiz,mesh,ref mf,ref smr,searchDir);
		}

		public static void BakeMesh(this Component thiz,MeshFilter dest,int searchDir=0) {
			if(thiz!=null) {
				MeshFilter mf=null;
				SkinnedMeshRenderer smr=null;
				Mesh mesh=GetSharedMesh(thiz,ref mf,ref smr,searchDir);
				if(smr!=null) {
					mesh=Object.Instantiate<Mesh>(mesh);// TODO : 
					smr.BakeMesh(mesh);
				}
				dest.sharedMesh=mesh;
			}
		}

		public static Mesh GetInstancedMesh(this Component thiz,int searchDir=0) {
			MeshFilter mf=null;
			SkinnedMeshRenderer smr=null;
			Mesh mesh=GetSharedMesh(thiz,ref mf,ref smr,searchDir);
			if(smr!=null&&mesh!=null) {
			if(mesh.name.IndexOf("(Clone)")<0) {
				mesh=Mesh.Instantiate(mesh);smr.BakeMesh(mesh,false);
			}}
			return mesh;
		}

		public static Vector2[] GetUVs(this Mesh thiz,int channel) {
			if(thiz!=null) {
				switch(channel+1) {
					case 2:return thiz.uv2;
					case 3:return thiz.uv3;
					case 4:return thiz.uv4;
					case 5:return thiz.uv5;
					case 6:return thiz.uv6;
					case 7:return thiz.uv7;
					case 8:return thiz.uv8;
					default:return thiz.uv;
				}
			}
			return null;
		}
	}
}
