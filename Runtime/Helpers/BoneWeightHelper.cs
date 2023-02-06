/* <!-- Macro.Define bFixPatch=
true
 Macro.End --> */

/* <!-- Macro.Define Accessor
		public static {0} {1}(this ref BoneWeight thiz,int index{2}) {{
			switch(index) {{
				case 0:{3}thiz.{4}0{5};break;
				case 1:{3}thiz.{4}1{5};break;
				case 2:{3}thiz.{4}2{5};break;
				case 3:{3}thiz.{4}3{5};break;
			}}
			return {6};
		}}

 Macro.End --> */
/* <!-- Macro.Call Accessor
float,GetWeight,,return ,weight,,-1.0f,
void,SetWeight,&#44;float value,,weight,=value,,
int,GetBoneIndex,,return ,boneIndex,,-1,
void,SetBoneIndex,&#44;int value,,boneIndex,=value,,
 Macro.End --> */

/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public static partial class BoneWeightHelper
	{
// <!-- Macro.Patch AutoGen
		public static float GetWeight(this ref BoneWeight thiz,int index) {
			switch(index) {
				case 0:return thiz.weight0;break;
				case 1:return thiz.weight1;break;
				case 2:return thiz.weight2;break;
				case 3:return thiz.weight3;break;
			}
			return -1.0f;
		}

		public static void SetWeight(this ref BoneWeight thiz,int index,float value) {
			switch(index) {
				case 0:thiz.weight0=value;break;
				case 1:thiz.weight1=value;break;
				case 2:thiz.weight2=value;break;
				case 3:thiz.weight3=value;break;
			}
			return ;
		}

		public static int GetBoneIndex(this ref BoneWeight thiz,int index) {
			switch(index) {
				case 0:return thiz.boneIndex0;break;
				case 1:return thiz.boneIndex1;break;
				case 2:return thiz.boneIndex2;break;
				case 3:return thiz.boneIndex3;break;
			}
			return -1;
		}

		public static void SetBoneIndex(this ref BoneWeight thiz,int index,int value) {
			switch(index) {
				case 0:thiz.boneIndex0=value;break;
				case 1:thiz.boneIndex1=value;break;
				case 2:thiz.boneIndex2=value;break;
				case 3:thiz.boneIndex3=value;break;
			}
			return ;
		}

// Macro.Patch -->

		#region Methods

		public static void SetBoneIndex(ref BoneWeight thiz,IDictionary<int,int> m) {
			if(m!=null) {
				int i;
				i=thiz.boneIndex0;if(i>=0) {thiz.boneIndex0=m[i];}
				i=thiz.boneIndex1;if(i>=0) {thiz.boneIndex1=m[i];}
				i=thiz.boneIndex2;if(i>=0) {thiz.boneIndex2=m[i];}
				i=thiz.boneIndex3;if(i>=0) {thiz.boneIndex3=m[i];}
			}
		}

		public static void GetWeights(BoneWeight[] inputs,float[] outputs,int[] triangles) {
			if(inputs!=null&&outputs!=null) {
				HashSet<int> hs=new HashSet<int>(triangles);
				for(int i=0,imax=inputs.Length;i<imax;++i) {
				if(hs.Contains(i)) {
					var tmp=inputs[i];
					for(int j=0,k;j<4;++j) {
						k=tmp.GetBoneIndex(j);
						if(k>=0) {outputs[k]+=tmp.GetWeight(j);}
					}
				}}
			}
		}

		public static int FindIndex(string[] array,string item) {
			for(int i=0,imax=array?.Length??0;i<imax;++i) {
				if(System.Text.RegularExpressions.Regex.IsMatch(item,array[i])) {return i;}
			}
			return -1;
		}

		public static void Remap<T>(ref T[] array,int count,IDictionary<int,int> remap) {
			if(array!=null&&remap!=null) {
				T[] tmp=new T[count];
				for(int i=0;i<count;++i) {
					tmp[i]=array[remap[i]];
				}
				array=tmp;
			}
		}

		public static void ReduceBones(this SkinnedMeshRenderer thiz,float threshold=-1.0f,string[] reserves=null) {
			if(thiz!=null) {
				using(DictionaryPool<int,int>.Get(out var i2o)) {
				using(DictionaryPool<int,int>.Get(out var o2i)) {
				using(ListPool<Transform>.Get(out var list)) {
					Mesh mesh=thiz.sharedMesh;
					Transform[] bones=thiz.bones;
					Matrix4x4[] bindposes=mesh.bindposes;
					BoneWeight[] boneWeights=mesh.boneWeights;
					int i,imax=bones?.Length??0,j;
						if(threshold>=0.0f) {
							float[] weights=new float[bindposes.Length];
							GetWeights(boneWeights,weights,mesh.triangles);
							for(i=0;i<imax;++i) {
								if(weights[i]<threshold&&
									(reserves==null||FindIndex(reserves,bones[i].GetName())<0)
								) {
									bones[i]=null;
								}
							}
						}
						Transform t;for(i=0;i<imax;++i) {
							t=bones[i];
							if(t!=null) {
								j=list.IndexOf(t);
								if(j<0) {j=list.Count;list.Add(t);}
								i2o[i]=j;o2i[j]=i;
							}else {
								i2o[i]=0;
							}
						}
						imax=list.Count;
						Remap(ref bones,imax,o2i);
						Remap(ref bindposes,imax,o2i);
						for(i=0,imax=boneWeights?.Length??0;i<imax;++i) {
							BoneWeightHelper.SetBoneIndex(ref boneWeights[i],i2o);
						}
					thiz.bones=bones;
					mesh.bindposes=bindposes;
					mesh.boneWeights=boneWeights;
					mesh.Optimize();
				}}}
			}
		}

		#endregion Methods
	}
}
