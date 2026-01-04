using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YouSingStudio.MeshKit {
	public class TaskExecutor
		:MonoTask
	{
		#region Fields

		public Mesh mesh;
		public List<MonoTask> tasks;
		public UnityEvent<Mesh> preExecute=null;
		public UnityEvent<Mesh> postExecute=null;

		#endregion Fields

		#region Methods

		// https://www.runoob.com/sql/sql-orderby.html

		[ContextMenu("Sort Asc")]
		public virtual void SortAsc() {
			tasks.Sort((x,y)=>string.Compare(x.GetName(),y.GetName()));
		}

		[ContextMenu("Sort Desc")]
		public virtual void SortDesc() {
			tasks.Sort((x,y)=>-string.Compare(x.GetName(),y.GetName()));
		}

		public override void Run() {
			preExecute?.Invoke(mesh);
			//
			MonoTask it;for(int i=0,imax=tasks?.Count??0;i<imax;++i) {
				it=tasks[i];if(it!=null) {
					if(mesh!=null) {it.Run(mesh);}
					else {it.Run();}
				}
			}
			//
			postExecute?.Invoke(mesh);
		}

		#endregion Methods
	}
}
