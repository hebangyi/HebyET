using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class BattleMapConfigCategory : BaseCategory<BattleMapConfigCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, BattleMapConfig> dict = new();
		
        public override void Merge(object o)
        {
            BattleMapConfigCategory s = o as BattleMapConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public BattleMapConfig GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, BattleMapConfig> GetAll()
        {
            return this.dict;
        }

        public BattleMapConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
        
        public override int Count()
        {
            return this.dict.Count;
        }
    }

	public partial class BattleMapConfig: ProtoObject, IConfig
	{
		/// <summary>
		/// Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 地图边长
		/// </summary>
		public int AreaSize { get; set; }
		/// <summary>
		/// Cell中心点数量
		/// </summary>
		public int CellPointCount { get; set; }
		/// <summary>
		/// Cell中心点最近距离
		/// </summary>
		public int CellPointMinDistance { get; set; }
		/// <summary>
		/// 生成Cell数量
		/// </summary>
		public int GenCellCount { get; set; }

	}
}
