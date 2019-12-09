using System.Text;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class BlendShapeExporter
		:MonoTask
	{
		#region Fields

		public string path="Assets/{1}_{3}.asset";
		public Mesh mesh;

		#endregion Fields

		#region Methods

		public override void Run() {
			if(mesh==null) {return;}
			//
			SkinnedMeshRenderer smr=this.AddMissingComponent<SkinnedMeshRenderer>();
			Mesh tmp;
			StringBuilder sb=new StringBuilder();
			string s,n=name,mn=mesh.name;
			int i=0,imax=mesh.blendShapeCount;
			//
			smr.sharedMesh=mesh;
			for(;i<imax;++i) {
				s=mesh.GetBlendShapeName(i);
				sb.AppendLine(s);
				//
				if(i>0) {smr.SetBlendShapeWeight(i-1,0.0f);}
				smr.SetBlendShapeWeight(i,100.0f);
				//
				tmp=Mesh.Instantiate(mesh);
				smr.BakeMesh(tmp);
				//
#if UNITY_EDITOR
				UnityEditor.AssetDatabase.CreateAsset(tmp,string.Format(path,
					n,mn,i,s
				));
#endif
			}
			print(sb.ToString());
		}

		#endregion Methods
	}
}