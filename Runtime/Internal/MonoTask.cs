using UnityEngine;

namespace YouSingStudio.MeshKit {
	public abstract class MonoTask 
		:MonoBehaviour
	{
		#region Nested Types

		public enum RunType {
			Manual=-1,
			Awake,
			Start,
			OnDestroy,
			OnEnable,
			OnDisable,
			Count
		}

		#endregion Nested Types

		#region Fields

		public RunType runType=RunType.Start;

		#endregion Fields

		#region Unity Messages

		protected virtual void Awake() {
			if(runType==RunType.Awake) {
				Run();
			}
		}

		protected virtual void Start() {
			if(runType==RunType.Start) {
				Run();
			}
		}

		protected virtual void OnDestroy() {
			if(runType==RunType.OnDestroy) {
				Run();
			}
		}

		protected virtual void OnEnable() {
			if(runType==RunType.OnEnable) {
				Run();
			}
		}

		protected virtual void OnDisable() {
			if(runType==RunType.OnDisable) {
				Run();
			}
		}

		#endregion Unity Messages

		#region Methods

		[ContextMenu("Run")]
		public virtual void RunIfActive() {
			if(isActiveAndEnabled) {
				Run();
			}
		}

		public abstract void Run();

		#endregion Methods
	}
}
