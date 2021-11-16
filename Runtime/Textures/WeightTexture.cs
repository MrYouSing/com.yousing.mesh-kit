using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract class WeightTexture<T>
		:ScriptableTexture
	{
		#region Fields
		
		public Mesh mesh;
		public int radius;

		[System.NonSerialized]public Vector3[] vertices;
		[System.NonSerialized]public Vector2[] uv;

		#endregion Fields

		#region Methods

		protected abstract T FromColor(Color c);
		protected abstract Color ToColor(Color c,T v);

		protected override void Init() {
			if(m_IsInited) {
				return;
			}
			base.Init();
			//
			if(mesh!=null) {
				vertices=mesh.vertices;
				uv=mesh.uv;
			}
		}

		public virtual int IndexOf(Vector3 v) {
			if(!m_IsInited) {Init();}
			//
			if(vertices!=null) {
				return System.Array.IndexOf(vertices,v);
			}
			return -1;
		}

		public virtual Vector2 GetUV(int index) {
			if(!m_IsInited) {Init();}
			//
			if(uv!=null) {
				return uv[index];
			}
			if(texture!=null) {
				return new Vector2((index%size.x)/(float)size.x,(index/size.x)/(float)size.y);
			}
			return Vector2.zero;
		}

		public virtual T GetWeight(int index) {
			if(!m_IsInited) {Init();}
			//
			if(texture!=null) {
				Vector2 v=GetUV(index);
				return FromColor(texture.GetPixelBilinear(v.x,v.y));
			}
			return default;
		}

		public virtual void SetWeight(int index,T value) {
			if(!m_IsInited) {Init();}
			//
			if(texture!=null) {
				Vector2 v=GetUV(index);
				Color c=ToColor(texture.GetPixelBilinear(v.x,v.y),value);
				v=Vector2.Scale(v,size);
				if(radius<=1) {
					texture.SetPixel((int)v.x,(int)v.y,c);
				}else {
					for(int i=0;i<radius;++i) {
					for(int j=0;j<radius;++j) {
						texture.SetPixel((int)v.x-radius+j,(int)v.y-radius+i,c);
					}}
				}
			}
		}


		#endregion Methods
	}
	
	public class WeightTexture
		:WeightTexture<float>
	{
		#region Fields

		public int channel;
#if MAGICACLOTH_DEBUG
		public MagicaCloth.SelectionData magicaData;
		public int magicaId;
#endif

		#endregion Fields

		#region Methods

		protected override float FromColor(Color c)=>c[channel];
		protected override Color ToColor(Color c,float v) {
			c[channel]=v;
			return c;
		}
		
#if MAGICACLOTH_DEBUG
		[ContextMenu("Read Magica")]
		public virtual void ReadMagica() {
			var data=magicaData.selectionList[magicaId];
			for(int i=0,imax=data.selectData?.Count??0;i<imax;++i) {
				SetWeight(i,data.selectData[i]*.25f);
			}
			Flush();
		}

		[ContextMenu("Write Magica")]
		public virtual void WriteMagica() {
			var data=magicaData.selectionList[magicaId];
			for(int i=0,imax=data.selectData?.Count??0;i<imax;++i) {
				data.selectData[i]=(int)(GetWeight(i)*4);
			}
		}
#endif

		#endregion Methods
	}
}