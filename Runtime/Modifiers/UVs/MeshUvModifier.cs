﻿using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {

	public class MeshUvModifier:MeshModifierBase {

		#region Fields

		public static List<int> s_TrianglesHelper=new List<int>();

		public Sprite sprite;
		public Vector2 offset=Vector2.zero;
		public Vector2 scale=Vector2.one;

		#endregion Fields

		#region Methods

		[ContextMenu("Run")]
		public override void Run() {
			Vector2 offset=this.offset;
			Vector2 scale=this.scale;
			if(sprite!=null) {
				Rect rect=sprite.rect;
				Texture texture=sprite.texture;
				Vector2Int size=(texture!=null)?new Vector2Int(texture.width,texture.height):Vector2Int.zero;
				//
				offset=new Vector2(rect.x/size.x,rect.y/size.y);
				scale=new Vector2(rect.width/size.x,rect.height/size.y);
			}
			//
			Mesh mesh=BeginModifyMesh();
			if(mesh!=null) {
				Vector2[] uvs=mesh.uv;
				Vector2 uv;
				if(submesh<0) {
					for(int i=0,imax=uvs?.Length??0;i<imax;++i) {
						uv=uvs[i];
							uv.Set(
								scale.x*uv.x+offset.x,
								scale.y*uv.y+offset.y
							);
						uvs[i]=uv;
					}
				}else {
					int[] triangles=GetTriangles(mesh);
					var list=s_TrianglesHelper;
					for(int i=0,imax=triangles?.Length??0,t;i<imax;++i) {
						t=triangles[i];
						if(list.IndexOf(t)==-1) {
							list.Add(t);
							//
							uv=uvs[t];
								uv.Set(
									scale.x*uv.x+offset.x,
									scale.y*uv.y+offset.y
								);
							uvs[t]=uv;
						}
					}
					s_TrianglesHelper.Clear();
				}
				mesh.uv=uvs;
			}
			EndModifyMesh(mesh);
		}

		#endregion Methods

	}
}
