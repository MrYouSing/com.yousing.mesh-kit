using UnityEngine;

namespace YouSingStudio.MeshKit {

	public static partial class UnityExtension {
		#region Methods

		public static Vector2 Matrix22_Mul(Vector4 m22,Vector2 v2) {
			return new Vector2(
				m22.x*v2.x+m22.y*v2.y,
				m22.z*v2.x+m22.w*v2.y
			);
		}

		public static Vector3 Division(Vector3 lhs,Vector3 rhs) {
			return new Vector3(
				lhs.x/rhs.x,
				lhs.y/rhs.y,
				lhs.z/rhs.z
			);
		}

		public static Color ToColor(this int v){
			return new Color(
				((v>>16)&0xFF)/255.0f,
				((v>> 8)&0xFF)/255.0f,
				((v>> 0)&0xFF)/255.0f,
				((v>>24)&0xFF)/255.0f
			);
		}

		public static bool IsActiveAndEnabled(this Renderer thiz) {
			if(thiz!=null) {
				return thiz.gameObject.activeInHierarchy&&thiz.enabled;
			}
			return false;
		}

		public static T AddMissingComponent<T>(this GameObject thiz) where T:Component {
			T comp=thiz.GetComponent<T>();
			if(comp==null) {
				comp=thiz.AddComponent<T>();
			}
			return comp;
		}

		public static Component AddMissingComponent(this GameObject thiz,System.Type type){
			if(type==null) {
				return null;
			}
			Component comp=thiz.GetComponent(type);
			if(comp==null) {
				comp=thiz.AddComponent(type);
			}
			return comp;
		}

		public static T AddMissingComponent<T>(this Component thiz) where T:Component {
			T comp=thiz.GetComponent<T>();
			if(comp==null) {
				comp=thiz.gameObject.AddComponent<T>();
			}
			return comp;
		}

		public static Component AddMissingComponent(this Component thiz,System.Type type){
			if(type==null) {
				return null;
			}
			Component comp=thiz.GetComponent(type);
			if(comp==null) {
				comp=thiz.gameObject.AddComponent(type);
			}
			return comp;
		}

		public static void CheckComponent<T>(this GameObject thiz,ref T comp,int searchDir=0) where T:Component {
			if(comp==null) {
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent<T>();
					break;
					case 1:
						comp=thiz.GetComponentInChildren<T>();
					break;
					case 0:
					default:
						comp=thiz.GetComponent<T>();
					break;
				}
			}
		}
		public static void CheckComponent(this GameObject thiz,ref Component comp,System.Type type,int searchDir=0) {
			if(comp==null) {
				if(type==null) {return;}
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent(type);
					break;
					case 1:
						comp=thiz.GetComponentInChildren(type);
					break;
					case 0:
					default:
						comp=thiz.GetComponent(type);
					break;
				}
			}
		}
		public static void CheckComponent<T>(this Component thiz,ref T comp,int searchDir=0) where T:Component {
			if(comp==null) {
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent<T>();
					break;
					case 1:
						comp=thiz.GetComponentInChildren<T>();
					break;
					case 0:
					default:
						comp=thiz.GetComponent<T>();
					break;
				}
			}
		}
		public static void CheckComponent(this Component thiz,ref Component comp,System.Type type,int searchDir=0) {
			if(comp==null) {
				if(type==null) {return;}
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent(type);
					break;
					case 1:
						comp=thiz.GetComponentInChildren(type);
					break;
					case 0:
					default:
						comp=thiz.GetComponent(type);
					break;
				}
			}
		}

		#endregion Methods

		#region Experimentals

		public static readonly string[] SPLIT_DIR=new string[2]{"/","\\"};

		/// <summary>
		/// 
		/// </summary>
		public static Transform FindEx(this Transform thiz,string path) {
			if(string.IsNullOrEmpty(path)) {
				return thiz;
			}
			if(thiz==null) {
				GameObject go=GameObject.Find(path);
				return go==null?null:go.transform;
			}
			//
			Transform t;
			int index;
			string[] cmds=path.Split(SPLIT_DIR,System.StringSplitOptions.RemoveEmptyEntries);
			for(int i=0,imax=cmds.Length;i<imax;++i) {
				t=null;
				if(cmds[i]=="..") {
					t=thiz.parent;
				}else if(cmds[i].StartsWith(".@")){
					if(!int.TryParse(cmds[i].Substring(2),out index)) {
						index=-1;
					}
					if(index>=0&&index<thiz.childCount) {
						t=thiz.GetChild(index);
					}
				}else {
					t=thiz.Find(cmds[i]);
				}
				//
				if(t!=null) {
					thiz=t;
				}else {
					break;
				}
			}
			return thiz;
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetPath(this Transform thiz,Transform root = null) {
			if(thiz==null) {
				return "";
			}
			// It may cause many GC calls in this implement.
			// TODO : Here is no method named "Reverse()" in System.Text.StringBuilder.
			string path = null;
			while(thiz!=root&&thiz!=null) {
				if(string.IsNullOrEmpty(path)) {
					path=thiz.name;
				} else {
					path=thiz.name+"/"+path;
				}
				thiz=thiz.parent;
			}
			return path;
		}

		#endregion Experimentals

	}
}
