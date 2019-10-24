using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public abstract class MeshGeneratorBase:MonoBehaviour {

		#region Fields

		public bool buildOnAwake=true;
		public Mesh mesh;
		public GameObject target;

		[System.NonSerialized]protected Vector3[] m_Vertices;
		[System.NonSerialized]protected Vector2[] m_UVs;
		[System.NonSerialized]protected Color[] m_Colors;
		[System.NonSerialized]protected int[] m_Triangles;

		#endregion Fields

		#region Unity Messages

		protected virtual void Awake() { 
			if(target==null) {
				target=gameObject;
			}
			if(buildOnAwake) {
				BuildMesh();
			}
		}

		#endregion Unity Messages

		#region Methods

		protected virtual void EnsureSize<T>(ref T[] array,int size) {
			if(array==null||array.Length!=size) {
				array=new T[size];
			}
		}

		[ContextMenu("Build Mesh")]
		public virtual void BuildMesh() {
			OnPreBuildMesh();
			OnBuildMesh();
			OnPostBuildMesh();
		}

		protected virtual void OnPreBuildMesh() {
		}

		protected virtual void OnBuildMesh() {
			//
			if(mesh==null) {
				mesh=new Mesh();
				mesh.name=name;
			}else {
				mesh.Clear();
			}
			//
			if(m_Vertices!=null) {mesh.vertices=m_Vertices;}
			if(m_UVs!=null) {mesh.uv=m_UVs;}
			if(m_Colors!=null) {mesh.colors=m_Colors;}
			if(m_Triangles!=null) {mesh.triangles=m_Triangles;}
			//
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		protected virtual void OnPostBuildMesh() {
			if(target!=null) {
				target.SetSharedMesh(mesh);
			}
		}

		#endregion Methods

	}

}
