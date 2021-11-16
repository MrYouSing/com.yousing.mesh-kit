using System.IO;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class ScriptableTexture
		:ScriptableObject
	{
		#region Fields

		public static RenderTexture s_RenderTexture;

		public Texture2D texture;
		public Vector2Int size;
		public Color color;
		[System.NonSerialized]protected bool m_IsInited;

		#endregion Fields

		#region Methods

		public static Texture2D FlushTexture2D(Texture2D texture,Object obj) {
#if UNITY_EDITOR
			string path=UnityEditor.AssetDatabase.GetAssetPath(obj);
			if(!string.IsNullOrEmpty(path)) {
				if(Path.GetExtension(path).ToLower()==".asset") {
					path=Path.ChangeExtension(path,".png");
				}
				//
				File.WriteAllBytes(path,texture.EncodeToPNG());
				UnityEditor.AssetDatabase.Refresh();
				//
				UnityEditor.TextureImporter ti=UnityEditor.AssetImporter.GetAtPath(path) as UnityEditor.TextureImporter;
				if(ti!=null) {
					ti.isReadable=true;
					ti.textureCompression=UnityEditor.TextureImporterCompression.Uncompressed;
					ti.SaveAndReimport();
				}
				return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);
			}
#endif
			return texture;
		}

		public static void PushRenderTexture(RenderTexture rt) {
			s_RenderTexture=RenderTexture.active;
			RenderTexture.active=rt;
		}

		public static void PopRenderTexture() {
			RenderTexture.active=s_RenderTexture;
		}
		

		protected virtual void Init() {
			if(m_IsInited) {
				return;
			}
			m_IsInited=true;
			//
			if(texture==null) {
				texture=new Texture2D(size.x,size.y,TextureFormat.RGBA32,false);
				texture=FlushTexture2D(texture,this);
			}
		}

		public virtual void Flush() {
			if(!m_IsInited) {Init();}
			//
			if(texture!=null) {
				texture.Apply();
				texture=FlushTexture2D(texture,texture);
			}
		}

		#endregion Methods
	}
}