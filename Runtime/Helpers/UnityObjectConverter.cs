using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Newtonsoft.Json {
	public class UnityObjectConverter:JsonConverter {
		public static Transform transform;
		public static bool IsList(System.Type type) {
			return type.IsArray||typeof(IList).IsAssignableFrom(type);
		}

		public static Object GetObject(System.Type type,string path) {
			if(transform!=null&&type.IsSubclassOf(typeof(Component))) {
				Transform t=transform.Find(path);
				if(t==null) {t=transform;}
				return t.GetComponent(type);
			}else {
#if UNITY_EDITOR
				return UnityEditor.AssetDatabase.LoadAssetAtPath(path,type);
#endif
				return Resources.Load(path,type);
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			if(IsList(objectType)) {objectType=objectType.GetElementType();}
			return objectType.IsSubclassOf(typeof(Object));
		}

		public override object ReadJson(JsonReader reader,System.Type objectType,object existingValue,JsonSerializer serializer) {
			object obj=null;
			if(IsList(objectType)) {
				string[] paths=serializer.Deserialize<string[]>(reader);
				int i=0,imax=paths?.Length??0;
				if(objectType.IsArray) {// Array
					objectType=objectType.GetElementType();
					System.Array array=(System.Array)existingValue;
					if((array?.Length??0)!=imax) {
						array=System.Array.CreateInstance(objectType,imax);
					}
					for(;i<imax;++i) {array.SetValue(GetObject(objectType,paths[i]),i);}
					obj=array;
				}else {// IList
					objectType=objectType.GetGenericArguments()[0];
					IList list=(IList)existingValue;
					if(list!=null) {list.Clear();}else {
						list=(IList)System.Activator.CreateInstance(typeof(List<>).MakeGenericType(objectType));
					}
					for(;i<imax;++i) {list.Add(GetObject(objectType,paths[i]));}
					obj=list;
				}
			}else {
				obj=GetObject(objectType,serializer.Deserialize<string>(reader));
			}
			return obj??existingValue;
		}

		public override bool CanWrite=>false;
		public override void WriteJson(JsonWriter writer,object value,JsonSerializer serializer) {}
	}
}