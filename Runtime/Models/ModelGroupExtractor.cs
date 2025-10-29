using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	public class ModelGroupExtractor
		:ModelElementExtractor
	{
		#region Fields

		[Header("Group")]
		public bool inverse;
		public Transform container;
		public List<int> group;

		#endregion Fields

		#region Methods

		protected virtual bool Filter(int index) {
			if(submeshes!=-1) {
				int i=selector.GetSubMesh(index);
				if(i>=0&&(submeshes&(1<<i))==0) {return false;}
			}
			if(container!=null) {
				Transform t=container.Find(string.Format(format,index));
				if(t!=null) {return !inverse;}
			}
			if(group.IndexOf(index)>=0) {
				return !inverse;
			}
			return inverse;
		}

		protected override void InternalRun() {
			using(ListPool<int>.Get(out var list)) {
				for(int i=0,imax=selector.shapes?.Count??0;i<imax;++i) {
					if(Filter(i)) {list.Add(selector.shapes[i].First());}
				}
				selector.indexes=list.ToArray();
			}
			InternalRun(0);
		}

		#endregion Methods
	}
}
