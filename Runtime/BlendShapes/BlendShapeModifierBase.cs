using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract class BlendShapeModifierBase
		:MonoTask
	{
		#region Fields

		public Mesh mesh;
		public string source;
		public string destination;

		#endregion Fields

		#region Methods

		public override void Run() {
			if(mesh==null) {return;}
			//
			BlendShapeWrapper srcShape=new BlendShapeWrapper(mesh,source);
			BlendShapeWrapper dstShape=new BlendShapeWrapper(mesh,destination);
			BlendShapeFrame rawFrame=new BlendShapeFrame(mesh);
			BlendShapeFrame srcFrame;
			BlendShapeFrame dstFrame;
			for(int i=0,imax=srcShape.frames.Count,icnt=dstShape.frames.Count;i<imax;++i) {
				srcFrame=srcShape.frames[i];
				if(i<icnt) {
					dstFrame=dstShape.frames[i];
				}else {
					dstFrame=new BlendShapeFrame(srcFrame.vertices.Length);
					dstFrame.index=srcFrame.index;
					dstFrame.weight=srcFrame.weight;
					dstShape.frames.Add(dstFrame);
				}
				OnModifyBlendShape(rawFrame,srcFrame,dstFrame);
			}
			dstShape.Write();
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(mesh);
			UnityEditor.AssetDatabase.Refresh();
#endif
		}

		protected abstract void OnModifyBlendShape(BlendShapeFrame raw,BlendShapeFrame src,BlendShapeFrame dst);

		#endregion Methods
	}
}