/* <!-- Macro.Table VertexAttribute
Position,Vector3,vertices,
Normal,Vector3,normals,
Tangent,Vector4,tangents,
Color,Color,colors,
TexCoord0,Vector2,uv,
TexCoord1,Vector2,uv2,
TexCoord2,Vector2,uv3,
TexCoord3,Vector2,uv4,
TexCoord4,Vector2,uv5,
TexCoord5,Vector2,uv6,
TexCoord6,Vector2,uv7,
TexCoord7,Vector2,uv8,
BlendWeight,BoneWeight,boneWeights,
 Macro.End --> */

/* <!-- Macro.Copy
		[System.Flags]
		public enum Mask {
 Macro.End -->*/
/* <!-- Macro.Call  VertexAttribute
			{0}=1<<$(Table.Row),
 Macro.End --> */
/* <!-- Macro.Copy
			All=-1
		}

 Macro.End --> */
/* <!-- Macro.Call  VertexAttribute
		[System.NonSerialized]public {1}[] {2}=null;
 Macro.End -->*/
/* <!-- Macro.Copy

		public virtual void Clear() {
 Macro.End --> */
/* <!-- Macro.Call  VertexAttribute
			{2}=null;
 Macro.End --> */
/* <!-- Macro.Copy
		}

		public virtual System.Action<int> GetAction(VertexAttribute attribute) {
			switch(attribute) {
 Macro.End --> */
/* <!-- Macro.Call  VertexAttribute
				case VertexAttribute.{0}:
					if({2}==null) {{{2}=mesh.{2};}}
					return (x)=>{{
						if((printMask&Mask.{0})!=0) {{Print("	{0}:"+{2}[x].ToString(format));}}
						if((outputMask&Mask.{0})!=0) {{list.Add({2}[x]);}}
					}};
 Macro.End --> */
/* <!-- Macro.Copy
			}
			return null;
		}

 Macro.End --> */
