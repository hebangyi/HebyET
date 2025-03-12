using System;
using System.Collections.Generic;

namespace ET
{
    
    [Code]
    public class DataUnitManager: Singleton<DataUnitManager>, ISingletonAwake
    {
        public Dictionary<Type, IUnitDataConverter> ServerDataType2Converters = new ();
        public Dictionary<Type, IUnitDataConverter> ClientDataType2Converters = new ();
        public Dictionary<Type, IUnitDataConverter> UnitDataType2Converters = new ();
    
        public void Awake()
        {
            var converters = CodeTypes.Instance.GetAttributeTypes(typeof(DataUnitConverterAttribute));
            foreach (var convertType in converters)
            {
                var convert = Activator.CreateInstance(convertType) as IUnitDataConverter;
                if (convert == null)
                {
                    Log.Error($"Type : {convertType} 没有继承 IDataUnitConverter");
                    continue;
                }

                var serverDataType = convert.GetServerDataType();
                var clientDataType = convert.GetClientDataType();
                var unitDataType = convert.GetUnitDataType();


                this.ServerDataType2Converters[serverDataType] = convert;
                this.ClientDataType2Converters[clientDataType] = convert;
                this.UnitDataType2Converters[unitDataType] = convert;
            }
        }
    }
}
