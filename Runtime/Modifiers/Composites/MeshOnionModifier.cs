using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

namespace YouSingStudio.MeshKit {
	/// <summary>
	/// A onion workflow,which split mesh into layers.
	/// </summary>
	public class MeshOnionModifier
		:MonoTask
	{
		#region Nested Types

		[System.Serializable]
		public class Layer {
			public string name;
			public MonoTask task;
			public MeshSelectorBase select;
			[TextArea]public string preset;
		}

		#endregion Nested Types

		#region Fields

		public Mesh mesh;
		public Transform container;
		public MeshCombiner combiner;
		public MonoTask task;
		public string[] selects=new string[]{"selector","selection"};
		public Layer[] layers;

		#endregion Fields

		#region Methods

		public override void Run() {
			Mesh m=mesh;
			if(m==null) {m=s_Mesh;}
			if(m==null) {return;}
			if(container==null) {container=transform;}
			//
			for(int i=0,imax=layers?.Length??0;i<imax;++i) {
				Create(m,layers[i]);
			}
			if(combiner!=null) {
				container.GetComponentsInChildren(true,combiner.meshFilters);
				container.GetComponentsInChildren(true,combiner.skinnedMeshRenderers);
				combiner.Run();mesh=combiner.GetMesh();
			}
			if(task!=null) {
				task.Run(mesh);
			}
		}

		public virtual void Select(MonoTask task,MeshSelectorBase select) {
			if(task==null||select==null) {return;}
			//
			using(ListPool<MonoBehaviour>.Get(out var list)) {
				task.GetComponentsInChildren(true,list);
				System.Type type;MonoBehaviour it;FieldInfo fi;
				int i,imax=list.Count,j,jmax=selects?.Length??0;
				for(i=0;i<imax;++i) {
					it=list[i];type=it.GetType();
					for(j=0;j<jmax;++j) {
						fi=type.GetField(selects[j]);
						if(fi!=null) {
							if(fi.GetValue(it)==null) {fi.SetValue(it,select);break;}
						}
					}
				}
			}
		}

		public virtual void Preset(MonoTask task,string preset) {
			if(task==null||string.IsNullOrEmpty(preset)) {return;}
			//
			Object obj;Transform t=task.transform;
			foreach(var it in JsonConvert.DeserializeObject<Dictionary<string,string>>(preset)) {
				obj=t.FindSmart(it.Key);
				if(obj!=null) {JsonConvert.PopulateObject(it.Value,obj);}

			}
		}

		public virtual void Create(Mesh mesh,Layer layer) {
			MonoTask task=layer.task;
			if(task==null) {return;}
			string key=!string.IsNullOrEmpty(layer.name)?layer.name:
				(layer.select!=null?layer.select.name:task.name);
			if(task.IsPrefab()) {
				task=MonoTask.Instantiate(task);
				task.transform.SetParent(container,false);
			}
			task.name=key;
			Select(task,layer.select);
			Preset(task,layer.preset);
			//
			task.Run(Mesh.Instantiate(mesh));
		}

		#endregion Methods
	}
}
