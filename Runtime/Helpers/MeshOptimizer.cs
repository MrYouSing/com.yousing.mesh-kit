using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshOptimizer
		:MonoTask
	{
		#region Nested Types

		[System.Flags]
		public enum Mode {
			 Clear                            = 0x0001
			,ClearBlendShapes                 = 0x0002
			,MarkDynamic                      = 0x0004
			,MarkModified                     = 0x0008
			,Optimize                         = 0x0010
			,OptimizeIndexBuffers             = 0x0020
			,OptimizeReorderVertexBuffer      = 0x0040
			,RecalculateBounds                = 0x0100
			,RecalculateNormals               = 0x0200
			,RecalculateTangents              = 0x0400
			,RecalculateUVDistributionMetrics = 0x0800
			,UploadMeshData                   = 0x1000
		}

		#endregion Nested Types

		#region Fields

		public Mesh mesh;
		public Mode mode=Mode.RecalculateBounds|Mode.RecalculateNormals;
		public bool markNoLongerReadable;

		#endregion Fields

		#region Methods

		public override void Run() {
			Mesh mesh=s_Mesh!=null?s_Mesh:this.mesh;
			if(mesh!=null) {
				// ="if((mode&Mode."&A1&")!=0) {mesh."&A1&"();}"
				if((mode&Mode.Clear)!=0) {mesh.Clear();}
				if((mode&Mode.ClearBlendShapes)!=0) {mesh.ClearBlendShapes();}
				if((mode&Mode.MarkDynamic)!=0) {mesh.MarkDynamic();}
				if((mode&Mode.MarkModified)!=0) {mesh.MarkModified();}
				if((mode&Mode.Optimize)!=0) {mesh.Optimize();}
				if((mode&Mode.OptimizeIndexBuffers)!=0) {mesh.OptimizeIndexBuffers();}
				if((mode&Mode.OptimizeReorderVertexBuffer)!=0) {mesh.OptimizeReorderVertexBuffer();}
				if((mode&Mode.RecalculateBounds)!=0) {mesh.RecalculateBounds();}
				if((mode&Mode.RecalculateNormals)!=0) {mesh.RecalculateNormals();}
				if((mode&Mode.RecalculateTangents)!=0) {mesh.RecalculateTangents();}
				if((mode&Mode.RecalculateUVDistributionMetrics)!=0) {mesh.RecalculateUVDistributionMetrics();}
				if((mode&Mode.UploadMeshData)!=0) {mesh.UploadMeshData(markNoLongerReadable);}
			}
		}

		#endregion Methods
	}
}
