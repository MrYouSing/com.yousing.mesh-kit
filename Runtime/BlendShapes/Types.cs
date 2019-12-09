using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class BlendShapeFrame {
		public int index;
		public float weight;
		public Vector3[] vertices;
		public Vector3[] normals;
		public Vector3[] tangents;

		public BlendShapeFrame(int numVertices) {
			vertices=new Vector3[numVertices];
			normals=new Vector3[numVertices];
			tangents=new Vector3[numVertices];
		}

		public BlendShapeFrame(Mesh mesh) {
			vertices=mesh.vertices;
			normals=mesh.normals;
			//tangents=mesh.tangents;
		}
	}

	public class BlendShapeWrapper {
		public Mesh mesh;
		public string name;
		public int index;
		public List<BlendShapeFrame> frames;

		public BlendShapeWrapper(Mesh mesh,string name) {
			this.mesh=mesh;
			this.name=name;
			Read();
		}

		public virtual void Read() {
			if(mesh==null) {return;}
			//
			if(frames==null) {
				frames=new List<BlendShapeFrame>();
			}else {
				frames.Clear();
			}
			index=mesh.GetBlendShapeIndex(name);
			if(index>=0) {
				int numVertices=mesh.vertexCount;
				BlendShapeFrame f;
				for(int i=0,imax=mesh.GetBlendShapeFrameCount(index);i<imax;++i) {
					f=new BlendShapeFrame(numVertices);
					f.index=i;
					f.weight=mesh.GetBlendShapeFrameWeight(index,i);
					mesh.GetBlendShapeFrameVertices(index,i,f.vertices,f.normals,f.tangents);
					frames.Add(f);
				}
			}
		}

		public virtual void Write() {
			if(mesh==null) {return;}
			//
			if(index>=0) {
				Debug.LogError("index>=0");
				return;
			}
			BlendShapeFrame f;
			for(int i=0,imax=frames?.Count??0;i<imax;++i) {
				f=frames[i];
				if(f!=null) {
					mesh.AddBlendShapeFrame(name,f.weight,f.vertices,f.normals,f.tangents);
				}
			}
		}
	}
}
