/* <!-- Macro.Copy
	[System.Serializable]
 Macro.End --> */
/* <!-- Macro.Copy File
:Packages/com.unity.vectorgraphics/Runtime/Matrix2D.cs,5~357
 Macro.End --> */
/* <!-- Macro.Replace
IndexOutOfRangeException,System.IndexOutOfRangeException
    ,	
 Macro.End --> */
/* <!-- Macro.Patch
,AutoGen
 Macro.End --> */
using System.Collections.Generic;
using UnityEngine;

namespace YouSingStudio.MeshKit {
// <!-- Macro.Patch AutoGen
	[System.Serializable]
	/// <summary>A 2x3 transformation matrix used for 2D operations.</summary>
	public struct Matrix2D
	{
		// memory layout:
		//
		//				row no (=vertical)
		//			   |  0   1   2
		//			---+------------
		//			0  | m00 m10  0
		// column no  1  | m01 m11  0
		// (=horiz)   2  | m02 m12  1

		/// <summary>The matrix member at (0,0)</summary>
		public float m00;
		/// <summary>The matrix member at (1,0)</summary>
		public float m10;

		/// <summary>The matrix member at (0,1)</summary>
		public float m01;
		/// <summary>The matrix member at (1,1)</summary>
		public float m11;

		/// <summary>The matrix member at (0,2)</summary>
		public float m02;
		/// <summary>The matrix member at (1,2)</summary>
		public float m12;

		/// <summary>Initializes a Matrix2D with column vectors</summary>
		/// <param name="column0">The first column</param>
		/// <param name="column1">The second column</param>
		/// <param name="column2">The third column</param>
		public Matrix2D(Vector2 column0, Vector2 column1, Vector2 column2)
		{
			this.m00 = column0.x; this.m01 = column1.x; this.m02 = column2.x;
			this.m10 = column0.y; this.m11 = column1.y; this.m12 = column2.y;
		}

		/// <summary>Access element at [row, column].</summary>
		/// <returns>The value at [row, column]</returns>
		public float this[int row, int column]
		{
			get
			{
				return this[row + column * 2];
			}

			set
			{
				this[row + column * 2] = value;
			}
		}

