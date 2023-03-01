using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using Dictionary=System.Collections.Generic.Dictionary<string,
	System.Func<YouSingStudio.MeshKit.TextureCanvas>>;

namespace YouSingStudio.MeshKit {
	public abstract class TextureCanvas
	{
		#region Fields

		public static bool s_Linear;
		public static Dictionary s_Canvases=
			new Dictionary(System.StringComparer.OrdinalIgnoreCase);

		#endregion Fields

		#region Methods

		static TextureCanvas() {
			s_Canvases["Texture2D"]=GenericPool<Texture2DCanvas>.Get;
			s_Canvases["RenderTexture"]=GenericPool<RenderTextureCanvas>.Get;
		}

		public static TextureCanvas Get(string key) {
			if(s_Canvases.TryGetValue(key,out var func)&&func!=null) {
				return func();
			}
			return null;
		}
		
		public static bool IsNormalized(Vector2 size) {
			return size.x<=1.0f&&size.y<=1.0f;
		}
		
		public static void Destroy(Object obj) {
			if(obj!=null) {
#if UNITY_EDITOR
				if(!UnityEditor.EditorApplication.isPlaying)// Editor-Mode
					Object.DestroyImmediate(obj);
				else
#endif
				Object.Destroy(obj);
			}
		}

		public static Texture2D NewTexture2D(int width,int height) {
			return new Texture2D(width,height,TextureFormat.RGBA32,false,s_Linear);
		}

		public static Texture2D LoadTexture(Texture2D texture) {
			if(texture!=null) {
#if UNITY_EDITOR
				if(!texture.isReadable) {
					string path=UnityEditor.AssetDatabase.GetAssetPath(texture);
					texture=NewTexture2D(1,1);texture.LoadImage(File.ReadAllBytes(path));
				}
#endif
			}
			return texture;
		}
		
		public static Texture2D SaveTexture(string path,Texture2D texture) {
			if(texture!=null) {
				byte[] bytes=null;
				switch(Path.GetExtension(path).ToLower()) {
					case ".jpeg":
					case ".jpg":bytes=texture.EncodeToJPG();break;
					case ".png":bytes=texture.EncodeToPNG();break;
					case ".tga":bytes=texture.EncodeToTGA();break;
					case ".exr":bytes=texture.EncodeToEXR();break;
				}
				if(bytes!=null) {
					File.WriteAllBytes(path,bytes);
#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh();//Destroy(texture);
					texture=UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);
#endif
				}
			}
			return texture;
		}
		
		public abstract void Begin(int width,int height,Color color);
		public abstract void DrawTexture(Rect rect,Texture texture);
		public abstract Texture2D SaveTexture(string path);
		public abstract void End();

		#endregion Methods
	}

	public class Texture2DCanvas
		:TextureCanvas
	{
		public Texture2D thiz;

		#region Methods

		public override void Begin(int width,int height,Color color) {
			thiz=NewTexture2D(width,height);
			Color[] colors=new Color[width*height];System.Array.Fill(colors,color);
			thiz.SetPixels(colors);thiz.Apply();
		}

		public override void DrawTexture(Rect rect,Texture texture) {
			if(texture is Texture2D t) {
				if(IsNormalized(rect.size)) {
					rect.x*=thiz.width;rect.y*=thiz.height;
				}
				thiz.SetPixels((int)rect.x,(int)rect.y,t.width,t.height,t.GetPixels());thiz.Apply();
			}
		}

		public override Texture2D SaveTexture(string path) {
			return SaveTexture(path,thiz);
		}

		public override void End() {
			if(thiz!=null) {Destroy(thiz);}
			thiz=null;GenericPool<Texture2DCanvas>.Release(this);
		}

		#endregion Methods
	}

	public class RenderTextureCanvas
		:TextureCanvas
	{
		public static Material material;
		public static RenderTexture last;

		public RenderTexture thiz;

		#region Methods

		public virtual void Push() {
			last=RenderTexture.active;
			RenderTexture.active=thiz;
		}

		public virtual void Pop() {
			RenderTexture.active=last;
		}

		public override void Begin(int width,int height,Color color) {
			RenderTextureDescriptor d=new RenderTextureDescriptor(width,height);
			d.useMipMap=false;d.sRGB=!s_Linear;
			thiz=RenderTexture.GetTemporary(d);
			Push();GL.Clear(true,true,color);Pop();
		}

		public override void DrawTexture(Rect rect,Texture texture) {
			if(!IsNormalized(rect.size)) {
				rect.x/=thiz.width;rect.y/=thiz.height;
				rect.width/=thiz.width;rect.height/=thiz.height;
			}
			Vector4 vector=new Vector4(rect.x,rect.y,rect.width,rect.height);
			vector.z+=vector.x;
			vector.w+=vector.y;
			RenderTexture rt=RenderTexture.active;RenderTexture.active=thiz;
				if(material==null) {material=new Material(Shader.Find("Unlit/Transparent"));}
				material.mainTexture=texture;
				material.SetPass(0);
				GL.PushMatrix();GL.LoadOrtho();
				GL.Begin(GL.QUADS);
					GL.TexCoord(Vector3.zero);
					GL.Vertex(new Vector3(vector.x,vector.y,0.0f));
					GL.TexCoord(Vector3.up);
					GL.Vertex(new Vector3(vector.x,vector.w,0.0f));
					GL.TexCoord(new Vector3(1.0f,1.0f,0.0f));
					GL.Vertex(new Vector3(vector.z,vector.w,0.0f));
					GL.TexCoord(Vector3.right);
					GL.Vertex(new Vector3(vector.z,vector.y,0.0f));
				GL.End();
				GL.PopMatrix();
			RenderTexture.active=rt;
		}

		public override Texture2D SaveTexture(string path) {
			int w=thiz.width,h=thiz.height;
			Texture2D tmp=NewTexture2D(w,h);
			Push();tmp.ReadPixels(new Rect(0,0,w,h),0,0);Pop();
			Texture2D ret=SaveTexture(path,tmp);
			if(ret!=tmp) {Destroy(tmp);}
			return ret;
		}

		public override void End() {
			if(thiz!=null) {RenderTexture.ReleaseTemporary(thiz);}
			thiz=null;GenericPool<RenderTextureCanvas>.Release(this);
		}

		#endregion Methods
	}
}
