using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YouSingStudio.MeshKit {
	public class MeshExporter:MonoTask {
		#region Fields

		public Object source;
		public Object destination;
		public string path;

		#endregion Fields

		#region Methods

		public override void Run() {
#if UNITY_EDITOR
			string path=this.path;
			if(string.IsNullOrEmpty(path)) {
				if(destination==null) {return;}
				if(!AssetDatabase.IsSubAsset(destination)) {
					path=AssetDatabase.GetAssetPath(destination);
				}
			}
			Mesh mesh=source as Mesh;
			if(mesh==null) {
				GameObject go=source as GameObject;
				if(go==null) {return;}
				mesh=go.GetSharedMesh();
				if(mesh==null) {return;}
				//
				mesh=Object.Instantiate(mesh);
				go.SetSharedMesh(mesh);
			}
			//
			if(!string.IsNullOrEmpty(path)) {
				AssetDatabase.CreateAsset(mesh,path);
			}else {
				Mesh dst=destination as Mesh;
				if(dst!=null) {
					Mesh.ApplyAndDisposeWritableMeshData(Mesh.AcquireReadOnlyMeshData(mesh),dst);
					dst.RecalculateBounds();EditorUtility.SetDirty(dst);
				}
			}
			AssetDatabase.Refresh();
#endif
		}

		#endregion Methods
	}
}
