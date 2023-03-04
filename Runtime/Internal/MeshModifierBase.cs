using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshModifierBase:MonoTask {

		#region Fields

		[Header("Mesh Modifier")]
		public Transform target;
		public Mesh mesh;
		public int submesh=-1;
		public TextAsset text;
		public bool useClone=true;
		public bool autoApply=true;
		public MeshEvent onApply=new MeshEvent();

		#endregion Fields

		#region Methods

		public static void ForEach(IList<int> triangles,System.Action<int> action) {
			if(triangles==null||action==null) {
				return;
			}
			//
			HashSet<int> hs=new HashSet<int>();
			int t;
			for(int i=0,imax=triangles.Count;i<imax;++i) {
				t=triangles[i];
				if(!hs.Contains(t)) {
					hs.Add(t);
					action(t);
				}
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Bake")]
		protected virtual void Bake() {
			Renderer r=GetComponentInChildren<Renderer>();
			if(r==null) {r=GetComponentInParent<Renderer>();}
			target=r!=null?r.transform:transform;
			mesh=target.GetSharedMesh();
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif

		protected virtual void LoadJson(string key) {
			if(text!=null) {
				Newtonsoft.Json.UnityObjectConverter.transform=transform;
					var all=Newtonsoft.Json.Linq.JObject.Parse(text.text);
					var it=all[key];//if(it==null) {it=all[name];}
					if(it!=null) {Newtonsoft.Json.JsonConvert.PopulateObject(it.ToString(),this);}
				Newtonsoft.Json.UnityObjectConverter.transform=null;
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
			EndModifyMesh(mesh);
		}

		protected virtual int[] GetTriangles(Mesh mesh) {
			if(mesh!=null) {
				return (submesh>=0)
					?mesh.GetIndices(submesh)
					:mesh.triangles;
			}
			return null;
		}

		protected virtual void SetTriangles(Mesh mesh,int[] triangles) {
			if(mesh!=null) {
				if(submesh>=0) {
					mesh.SetIndices(triangles,MeshTopology.Triangles,submesh);
				}else {
					mesh.triangles=triangles;
				}
			}
		}

		protected virtual Mesh BeginModifyMesh() {
			Mesh mesh=s_Mesh!=null?s_Mesh:this.mesh;
			//
			if(mesh==null) {
			if(target!=null) {
				mesh=target.GetSharedMesh();
			}}
			//
			if(mesh!=null) {
			if(useClone) {
			if(mesh.name.IndexOf("(Clone)")<0) {
				mesh=Object.Instantiate(mesh);
			}}}
			//
			return mesh;
		}

		protected virtual void EndModifyMesh(Mesh mesh) {
			if(mesh!=null) {
				if(autoApply) {
				if(target!=null) {
					target.SetSharedMesh(mesh);
				}}
				onApply?.Invoke(mesh);
			}
		}

		#endregion Methods

	}
}