		/// <summary>Access element at sequential index (0..5 inclusive).</summary>
		/// <returns>The value at [index]</returns>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return m00;
					case 1: return m10;
					case 2: return m01;
					case 3: return m11;
					case 4: return m02;
					case 5: return m12;
					default:
						throw new System.IndexOutOfRangeException("Invalid matrix index!");
				}
			}

			set
			{
				switch (index)
				{
					case 0: m00 = value; break;
					case 1: m10 = value; break;
					case 2: m01 = value; break;
					case 3: m11 = value; break;
					case 4: m02 = value; break;
					case 5: m12 = value; break;

					default:
						throw new System.IndexOutOfRangeException("Invalid matrix index!");
				}
			}
		}

		/// <summary>Gets a hashcode of the matrix.</summary>
		/// <remarks>Used to allow Matrix3x3s to be used as keys in hash tables.</remarks>
		/// <returns>The hashcode of the matrix</returns>
		public override int GetHashCode()
		{
			return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2);
		}

		/// <summary>Checks if two matrices are equal.</summary>
		/// <param name="other">The other matrix to compare with</param>
		/// <remarks>Used to allow Matrix3x3s to be used as keys in hash tables.</remarks>
		/// <returns>True when the matrix is equal to "other"</returns>
		public override bool Equals(object other)
		{
			if (!(other is Matrix2D)) return false;

			Matrix2D rhs = (Matrix2D)other;
			return GetColumn(0).Equals(rhs.GetColumn(0))
				&& GetColumn(1).Equals(rhs.GetColumn(1))
				&& GetColumn(2).Equals(rhs.GetColumn(2));
		}

		/// <summary>Multiplies two matrices.</summary>
		/// <param name="lhs">The left hand side matrix of the operation</param>
		/// <param name="rhs">The right hand side matrix of the operation</param>
		/// <returns>The multiplied matrix</returns>
		public static Matrix2D operator*(Matrix2D lhs, Matrix2D rhs)
		{
			Matrix2D res;
			res.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10;
			res.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11;
			res.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02;

			res.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10;
			res.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11;
			res.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12;

			return res;
		}

		/// <summary>Transforms a Vector2 by a matrix.</summary>
		/// <param name="lhs">The left hand side matrix of the operation</param>
		/// <param name="vector">The vector the matrix will be multiplied with</param>
		/// <returns>The transformed vector</returns>
		public static Vector2 operator*(Matrix2D lhs, Vector2 vector)
		{
			Vector2 res;
			res.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02;
			res.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12;
			return res;
		}

		/// <summary>Checks if two matrices are equal.</summary>
		/// <param name="lhs">The left hand side matrix of the comparison</param>
		/// <param name="rhs">The right hand side matrix of the comparison</param>
		/// <returns>True if "lhs" and "rhs" are equal, or false otherwise.</returns>
		public static bool operator==(Matrix2D lhs, Matrix2D rhs)
		{
			// Returns false in the presence of NaN values.
			return lhs.GetColumn(0) == rhs.GetColumn(0)
				&& lhs.GetColumn(1) == rhs.GetColumn(1)
				&& lhs.GetColumn(2) == rhs.GetColumn(2);
		}

		/// <summary>Checks if two matrices are not equal.</summary>
		/// <param name="lhs">The left hand side matrix of the comparison</param>
		/// <param name="rhs">The right hand side matrix of the comparison</param>
		/// <returns>True if "lhs" and "rhs" not are equal, or false otherwise.</returns>
		public static bool operator!=(Matrix2D lhs, Matrix2D rhs)
		{
			// Returns true in the presence of NaN values.
			return !(lhs == rhs);
		}

		/// <summary>Gets a column of the matrix.</summary>
		/// <param name="index">The column index, between 0 and 2 inclusively</param>
		/// <returns>The column at "index"</returns>
		public Vector2 GetColumn(int index)
		{
			switch (index)
			{
				case 0: return new Vector2(m00, m10);
				case 1: return new Vector2(m01, m11);
				case 2: return new Vector2(m02, m12);
				default:
					throw new System.IndexOutOfRangeException("Invalid column index!");
			}
		}

		/// <summary>Gets a row of the matrix.</summary>
		/// <param name="index">The row index, between 0 and 1 inclusively</param>
		/// <returns>The row at "index"</returns>
		public Vector3 GetRow(int index)
		{
			switch (index)
			{
				case 0: return new Vector3(m00, m01, m02);
				case 1: return new Vector3(m10, m11, m12);
				default:
					throw new System.IndexOutOfRangeException("Invalid row index!");
			}
		}

		/// <summary>Sets a column of the matrix.</summary>
		/// <param name="index">The column index, between 0 and 2 inclusively</param>
		/// <param name="column">The column</param>
		public void SetColumn(int index, Vector2 column)
		{
			this[0, index] = column.x;
			this[1, index] = column.y;
		}

		/// <summary>Sets a row of the matrix.</summary>
		/// <param name="index">The column index, between 0 and 1 inclusively</param>
		/// <param name="row">The row</param>
		public void SetRow(int index, Vector3 row)
		{
			this[index, 0] = row.x;
			this[index, 1] = row.y;
			this[index, 2] = row.z;
		}

		/// <summary>Transforms a position by this matrix (effectively by 2x3).</summary>
		/// <param name="point">The point to multiply with this matrix</param>
		/// <returns>The multiplied point</returns>
		public Vector2 MultiplyPoint(Vector2 point)
		{
			Vector2 res;
			res.x = this.m00 * point.x + this.m01 * point.y + this.m02;
			res.y = this.m10 * point.x + this.m11 * point.y + this.m12;
			return res;
		}

		/// <summary>Transforms a direction by this matrix.</summary>
		/// <param name="vector">The direction to multiply with this matrix</param>
		/// <returns>The multiplied direction</returns>
		public Vector2 MultiplyVector(Vector2 vector)
		{
			Vector2 res;
			res.x = this.m00 * vector.x + this.m01 * vector.y;
			res.y = this.m10 * vector.x + this.m11 * vector.y;
			return res;
		}

		/// <summary>Computes the inverse of the matrix.</summary>
		/// <returns>The inverse matrix</returns>
		public Matrix2D Inverse()
		{
			Matrix2D invMat = new Matrix2D();

			float det = this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
			if (Mathf.Approximately(0.0f, det))
				return zero;

			float invDet = 1.0F / det;

			invMat[0, 0] = this[1, 1] * invDet;
			invMat[0, 1] = -this[0, 1] * invDet;
			invMat[1, 0] = -this[1, 0] * invDet;
			invMat[1, 1] = this[0, 0] * invDet;

			// Do the translation part
			invMat[0, 2] = -(this[0, 2] * invMat[0, 0] + this[1, 2] * invMat[0, 1]);
			invMat[1, 2] = -(this[0, 2] * invMat[1, 0] + this[1, 2] * invMat[1, 1]);

			return invMat;
		}

		/// <summary>Creates a scaling matrix.</summary>
		/// <param name="vector">The scaling vector</param>
		/// <returns>The scaling matrix</returns>
		public static Matrix2D Scale(Vector2 vector)
		{
			Matrix2D m;
			m.m00 = vector.x; m.m01 = 0F; m.m02 = 0F;
			m.m10 = 0F; m.m11 = vector.y; m.m12 = 0F;
			return m;
		}

		/// <summary>Creates a translation matrix.</summary>
		/// <param name="vector">The translation vector</param>
		/// <returns>The translation matrix</returns>
		public static Matrix2D Translate(Vector2 vector)
		{
			Matrix2D m;
			m.m00 = 1F; m.m01 = 0F; m.m02 = vector.x;
			m.m10 = 0F; m.m11 = 1F; m.m12 = vector.y;
			return m;
		}

		/// <summary>Creates a right-hand side rotation matrix.</summary>
		/// <param name="angleRadians">The rotation angle, in radians</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix2D RotateRH(float angleRadians)
		{
			return RotateLH(-angleRadians);
		}

		/// <summary>Creates a left-hand side rotation matrix.</summary>
		/// <param name="angleRadians">The rotation angle, in radians</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix2D RotateLH(float angleRadians)
		{
			// No SinCos? I hope the compiler optimizes this
			float s = Mathf.Sin(angleRadians);
			float c = Mathf.Cos(angleRadians);

			Matrix2D m;
			m.m00 = c; m.m10 = -s;
			m.m01 = s; m.m11 = c;
			m.m02 = 0.0F; m.m12 = 0.0F;
			return m;
		}

		/// <summary>Creates a skew matrix on X.</summary>
		/// <param name="angleRadians">The skew angle, in radians</param>
		/// <returns>The skew matrix</returns>		
		public static Matrix2D SkewX(float angleRadians)
		{
			Matrix2D m;
			m.m00 = 1.0f; m.m01 = Mathf.Tan(angleRadians); m.m02 = 0F;
			m.m10 = 0F; m.m11 = 1.0f; m.m12 = 0F;
			return m;
		}

		/// <summary>Creates a skew matrix on U.</summary>
		/// <param name="angleRadians">The skew angle, in radians</param>
		/// <returns>The skew matrix</returns>		
		public static Matrix2D SkewY(float angleRadians)
		{
			Matrix2D m;
			m.m00 = 1.0f; m.m01 = 0F; m.m02 = 0F;
			m.m10 = Mathf.Tan(angleRadians); m.m11 = 1.0f; m.m12 = 0F;
			return m;
		}

		static readonly Matrix2D zeroMatrix = new Matrix2D(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));

		/// <summary>Returns a matrix with all elements set to zero (read-only).</summary>
		/// <returns>The zero matrix</returns>
		public static Matrix2D zero { get { return zeroMatrix; } }

		static readonly Matrix2D identityMatrix = new Matrix2D(new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0));

		/// <summary>Returns the identity matrix (read-only).</summary>
		/// <returns>The identity matrix</returns>
		public static Matrix2D identity { get { return identityMatrix; } }

		/// <summary>Returns a string representation of the matrix.</summary>
		/// <returns>The matrix string representation</returns>
		public override string ToString()
		{
			return string.Format("{0:F5}\t{1:F5}\t{2:F5}\n{3:F5}\t{4:F5}\t{5:F5}\n", m00, m01, m02, m10, m11, m12);
		}

		/// <summary>Returns a string representation of the matrix using a format.</summary>
		/// <param name="format">The format to be used for the matrix components</param>
		/// <returns>The matrix string representation</returns>
		public string ToString(string format)
		{
			return string.Format("{0}\t{1}\t{2}\n{3}\t{4}\t{5}\n",
				m00.ToString(format), m01.ToString(format), m02.ToString(format),
				m10.ToString(format), m11.ToString(format), m12.ToString(format));
		}
	}
