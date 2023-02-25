/* <!-- Macro.Table VertexAttribute
Position,vertices,property.Scale,
Normal,normals,null,
Tangent,tangents,null,
Color,colors,null,
TexCoord0,uv,property.Scale,
TexCoord1,uv2,property.Scale,
TexCoord2,uv3,property.Scale,
TexCoord3,uv4,property.Scale,
TexCoord4,uv5,property.Scale,
TexCoord5,uv6,property.Scale,
TexCoord6,uv7,property.Scale,
TexCoord7,uv8,property.Scale,
BlendWeight,boneWeights,null,
BlendIndices,triangles,null,
 Macro.End -->*/
/* <!-- Macro.Call  VertexAttribute
					case VertexAttribute.{0}:mesh.{1}=Modify(mesh.{1},property.GetText(),{2});break;
 Macro.End -->*/
/* <!-- Macro.Patch
,VertexAttribute
 Macro.End -->*/
using System.Text;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace YouSingStudio.MeshKit {
	public class MeshPropertyModifier
		:MeshModifierBase
	{
		#region Nested Types

		[System.Serializable]
		public class Property {
			public VertexAttribute attribute;
			public float scale;
			public TextAsset asset;
			[TextArea]
			public string text;

			public virtual string GetText()=>asset!=null?asset.text:text;
			public virtual Vector2 Scale(Vector2 x)=>x*scale;
			public virtual Vector3 Scale(Vector3 x)=>x*scale;
		}

		#endregion Nested Types

		#region Fields

		public Property[] properties;

		#endregion Fields

		#region Methods

		protected virtual void Repeat<T>(T[] lhs,T[] rhs) {
			int j,jmax=rhs?.Length??0,i=0,imax=lhs?.Length??0;
			while(i<imax) {
				j=0;while(i<imax&&j<jmax) {
					lhs[i++]=rhs[j++];
				}
			}
		}

		public override void Run() {
			Mesh mesh=BeginModifyMesh();
				if(mesh!=null) {
					for(int i=0,imax=properties?.Length??0;i<imax;++i) {
						Modify(mesh,properties[i]);
					}
				}
			EndModifyMesh(mesh);
		}

		public virtual T[] Modify<T>(T[] data,string text,System.Func<T,T> func=null) {
			if(data!=null) {
				if(!string.IsNullOrEmpty(text)) {
					T[] tmp=Newtonsoft.Json.JsonConvert.DeserializeObject<T[]>(text);
					if(tmp!=null) {
						if(func!=null) {int i=tmp.Length;while(i-->0) {tmp[i]=func(tmp[i]);}}
						if(tmp.Length<data.Length) {Repeat(data,tmp);}
						else {data=tmp;}
					}
				}else{
					using(GenericPool<StringBuilder>.Get(out var sb)) {
						sb.Clear();sb.AppendLine(":[");
						int i=0,imax=data?.Length??0;for(;i<imax;++i) {
							if(i>0) {sb.AppendLine(",");}sb.Append("  ");
							sb.Append(JsonUtility.ToJson(data[i]));
						}
						if(imax>0) {sb.AppendLine();}sb.AppendLine("]");
						Debug.Log(sb.ToString());
					}
				}
			}
			return data;
		}

		public virtual void Modify(Mesh mesh,Property property) {
			if(mesh!=null&&property!=null) {
				switch(property.attribute) {
// <!-- Macro.Patch VertexAttribute
					case VertexAttribute.Position:mesh.vertices=Modify(mesh.vertices,property.GetText(),property.Scale);break;
					case VertexAttribute.Normal:mesh.normals=Modify(mesh.normals,property.GetText(),null);break;
					case VertexAttribute.Tangent:mesh.tangents=Modify(mesh.tangents,property.GetText(),null);break;
					case VertexAttribute.Color:mesh.colors=Modify(mesh.colors,property.GetText(),null);break;
					case VertexAttribute.TexCoord0:mesh.uv=Modify(mesh.uv,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord1:mesh.uv2=Modify(mesh.uv2,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord2:mesh.uv3=Modify(mesh.uv3,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord3:mesh.uv4=Modify(mesh.uv4,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord4:mesh.uv5=Modify(mesh.uv5,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord5:mesh.uv6=Modify(mesh.uv6,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord6:mesh.uv7=Modify(mesh.uv7,property.GetText(),property.Scale);break;
					case VertexAttribute.TexCoord7:mesh.uv8=Modify(mesh.uv8,property.GetText(),property.Scale);break;
					case VertexAttribute.BlendWeight:mesh.boneWeights=Modify(mesh.boneWeights,property.GetText(),null);break;
					case VertexAttribute.BlendIndices:mesh.triangles=Modify(mesh.triangles,property.GetText(),null);break;
// Macro.Patch -->
				}
			}
		}

		#endregion Methods
	}
}
