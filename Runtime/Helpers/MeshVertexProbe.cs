/* <!-- Macro.Table VertexAttribute
Position,vertices,format,
Normal,normals,format,
Tangent,tangents,format,
Color,colors,format,
TexCoord0,uv,format,
TexCoord1,uv2,format,
TexCoord2,uv3,format,
TexCoord3,uv4,format,
TexCoord4,uv5,format,
TexCoord5,uv6,format,
TexCoord6,uv7,format,
TexCoord7,uv8,format,
BlendWeight,boneWeights,,
 Macro.End -->*/
/* <!-- Macro.Call  VertexAttribute
				case VertexAttribute.{0}: {{
					var tmp=mesh.{1};return (x)=>Print("	{0}:"+tmp[x].ToString({2}));
				}};break;
 Macro.End -->*/
/* <!-- Macro.Patch
,VertexAttribute
 Macro.End -->*/
using System.Text;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace YouSingStudio.MeshKit {
	public class MeshVertexProbe
		:MonoTask
	{
		#region Fields

		public Mesh mesh;
		public new Collider collider;
		public string format="0.00";
		
		[System.NonSerialized]public StringBuilder stringBuilder;

		#endregion Fields

		#region Methods

		public override void Run() {
			if(mesh!=null) {
				var printer=CreatePrinter();
				Vector3[] vertices=mesh.vertices;
				using(GenericPool<StringBuilder>.Get(out stringBuilder)) {
					stringBuilder.Clear();
					for(int i=0,imax=vertices?.Length??0;i<imax;++i) {
						if(collider.OverlapPoint(vertices[i])) {
							printer?.Invoke(i);
						}
					}
					Debug.Log(stringBuilder.ToString());
				}stringBuilder=null;
			}
		}

		protected virtual void Print(string x)=>stringBuilder?.AppendLine(x);

		protected virtual System.Action<int> CreatePrinter(VertexAttribute attribute) {
			switch(attribute) {
// <!-- Macro.Patch VertexAttribute
				case VertexAttribute.Position: {
					var tmp=mesh.vertices;return (x)=>Print("	Position:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.Normal: {
					var tmp=mesh.normals;return (x)=>Print("	Normal:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.Tangent: {
					var tmp=mesh.tangents;return (x)=>Print("	Tangent:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.Color: {
					var tmp=mesh.colors;return (x)=>Print("	Color:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord0: {
					var tmp=mesh.uv;return (x)=>Print("	TexCoord0:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord1: {
					var tmp=mesh.uv2;return (x)=>Print("	TexCoord1:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord2: {
					var tmp=mesh.uv3;return (x)=>Print("	TexCoord2:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord3: {
					var tmp=mesh.uv4;return (x)=>Print("	TexCoord3:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord4: {
					var tmp=mesh.uv5;return (x)=>Print("	TexCoord4:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord5: {
					var tmp=mesh.uv6;return (x)=>Print("	TexCoord5:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord6: {
					var tmp=mesh.uv7;return (x)=>Print("	TexCoord6:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.TexCoord7: {
					var tmp=mesh.uv8;return (x)=>Print("	TexCoord7:"+tmp[x].ToString(format));
				};break;
				case VertexAttribute.BlendWeight: {
					var tmp=mesh.boneWeights;return (x)=>Print("	BlendWeight:"+tmp[x].ToString());
				};break;
// Macro.Patch -->
			}
			return null;
		}

		protected virtual System.Action<int> CreatePrinter() {
			System.Action<int> action=(x)=>Print("Index:"+x.ToString());
			VertexAttributeDescriptor[] descriptors=mesh.GetVertexAttributes();
			for(int i=0,imax=descriptors?.Length??0;i<imax;++i) {
				action+=CreatePrinter(descriptors[i].attribute);
			}
			return action;
		}

		#endregion Methods
	}
}
