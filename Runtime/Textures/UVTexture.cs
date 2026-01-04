using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class UVTexture
		:ScriptableTexture
	{
		#region Fields

		public TextureWrapMode wrapMode;
		public Material material;
		public Mesh[] meshes;
		public Color[] colors;
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
					if(texture!=null) {
						if(texture.width!=size.x||texture.height!=size.y) {texture.Reinitialize(size.x,size.y);}
						texture.ReadPixels(new Rect(Vector2.zero,size),0,0);
					}
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
			if(wrapMode==TextureWrapMode.Repeat) {
				for(int i=0,imax=uv.Length;i<imax;++i) {
					uv[i]=new Vector2(Mathf.Repeat(uv[i].x,1.0f),Mathf.Repeat(uv[i].y,1.0f));
				}
			}
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
				Render(meshes[i],colors[i],submeshes[i]);
			}
			Flush();
		}

		#endregion Methods
	}
}