/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace YouSingStudio.MeshKit {
	public class MeshVertexProbe
		:MonoTask
	{
// <!-- Macro.Patch AutoGen
		[System.Flags]
		public enum Mask {
			Position=1<<0,
			Normal=1<<1,
			Tangent=1<<2,
			Color=1<<3,
			TexCoord0=1<<4,
			TexCoord1=1<<5,
			TexCoord2=1<<6,
			TexCoord3=1<<7,
			TexCoord4=1<<8,
			TexCoord5=1<<9,
			TexCoord6=1<<10,
			TexCoord7=1<<11,
			BlendWeight=1<<12,
			All=-1
		}

		[System.NonSerialized]public Vector3[] vertices=null;
		[System.NonSerialized]public Vector3[] normals=null;
		[System.NonSerialized]public Vector4[] tangents=null;
		[System.NonSerialized]public Color[] colors=null;
		[System.NonSerialized]public Vector2[] uv=null;
		[System.NonSerialized]public Vector2[] uv2=null;
		[System.NonSerialized]public Vector2[] uv3=null;
		[System.NonSerialized]public Vector2[] uv4=null;
		[System.NonSerialized]public Vector2[] uv5=null;
		[System.NonSerialized]public Vector2[] uv6=null;
		[System.NonSerialized]public Vector2[] uv7=null;
		[System.NonSerialized]public Vector2[] uv8=null;
		[System.NonSerialized]public BoneWeight[] boneWeights=null;

		public virtual void Clear() {
			vertices=null;
			normals=null;
			tangents=null;
			colors=null;
			uv=null;
			uv2=null;
			uv3=null;
			uv4=null;
			uv5=null;
			uv6=null;
			uv7=null;
			uv8=null;
			boneWeights=null;
		}

		public virtual System.Action<int> GetAction(VertexAttribute attribute) {
			switch(attribute) {
				case VertexAttribute.Position:
					if(vertices==null) {vertices=mesh.vertices;}
					return (x)=>{
						if((printMask&Mask.Position)!=0) {Print("	Position:"+vertices[x].ToString(format));}
						if((outputMask&Mask.Position)!=0) {list.Add(vertices[x]);}
					};
				case VertexAttribute.Normal:
					if(normals==null) {normals=mesh.normals;}
					return (x)=>{
						if((printMask&Mask.Normal)!=0) {Print("	Normal:"+normals[x].ToString(format));}
						if((outputMask&Mask.Normal)!=0) {list.Add(normals[x]);}
					};
				case VertexAttribute.Tangent:
					if(tangents==null) {tangents=mesh.tangents;}
					return (x)=>{
						if((printMask&Mask.Tangent)!=0) {Print("	Tangent:"+tangents[x].ToString(format));}
						if((outputMask&Mask.Tangent)!=0) {list.Add(tangents[x]);}
					};
				case VertexAttribute.Color:
					if(colors==null) {colors=mesh.colors;}
					return (x)=>{
						if((printMask&Mask.Color)!=0) {Print("	Color:"+colors[x].ToString(format));}
						if((outputMask&Mask.Color)!=0) {list.Add(colors[x]);}
					};
				case VertexAttribute.TexCoord0:
					if(uv==null) {uv=mesh.uv;}
					return (x)=>{
						if((printMask&Mask.TexCoord0)!=0) {Print("	TexCoord0:"+uv[x].ToString(format));}
						if((outputMask&Mask.TexCoord0)!=0) {list.Add(uv[x]);}
					};
				case VertexAttribute.TexCoord1:
					if(uv2==null) {uv2=mesh.uv2;}
					return (x)=>{
						if((printMask&Mask.TexCoord1)!=0) {Print("	TexCoord1:"+uv2[x].ToString(format));}
						if((outputMask&Mask.TexCoord1)!=0) {list.Add(uv2[x]);}
					};
				case VertexAttribute.TexCoord2:
					if(uv3==null) {uv3=mesh.uv3;}
					return (x)=>{
						if((printMask&Mask.TexCoord2)!=0) {Print("	TexCoord2:"+uv3[x].ToString(format));}
						if((outputMask&Mask.TexCoord2)!=0) {list.Add(uv3[x]);}
					};
				case VertexAttribute.TexCoord3:
					if(uv4==null) {uv4=mesh.uv4;}
					return (x)=>{
						if((printMask&Mask.TexCoord3)!=0) {Print("	TexCoord3:"+uv4[x].ToString(format));}
						if((outputMask&Mask.TexCoord3)!=0) {list.Add(uv4[x]);}
					};
				case VertexAttribute.TexCoord4:
					if(uv5==null) {uv5=mesh.uv5;}
					return (x)=>{
						if((printMask&Mask.TexCoord4)!=0) {Print("	TexCoord4:"+uv5[x].ToString(format));}
						if((outputMask&Mask.TexCoord4)!=0) {list.Add(uv5[x]);}
					};
				case VertexAttribute.TexCoord5:
					if(uv6==null) {uv6=mesh.uv6;}
					return (x)=>{
						if((printMask&Mask.TexCoord5)!=0) {Print("	TexCoord5:"+uv6[x].ToString(format));}
						if((outputMask&Mask.TexCoord5)!=0) {list.Add(uv6[x]);}
					};
				case VertexAttribute.TexCoord6:
					if(uv7==null) {uv7=mesh.uv7;}
					return (x)=>{
						if((printMask&Mask.TexCoord6)!=0) {Print("	TexCoord6:"+uv7[x].ToString(format));}
						if((outputMask&Mask.TexCoord6)!=0) {list.Add(uv7[x]);}
					};
				case VertexAttribute.TexCoord7:
					if(uv8==null) {uv8=mesh.uv8;}
					return (x)=>{
						if((printMask&Mask.TexCoord7)!=0) {Print("	TexCoord7:"+uv8[x].ToString(format));}
						if((outputMask&Mask.TexCoord7)!=0) {list.Add(uv8[x]);}
					};
				case VertexAttribute.BlendWeight:
					if(boneWeights==null) {boneWeights=mesh.boneWeights;}
					return (x)=>{
						if((printMask&Mask.BlendWeight)!=0) {Print("	BlendWeight:"+boneWeights[x].ToString(format));}
						if((outputMask&Mask.BlendWeight)!=0) {list.Add(boneWeights[x]);}
					};
			}
			return null;
		}

// Macro.Patch -->
		#region Fields

		public Mesh mesh;
		public int submesh=-1;
		public new Collider collider;
		public Mask printMask=Mask.All;
		public string format="0.00";
		public Mask outputMask=Mask.All;
		public UnityEvent<IEnumerable> output;

		[System.NonSerialized]public System.Action<int> action;
		[System.NonSerialized]public StringBuilder stringBuilder;
		[System.NonSerialized]public List<object> list;

		#endregion Fields

		#region Methods

		protected virtual void Run(int index) {
			if(collider.OverlapPoint(vertices[index])) {
				action(index);
			}
		}

		public override void Run() {
			if(mesh!=null&&collider!=null) {
				action=GetAction();
				using(GenericPool<StringBuilder>.Get(out stringBuilder)) {
				using(ListPool<object>.Get(out list)) {
					stringBuilder.Clear();vertices=mesh.vertices;
					if(submesh>=0){MeshModifierBase.ForEach(mesh.GetTriangles(submesh),Run);}
					else {for(int i=0,imax=mesh.vertexCount;i<imax;++i) {Run(i);}}
					//
					Debug.Log(stringBuilder.ToString());
					output?.Invoke(list);
				}}
				action=null;
				stringBuilder=null;
				list=null;
				Clear();
			}
		}

		protected virtual void Print(string x)=>stringBuilder?.AppendLine(x);

		protected virtual System.Action<int> GetAction() {
			System.Action<int> action=(x)=>Print("Index:"+x.ToString());
			VertexAttributeDescriptor[] descriptors=mesh.GetVertexAttributes();
			for(int i=0,imax=descriptors?.Length??0;i<imax;++i) {
				action+=GetAction(descriptors[i].attribute);
			}
			return action;
		}

		#endregion Methods
	}
}
