using UnityEngine;

namespace YouSingStudio.MeshKit {
	public class BatchedMeshModifier
		:MonoTask
	{
		#region Fields

		public MeshModifierBase meshModifier;
		public Transform[] transforms=new Transform[0];

		#endregion Fields

		#region Methods

		public override void Run() {
			if(meshModifier==null) {
				meshModifier=GetComponent<MeshModifierBase>();
				if(meshModifier==null) {
					return;
				}
			}
			//
			Transform t;
			for(int i=0,imax=transforms?.Length??0;i<imax;++i) {
				t=transforms[i];
				if(t!=null) {
					meshModifier.mesh=null;
					meshModifier.target=t;
					meshModifier.Run();
				}
			}
		}

		#endregion Methods
	}
}