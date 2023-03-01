using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	/// <summary>
	/// <seealso cref="Mesh.CombineMeshes(CombineInstance[],bool,bool,bool)"/><br/>
	/// <seealso cref="Texture2D.PackTextures(Texture2D[],int,int,bool)"/>
	/// </summary>
	public class ModelPacker
		:MonoBehaviour
	{
		#region Nested Types

		[System.Serializable]
		public class Group {
			public string name;
			public Transform root;
			public Material material;
			[Header("Packer")]
			public Texture2D texture;
			public int maximum;
			public Texture2D[] textures;
			public Rect[] rects;
			public string[] variants;

			[System.NonSerialized]public Vector2 size;

			public virtual int IndexOf(Material material) {
				if(material!=null&&textures!=null) {
					return System.Array.IndexOf(textures,material.mainTexture);
				}
				return -1;
			}

			public virtual void Bake() {
#if UNITY_EDITOR
			string path=null;
			if(texture==null||!texture.isReadable) {
				path=UnityEditor.AssetDatabase.GetAssetPath(current);
				texture=TextureCanvas.NewTexture2D(1,1);string dir=Path.GetDirectoryName(path);
				path=Path.Combine(dir,Path.GetFileNameWithoutExtension(path)+"_"+name+".png");
			}else {
				path=UnityEditor.AssetDatabase.GetAssetPath(texture);
			}
#else
			if(texture==null) {
				texture=TextureCanvas.NewTexture2D(1,1);
			}
#endif
			if((textures?.Length??0)>0) {
				rects=texture.PackTextures(System.Array.ConvertAll(textures,TextureCanvas.LoadTexture),0,maximum>0?maximum:2048);
#if UNITY_EDITOR
				texture=TextureCanvas.SaveTexture(path,texture);
#endif
				}
			}

			public virtual void Run(Material material) {
				int i=IndexOf(material);
				if(i>=0) {
					Texture2D it;
					for(int j=0,jmax=variants?.Length??0;j<jmax;++j) {
						it=material.GetTexture(variants[j]) as Texture2D;
						if(it!=null) {
							current.AddVariant(this,variants[j],rects[i],it);
						}
					}
				}
			}

			public virtual void Run(Material[] materials) {
				if((materials?.Length??0)==0) {return;}
				System.Array.ForEach(materials,Run);
				current.SaveVariants(this);
			}

			public virtual void Run(Renderer renderer) {
				int i=IndexOf(renderer.sharedMaterial);
				if(i>=0) {
					MeshUVModifier.Run(renderer.transform,rects[i]);
				}
			}

			public virtual void Run(Renderer[] renderers) {
				if((renderers?.Length??0)==0) {return;}
				System.Array.ForEach(renderers,Run);
				MeshCombiner.Run(root,material);
			}

			public virtual void Run() {
				if(texture==null) {return;}
				Rect[] tmp=rects;
					Bake();size=new Vector2(texture.width,texture.height);
					Run(current.materials);Run(current.renderers);
				rects=tmp;
			}
		}

		#endregion Nested Types

		#region Fields

		public static ModelPacker current;

		public bool linear;
		public string canvas=nameof(Texture2D);
		public Renderer[] renderers;
		public Material[] materials;
		public Group[] groups;
		[System.NonSerialized]public Dictionary<string,TextureCanvas> canvases=
			new Dictionary<string,TextureCanvas>();

		#endregion Fields

		#region Methods

		protected virtual void Begin() {
			current=this;
			TextureCanvas.s_Linear=linear;
		}

		protected virtual void End() {
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
			current=null;
		}


		[ContextMenu("Bake")]
		public virtual void Bake() {
			Begin();
			for(int i=0,imax=groups?.Length??0;i<imax;++i) {
				groups[i].Bake();
			}
			End();
		}

		[ContextMenu("Run")]
		public virtual void Run() {
			Begin();
			for(int i=0,imax=groups?.Length??0;i<imax;++i) {
				groups[i].Run();
			}
			End();
		}

		public virtual void AddVariant(Group group,string variant,Rect rect,Texture2D texture) {
			if(!canvases.TryGetValue(variant,out var tmp)||tmp==null) {
				canvases[variant]=tmp=TextureCanvas.Get(canvas);
				tmp.Begin((int)group.size.x,(int)group.size.y,Color.clear);
			}
			tmp.DrawTexture(rect,texture);
		}

		public virtual void SaveVariants(Group group) {
			string fn=string.Empty;
#if UNITY_EDITOR
			string path=UnityEditor.AssetDatabase.GetAssetPath(group.texture);
			path=path.Substring(0,path.LastIndexOf('.'))+"{0}"+Path.GetExtension(path);
#endif
			for(int i=0,imax=group?.variants?.Length??0;i<imax;++i) {
				if(canvases.Remove(group.variants[i],out var tmp)&&tmp!=null) {
#if UNITY_EDITOR
					fn=string.Format(path,group.variants[i]);
#endif
					tmp.SaveTexture(fn);tmp.End();
				}
			}
		}

		#endregion Methods
	}
}