// Macro.Patch -->
	public static partial class UnityExtension {
		#region Methods

		public static Vector2 Matrix22_Mul(Vector4 m22,Vector2 v2) {
			return new Vector2(
				m22.x*v2.x+m22.y*v2.y,
				m22.z*v2.x+m22.w*v2.y
			);
		}

		public static Vector3 Division(Vector3 lhs,Vector3 rhs) {
			return new Vector3(
				lhs.x/rhs.x,
				lhs.y/rhs.y,
				lhs.z/rhs.z
			);
		}

		public static Color ToColor(this int v){
			return new Color(
				((v>>16)&0xFF)/255.0f,
				((v>> 8)&0xFF)/255.0f,
				((v>> 0)&0xFF)/255.0f,
				((v>>24)&0xFF)/255.0f
			);
		}

		public static Bounds NewBounds()=>new Bounds(
			new Vector3(float.NaN,float.NaN,float.NaN),
			new Vector3(float.NaN,float.NaN,float.NaN)
		);

		public static void TryEncapsulate(this ref Bounds thiz,Vector3 point) {
			if(float.IsNaN(thiz.center.x)) {thiz.center=point;thiz.extents=Vector3.zero;}
			else {thiz.Encapsulate(point);}
		}

		public static void TryEncapsulate(this ref Bounds thiz,Bounds bounds) {
			if(float.IsNaN(thiz.center.x)) {thiz=bounds;}
			else {thiz.Encapsulate(bounds);}
		}

		public static void GetPoints(this Bounds thiz,Vector3[] points,Matrix4x4 matrix) {
			int i=0;Vector3 c=thiz.center,e=thiz.extents;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3(-e.x,-e.y,-e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3(-e.x,-e.y, e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3(-e.x, e.y,-e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3(-e.x, e.y, e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3( e.x,-e.y,-e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3( e.x,-e.y, e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3( e.x, e.y,-e.z));++i;
			points[i]=matrix.MultiplyPoint3x4(c+new Vector3( e.x, e.y, e.z));++i;
		}

		public static void Clockwise(this Vector3[] thiz) {
			if(thiz!=null) {
				Vector3 o=Vector3.zero,x=Vector3.zero,y=Vector3.zero;
				int i,imax=thiz?.Length??0;
				if(imax>=3) {
					for(i=0;i<imax;++i) {o+=thiz[i];}
					o/=imax;i=0;x=(thiz[i]-o).normalized;
					for(i=1;i<imax;++i) {
						y=(thiz[i]-o).normalized;
						if(Vector3.Dot(x,y)<=0.1f) {break;}
					}
					Vector3.OrthoNormalize(ref x,ref y);
					System.Func<Vector3,float> func=(v)=>{
						return Mathf.Repeat(Mathf.Atan2(
							Vector3.Dot(v,y),Vector3.Dot(v,x)
						)*Mathf.Rad2Deg,360.0f);
					};
					System.Array.Sort(thiz,(a,b)=>{
						return System.Math.Sign(func(a-o)-func(b-o));
					});
				}
			}
		}

		public static GameObject GetInstance(this GameObject thiz) {
			if(thiz!=null&&!thiz.scene.IsValid()) {
				string s=thiz.name;
				thiz=GameObject.Instantiate(thiz);
				thiz.name=s;
			}
			return thiz;
		}

		public static bool IsActiveAndEnabled(this Behaviour thiz) {
			if(thiz!=null) {
				return thiz.gameObject.activeSelf&&thiz.enabled;
			}
			return false;
		}

		public static bool IsActiveAndEnabled(this Renderer thiz) {
			if(thiz!=null) {
				return thiz.gameObject.activeInHierarchy&&thiz.enabled;
			}
			return false;
		}

		public static T AddMissingComponent<T>(this GameObject thiz) where T:Component {
			T comp=thiz.GetComponent<T>();
			if(comp==null) {
				comp=thiz.AddComponent<T>();
			}
			return comp;
		}

		public static Component AddMissingComponent(this GameObject thiz,System.Type type){
			if(type==null) {
				return null;
			}
			Component comp=thiz.GetComponent(type);
			if(comp==null) {
				comp=thiz.AddComponent(type);
			}
			return comp;
		}

		public static T AddMissingComponent<T>(this Component thiz) where T:Component {
			T comp=thiz.GetComponent<T>();
			if(comp==null) {
				comp=thiz.gameObject.AddComponent<T>();
			}
			return comp;
		}

		public static Component AddMissingComponent(this Component thiz,System.Type type){
			if(type==null) {
				return null;
			}
			Component comp=thiz.GetComponent(type);
			if(comp==null) {
				comp=thiz.gameObject.AddComponent(type);
			}
			return comp;
		}

		public static void CheckComponent<T>(this GameObject thiz,ref T comp,int searchDir=0) where T:Component {
			if(comp==null) {
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent<T>();
					break;
					case 1:
						comp=thiz.GetComponentInChildren<T>();
					break;
					case 0:
					default:
						comp=thiz.GetComponent<T>();
					break;
				}
			}
		}
		public static void CheckComponent(this GameObject thiz,ref Component comp,System.Type type,int searchDir=0) {
			if(comp==null) {
				if(type==null) {return;}
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent(type);
					break;
					case 1:
						comp=thiz.GetComponentInChildren(type);
					break;
					case 0:
					default:
						comp=thiz.GetComponent(type);
					break;
				}
			}
		}
		public static void CheckComponent<T>(this Component thiz,ref T comp,int searchDir=0) where T:Component {
			if(comp==null) {
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent<T>();
					break;
					case 1:
						comp=thiz.GetComponentInChildren<T>();
					break;
					case 0:
					default:
						comp=thiz.GetComponent<T>();
					break;
				}
			}
		}
		public static void CheckComponent(this Component thiz,ref Component comp,System.Type type,int searchDir=0) {
			if(comp==null) {
				if(type==null) {return;}
				switch(searchDir) {
					case -1:
						comp=thiz.GetComponentInParent(type);
					break;
					case 1:
						comp=thiz.GetComponentInChildren(type);
					break;
					case 0:
					default:
						comp=thiz.GetComponent(type);
					break;
				}
			}
		}

		public static string GetName(this Object thiz) {
			return thiz!=null?thiz.name:null;
		}

		public static bool IsPrefab(this Object thiz) {
			if(thiz is GameObject go) {return !go.scene.IsValid();}
			if(thiz is Component c) {return !c.gameObject.scene.IsValid();}
			return false;
		}

		public static void Destroy(this Object thiz) {
			if(thiz!=null) {
#if UNITY_EDITOR
				if(!UnityEditor.EditorApplication.isPlaying)
					Object.DestroyImmediate(thiz);
				else
#endif
				Object.Destroy(thiz);
			}
		}

		public static int GetDepth(this Transform thiz) {
			int i=-1;
			while(thiz!=null) {++i;thiz=thiz.parent;}
			return i;
		}

		public static Transform FindOrCreate(this Transform thiz,string path) {
			if(thiz!=null&&!string.IsNullOrEmpty(path)) {
				Transform tmp=thiz.Find(path);
				if(tmp==null) {
					tmp=new GameObject(path).transform;
					tmp.SetParent(thiz,false);
				}
				thiz=tmp;
			}
			return thiz;
		}

		public static Matrix4x4 GetLocalToWorldMatrix(this Renderer thiz,Transform root) {
			if(root!=null) {return root.localToWorldMatrix;}
			if(thiz!=null) {return thiz.localToWorldMatrix;}
			return Matrix4x4.identity;
		}

		public static Matrix4x4 GetLocalToWorldMatrix(this SkinnedMeshRenderer thiz)=>
			thiz!=null?thiz.GetLocalToWorldMatrix(thiz.rootBone):Matrix4x4.identity;

		public static void SetBounds(this SkinnedMeshRenderer thiz,IEnumerable<SkinnedMeshRenderer> others) {
			if(thiz!=null&&others!=null) {
				Bounds bounds=NewBounds();
				Matrix4x4 matrix=thiz.GetLocalToWorldMatrix(),m;
				Vector3[] points=new Vector3[8];
				foreach(var it in others) {
					if(it!=null) {
						m=it.GetLocalToWorldMatrix();
						if(matrix!=m) {
							Debug.LogWarning($"{thiz.name}.localToWorldMatrix!={it.name}.localToWorldMatrix");
							it.localBounds.GetPoints(points,matrix*m);
							for(int i=0,imax=points.Length;i<imax;++i) {
								bounds.TryEncapsulate(points[i]);
							}
						}else {
							bounds.TryEncapsulate(it.localBounds);
						}
					}
				}
				if(float.IsNaN(bounds.center.x)) {bounds=new Bounds();}
				thiz.localBounds=bounds;
			}
		}

		public static Transform RemapTransform(this Transform thiz,Transform src,Transform dst) {
			Transform tmp=thiz;
			using(UnityEngine.Pool.ListPool<string>.Get(out var list)) {
				while(tmp!=null&&tmp!=src) {
					list.Add(tmp.name);
					tmp=tmp.parent;
				}
				tmp=dst;
				for(int i=0,imax=list.Count;i<imax;++i) {
					if(tmp==null) {break;}
					tmp=tmp.Find(list[imax-1-i]);
				}
			}
			if(tmp==null) {Debug.LogWarning(thiz.name+" is missed.");}
			return tmp;
		}

		#endregion Methods

		#region Experimentals

		public static readonly string[] SPLIT_DIR=new string[2]{"/","\\"};

		/// <summary>
		/// 
		/// </summary>
		public static Transform FindEx(this Transform thiz,string path) {
			if(string.IsNullOrEmpty(path)) {
				return thiz;
			}
			if(thiz==null) {
				GameObject go=GameObject.Find(path);
				return go==null?null:go.transform;
			}
			//
			Transform t;
			int index;
			string[] cmds=path.Split(SPLIT_DIR,System.StringSplitOptions.RemoveEmptyEntries);
			for(int i=0,imax=cmds.Length;i<imax;++i) {
				t=null;
				if(cmds[i]=="..") {
					t=thiz.parent;
				}else if(cmds[i].StartsWith(".@")){
					if(!int.TryParse(cmds[i].Substring(2),out index)) {
						index=-1;
					}
					if(index>=0&&index<thiz.childCount) {
						t=thiz.GetChild(index);
					}
				}else {
					t=thiz.Find(cmds[i]);
				}
				//
				if(t!=null) {
					thiz=t;
				}else {
					break;
				}
			}
			return thiz;
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetPath(this Transform thiz,Transform root = null) {
			if(thiz==null) {
				return "";
			}
			// It may cause many GC calls in this implement.
			// TODO : Here is no method named "Reverse()" in System.Text.StringBuilder.
			string path = null;
			while(thiz!=root&&thiz!=null) {
				if(string.IsNullOrEmpty(path)) {
					path=thiz.name;
				} else {
					path=thiz.name+"/"+path;
				}
				thiz=thiz.parent;
			}
			return path;
		}

		public static void SendMessageEx(this GameObject thiz,string message) {
			if(thiz!=null&&!string.IsNullOrEmpty(message)) {
				int i=message.LastIndexOf(',');
				if(i>=0) {
					System.Type type=System.Type.GetType(message.Substring(0,i));
					if(type==null) {return;}
					var method=type.GetMethod(message.Substring(i+1));
					var tmp=thiz.GetComponent(type);
					if(method==null||tmp==null) {return;}
					method.Invoke(tmp,System.Type.EmptyTypes);
					return;
				}
				thiz.SendMessage(message,SendMessageOptions.DontRequireReceiver);
			}
		}

		#endregion Experimentals

	}
}
