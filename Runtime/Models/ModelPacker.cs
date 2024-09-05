using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	/// <summary>
	/// <seealso cref="Mesh.CombineMeshes(CombineInstance[],bool,bool,bool)"/><br/>
	/// <seealso cref="Texture2D.PackTextures(Texture2D[],int,int,bool)"/>
	/// </summary>
	public class ModelPacker
		:MonoTask
	{
		#region Nested Types

		[System.Serializable]
		public class Group {
			public string name;
			public Transform root;
			public Material material;
			public string baseMap;
			[Header("Packer")]
			public Texture2D texture;
			public int maximum;
			public Texture2D[] textures;
			public Rect[] rects;
			public string[] variants;

			[System.NonSerialized]public Vector2 size;
			[System.NonSerialized]protected int m_BaseMap;

			public virtual int IndexOf(Material material) {
				if(material!=null&&textures!=null) {
					Texture t=m_BaseMap!=0?material.GetTexture(m_BaseMap):material.mainTexture,it;
					if(t!=null) {
						string s=t.name;
						for(int i=0,imax=textures?.Length??0;i<imax;++i) {
							it=textures[i];
							if(it!=null&&(it==t||it.name==s)) {return i;}
						}
					}
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
				if(material==null) {return;}
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
				if(renderer==null) {return;}
				int i=IndexOf(renderer.sharedMaterial);
				if(i>=0) {
					MeshUVModifier.Run(renderer.transform,rects[i]);
					if(current!=null&&current.export) {
						current.unexports.Add(renderer.transform);
					}
				}
			}

			public virtual void Run(Renderer[] renderers) {
				if((renderers?.Length??0)==0) {return;}
				System.Array.ForEach(renderers,Run);
				MeshCombiner.Run(root,material);
			}

			public virtual void Run() {
				if(texture==null) {return;}
				m_BaseMap=!string.IsNullOrEmpty(baseMap)?Shader.PropertyToID(baseMap):0;
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
		public bool export;
		public Renderer[] renderers;
		public Material[] materials;
		public Group[] groups;
		[System.NonSerialized]public Dictionary<string,TextureCanvas> canvases=
			new Dictionary<string,TextureCanvas>();
		[System.NonSerialized]public List<Transform> unexports=
			new List<Transform>();

		[System.NonSerialized]protected int m_Index=-1;

		#endregion Fields

		#region Methods

		protected virtual void Begin() {
			current=this;
			TextureCanvas.s_Linear=linear;
		}

		protected virtual void End() {
			unexports.ForEach(Detach);unexports.Clear();
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

		public override void Run() {
			if((renderers?.Length??0)==0) {
				renderers=GetComponentsInChildren<Renderer>(false);
			}
			Begin();
			for(int i=0,imax=groups?.Length??0;i<imax;++i) {
				groups[i].Run();
			}
			End();
		}

		public virtual void Prepare(string key) {
			if(!this.IsActiveAndEnabled()) {return;}
			if(!string.IsNullOrEmpty(key)) {
				m_Index=System.Array.FindIndex(groups,x=>x.name==key);
			}else {
				m_Index=-1;
			}
		}

		public virtual void Run(Transform root) {
			if(!this.IsActiveAndEnabled()||root==null||m_Index<0) {return;}
			ModelPacker thiz=this;
			if(gameObject.IsPrefab()) {thiz=Object.Instantiate(thiz);}
			if(m_Index>=0) {thiz.groups[m_Index].root=root;}
			using(ListPool<Renderer>.Get(out var list)) {
				root.GetComponentsInChildren(true,list);
				list.RemoveAll((x)=>!x.IsActiveAndEnabled()||x.transform==root);
				thiz.renderers=list.ToArray();
			}
			thiz.Run();Prepare(null);
		}

		public virtual void Detach(Component comp) {
			if(comp!=null) {comp.transform.SetParent(null,false);}
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
