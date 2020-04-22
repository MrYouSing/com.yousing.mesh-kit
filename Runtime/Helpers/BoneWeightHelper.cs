using UnityEngine;

namespace YouSingStudio.MeshKit {
	public static partial class BoneWeightHelper
	{
		public static float GetWeight(ref BoneWeight thiz,int i) {
			switch(i) {
				case 0:return thiz.weight0;
				case 1:return thiz.weight1;
				case 2:return thiz.weight2;
				case 3:return thiz.weight3;
			}
			return -1.0f;
		}

		public static void SetWeight(ref BoneWeight thiz,int i,float f) {
			switch(i) {
				case 0:thiz.weight0=f;break;
				case 1:thiz.weight1=f;break;
				case 2:thiz.weight2=f;break;
				case 3:thiz.weight3=f;break;
			}
		}

		public static int GetBoneIndex(ref BoneWeight thiz,int i) {
			switch(i) {
				case 0:return thiz.boneIndex0;
				case 1:return thiz.boneIndex1;
				case 2:return thiz.boneIndex2;
				case 3:return thiz.boneIndex3;
			}
			return -1;
		}

		public static void SetBoneIndex(ref BoneWeight thiz,int i,int n) {
			switch(i) {
				case 0:thiz.boneIndex0=n;break;
				case 1:thiz.boneIndex1=n;break;
				case 2:thiz.boneIndex2=n;break;
				case 3:thiz.boneIndex3=n;break;
			}
		}
	}
}
