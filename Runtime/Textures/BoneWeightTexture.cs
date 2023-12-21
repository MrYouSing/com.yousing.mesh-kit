using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class BoneWeightTexture
		:ScriptableTexture
	{
		#region Fields

		public Material material;
		public float thickness=1.0f;
		public Mesh[] meshes;
		public int[] submeshes;
		[System.NonSerialized]public RenderTexture renderTexture;

		#endregion Fields

		#region Methods

		protected override void Init() {
			if(m_IsInited) {
				return;
			}
			base.Init();
			//
			if(renderTexture==null) {
				renderTexture=RenderTexture.GetTemporary(size.x,size.y,0,RenderTextureFormat.ARGB32,RenderTextureReadWrite.Default,1);
				PushRenderTexture(renderTexture);
					GL.Clear(false,true,color);
				PopRenderTexture();
			}
		}

		public override void Flush(){
			if(renderTexture!=null) {
				PushRenderTexture(renderTexture);
					if(texture!=null) {texture.ReadPixels(new Rect(Vector2.zero,size),0,0);}
				PopRenderTexture();
				//
				base.Flush();
				//
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			m_IsInited=false;
			renderTexture=null;
		}

		public static Color GetColor(BoneWeight weight) {
			return new Color(weight.weight0,weight.weight1,weight.weight2,weight.weight3==0.0f?1.0f:weight.weight3);
		}

		public virtual void Render(Mesh mesh,int submesh=-1) {
			if(!m_IsInited) {Init();}
#if UNITY_EDITOR
#endif
			float x=thickness*.5f/size.x,y=thickness*.5f/size.y;
			int[] triangles=submesh>=0?mesh.GetTriangles(submesh):mesh.triangles;
			Vector2[] uv=mesh.uv;
			BoneWeight[] weighs=mesh.boneWeights;
			PushRenderTexture(renderTexture);
			GL.PushMatrix();
				GL.LoadOrtho();
				if(material!=null) {material.SetPass(0);}
				GL.Begin(GL.QUADS);
				for(int i=0,imax=uv?.Length??0;i<imax;++i) {
					GL.Color(GetColor(weighs[i]));
					GL.Vertex(uv[i]+new Vector2(-x,-y));
					GL.Vertex(uv[i]+new Vector2( x,-y));
					GL.Vertex(uv[i]+new Vector2( x, y));
					GL.Vertex(uv[i]+new Vector2(-x, y));
				}
				GL.End();
			GL.PopMatrix();
			PopRenderTexture();
		}

		[ContextMenu("Render")]
		public virtual void Render() {
			int i=0,imax=Mathf.Min(meshes?.Length??0,submeshes?.Length??0);
			for(;i<imax;++i) {
				Render(meshes[i],submeshes[i]);
			}
			Flush();
		}

		#endregion Methods
	}
}
