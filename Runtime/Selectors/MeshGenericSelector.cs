/* <!-- Macro.Define bFixPatch=
true
 Macro.End -->*/
/* <!-- Macro.Define DeclareMethod
		[SerializeField]protected string m_{1}Key="{1}";
		{4}[SerializeField]protected {0} m_{1}Val;
		[System.NonSerialized]protected System.{3}> on{1}=null;

		public override {0} {1}({2} x) {{
			if(!m_IsInited) {{Init();}}return//{0}
			on{1}?.Invoke(x){5};
		}}

 Macro.End -->*/

/* <!-- Macro.Table Methods
void,BeginSelect,Mesh,Action<Mesh,//,,
void,EndSelect,Mesh,Action<Mesh,//,,
bool,TestIndex,int,Func<int&#44;bool,,??m_TestIndexVal,
bool,TestVertex,Vector3,Func<Vector3&#44;bool,,??m_TestVertexVal,
 Macro.End -->*/

/* <!-- Macro.Copy
		[SerializeField]protected Object m_Target;
 Macro.End -->*/
/* <!-- Macro.Call DeclareMethod Methods
 Macro.End -->*/

/* <!-- Macro.Replace
return//void,
 Macro.End -->*/
/* <!-- Macro.Patch
,AutoGen
 Macro.End -->*/
/* <!-- Macro.Call  Methods
				mi=type.GetMethod("{1}");if(mi!=null) {{
					on{1}=System.Delegate.CreateDelegate(typeof(System.{3}>),m_Target,mi,false) as System.{3}>;
				}}else{{on{1}=null;}}
 Macro.End -->*/
/* <!-- Macro.Patch
,Init
 Macro.End -->*/
using System.Reflection;
using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class MeshGenericSelector
		:MeshSelectorBase
	{
// <!-- Macro.Patch AutoGen
		[SerializeField]protected Object m_Target;
		[SerializeField]protected string m_BeginSelectKey="BeginSelect";
		//[SerializeField]protected void m_BeginSelectVal;
		[System.NonSerialized]protected System.Action<Mesh> onBeginSelect=null;

		public override void BeginSelect(Mesh x) {
			if(!m_IsInited) {Init();}
			onBeginSelect?.Invoke(x);
		}

		[SerializeField]protected string m_EndSelectKey="EndSelect";
		//[SerializeField]protected void m_EndSelectVal;
		[System.NonSerialized]protected System.Action<Mesh> onEndSelect=null;

		public override void EndSelect(Mesh x) {
			if(!m_IsInited) {Init();}
			onEndSelect?.Invoke(x);
		}

		[SerializeField]protected string m_TestIndexKey="TestIndex";
		[SerializeField]protected bool m_TestIndexVal;
		[System.NonSerialized]protected System.Func<int,bool> onTestIndex=null;

		public override bool TestIndex(int x) {
			if(!m_IsInited) {Init();}return//bool
			onTestIndex?.Invoke(x)??m_TestIndexVal;
		}

		[SerializeField]protected string m_TestVertexKey="TestVertex";
		[SerializeField]protected bool m_TestVertexVal;
		[System.NonSerialized]protected System.Func<Vector3,bool> onTestVertex=null;

		public override bool TestVertex(Vector3 x) {
			if(!m_IsInited) {Init();}return//bool
			onTestVertex?.Invoke(x)??m_TestVertexVal;
		}

// Macro.Patch -->
		#region Fields

		[System.NonSerialized]protected bool m_IsInited;

		#endregion Fields

		#region Methods

		protected virtual void Init() {
			if(m_IsInited) {return;}
			m_IsInited=true;
			//
			if(m_Target!=null) {
				System.Type type=m_Target.GetType();MethodInfo mi;
// <!-- Macro.Patch Init
				mi=type.GetMethod("BeginSelect");if(mi!=null) {
					onBeginSelect=System.Delegate.CreateDelegate(typeof(System.Action<Mesh>),m_Target,mi,false) as System.Action<Mesh>;
				}else{onBeginSelect=null;}
				mi=type.GetMethod("EndSelect");if(mi!=null) {
					onEndSelect=System.Delegate.CreateDelegate(typeof(System.Action<Mesh>),m_Target,mi,false) as System.Action<Mesh>;
				}else{onEndSelect=null;}
				mi=type.GetMethod("TestIndex");if(mi!=null) {
					onTestIndex=System.Delegate.CreateDelegate(typeof(System.Func<int,bool>),m_Target,mi,false) as System.Func<int,bool>;
				}else{onTestIndex=null;}
				mi=type.GetMethod("TestVertex");if(mi!=null) {
					onTestVertex=System.Delegate.CreateDelegate(typeof(System.Func<Vector3,bool>),m_Target,mi,false) as System.Func<Vector3,bool>;
				}else{onTestVertex=null;}
// Macro.Patch -->
			}
		}

		#endregion Methods
	}
}
