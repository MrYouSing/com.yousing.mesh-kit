using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class UVTexture
		:ScriptableTexture
	{
		#region Fields

		public Material material;
		public Mesh[] meshes;
		public Color[] colors;
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

		public virtual void Render(Mesh mesh,Color color,int submesh=-1) {
			if(!m_IsInited) {Init();}
#if UNITY_EDITOR
#endif
			int[] triangles=submesh>=0?mesh.GetTriangles(submesh):mesh.triangles;
			Vector2[] uv=mesh.uv;
			PushRenderTexture(renderTexture);
			GL.PushMatrix();
				GL.LoadOrtho();
				if(material!=null) {material.SetPass(0);}
				GL.Begin(GL.LINES);
				GL.Color(color);
				for(int i=0,imax=triangles.Length/3,j=0;i<imax;++i,j+=3) {
					GL.Vertex(uv[triangles[j]]);GL.Vertex(uv[triangles[j+1]]);
					GL.Vertex(uv[triangles[j+1]]);GL.Vertex(uv[triangles[j+2]]);
					GL.Vertex(uv[triangles[j+2]]);GL.Vertex(uv[triangles[j]]);
				}
				GL.End();
			GL.PopMatrix();
			PopRenderTexture();
		}

		[ContextMenu("Render")]
		public virtual void Render() {
			int i=0,imax=Mathf.Min(meshes?.Length??0,colors?.Length??0);
			for(;i<imax;++i) {
				Render(meshes[i],colors[i]);
			}
			Flush();
		}

		#endregion Methods
	}
}
