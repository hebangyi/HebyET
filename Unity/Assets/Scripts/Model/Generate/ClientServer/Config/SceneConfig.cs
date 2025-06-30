using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class SceneConfigCategory : Singleton<SceneConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, SceneConfig> dict = new();
		
        public void Merge(object o)
        {
            SceneConfigCategory s = o as SceneConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public SceneConfig GetById(int id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SceneConfig> GetAll()
        {
            return this.dict;
        }

        public SceneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }

	public partial class SceneConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>章节名称</summary>
		public string Name { get; set; }
		/// <summary>地图类型</summary>
		public int MapSceneType { get; set; }
		/// <summary>资源路径</summary>
		public string AssetPath { get; set; }

	}
}
