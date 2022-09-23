using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public abstract class StructSet<T>
		:MonoBehaviour
		,IEnumerable<T>
		where T:struct
	{
		#region Nested Types

#if UNITY_EDITOR
		public class StructEditor:UnityEditor.Editor {
			public static StructEditor current;
			public Vector3 point;

			public virtual void DrawLabel(Vector3 position,string label) {
				UnityEditor.Handles.Label(position,label);
				point=position;
			}

			public virtual void OnSceneGUI() {
				current=this;
				Matrix4x4 m=UnityEditor.Handles.matrix;
					var ss=target as StructSet<T>;T tmp;Vector3 p;
					if(ss.root!=null) {
						UnityEditor.Handles.matrix=ss.root.localToWorldMatrix;
					}
					for(int i=0,imax=ss.m_Items?.Count??0;i<imax;++i) {
						p=point;
						tmp=ss.m_Items[i];
							if(ss.OnSceneGUI(i,ref tmp)) {
								UnityEditor.Undo.RecordObject(ss,"Edit");
								UnityEditor.EditorUtility.SetDirty(ss);
							}
						ss.m_Items[i]=tmp;
						if(i>0&&point!=p) {UnityEditor.Handles.DrawLine(p,point);}
					}
				UnityEditor.Handles.matrix=m;
				current=null;
			}
		}
#endif

		#endregion Nested Types

		#region Fields

		public Transform root;
		[SerializeField]protected List<T> m_Items=new List<T>();

		[System.NonSerialized]public IList<T> items;
		[System.NonSerialized]protected Matrix4x4 m_Matrix;

		#endregion Fields

		#region Methods

		public static void Fill<TSrc,TDst>(ref IList<TDst> thiz,IList<TSrc> value,System.Func<TSrc,TDst> func) {
			int i=0,imax=value?.Count??0;
			if(imax>0) {
				if(thiz==null) {thiz=new List<TDst>(imax);}
				int icnt=thiz.Count;
				for(;i<imax;++i) {
					if(i<icnt) {thiz[i]=func(value[i]);}
					else {thiz.Add(func(value[i]));}
				}
				while(icnt-->imax) {thiz.RemoveAt(icnt);}
			}else {
				thiz?.Clear();
			}
		}

#if UNITY_EDITOR
		public virtual bool OnSceneGUI(int index,ref T item) {
			return false;
		}
#endif

		public virtual T Multiply(T item) {
			return item;
		}

		public virtual IList<T> GetItems() {
			if(root!=null) {
				m_Matrix=root.localToWorldMatrix;
				Fill(ref items,m_Items,Multiply);
			}else {
				items=m_Items;
			}
			return items;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()=>GetItems()?.GetEnumerator()??null;
		IEnumerator IEnumerable.GetEnumerator()=>GetItems()?.GetEnumerator()??null;

		#endregion Methods
	}
}
