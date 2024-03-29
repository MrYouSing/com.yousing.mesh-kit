﻿using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public abstract class MeshGeneratorBase:MonoBehaviour {

		#region Fields

		public bool buildOnAwake=true;
		public bool recalculate=true;
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

#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()=>DrawGizmos(false);

		protected virtual void OnDrawGizmosSelected()=>DrawGizmos(true);

		protected virtual void DrawGizmos(bool selected) {
			Color c=Gizmos.color;
			Matrix4x4 m=Gizmos.matrix;
				Gizmos.color=selected?Color.green:Color.gray;
				Gizmos.matrix=transform.localToWorldMatrix;
				InternalDrawGizmos(selected);
			Gizmos.color=c;
			Gizmos.matrix=m;
		}

		protected virtual void InternalDrawGizmos(bool selected) {
			if(selected) {Gizmos.DrawMesh(mesh);}
		}
#endif

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
			if(recalculate) {
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
			}
		}

		protected virtual void OnPostBuildMesh() {
			if(target!=null) {
				target.SetSharedMesh(mesh);
			}
		}

		#endregion Methods

	}

}
