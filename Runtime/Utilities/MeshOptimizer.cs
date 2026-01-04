/* <!-- Macro.Table Mesh
Clear,
ClearBlendShapes,
MarkDynamic,
MarkModified,
Optimize,
OptimizeIndexBuffers,
OptimizeReorderVertexBuffer,
RecalculateBounds,
RecalculateNormals,
RecalculateTangents,
RecalculateUVDistributionMetrics,
UploadMeshData,
 Macro.End --> */

/* <!-- Macro.Call  Mesh
			{0}=1<<$(Table.Row),
 Macro.End --> */
/* <!-- Macro.Patch
,Mode
 Macro.End --> */
/* <!-- Macro.Call  Mesh
				if((mode&Mode.{0})!=0) {{mesh.{0}();}}
 Macro.End --> */
/* <!-- Macro.Replace
UploadMeshData(),UploadMeshData(markNoLongerReadable)
 Macro.End --> */
/* <!-- Macro.Patch
,Run
 Macro.End --> */
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshOptimizer
		:MonoTask
	{
		#region Nested Types

		[System.Flags]
		public enum Mode {
// <!-- Macro.Patch Mode
			Clear=1<<0,
			ClearBlendShapes=1<<1,
			MarkDynamic=1<<2,
			MarkModified=1<<3,
			Optimize=1<<4,
			OptimizeIndexBuffers=1<<5,
			OptimizeReorderVertexBuffer=1<<6,
			RecalculateBounds=1<<7,
			RecalculateNormals=1<<8,
			RecalculateTangents=1<<9,
			RecalculateUVDistributionMetrics=1<<10,
			UploadMeshData=1<<11,
// Macro.Patch -->
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
// <!-- Macro.Patch Run
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
// Macro.Patch -->
			}
		}

		#endregion Methods
	}
}
