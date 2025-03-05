using System;
using System.Collections.Generic;
using ET.Server;

namespace ET;

[Code]
public class DataUnitManager: Singleton<DataUnitManager>, ISingletonAwake
{
    public Dictionary<Type, IDataUnitConverter> DataType2DataUnitConverters = new ();
    
    public void Awake()
    {
        var converters = CodeTypes.Instance.GetAttributeTypes(typeof(DataUnitConverterAttribute));
        foreach (var convertType in converters)
        {
            var convert = Activator.CreateInstance(convertType) as IDataUnitConverter;
            if (convert == null)
            {
                Log.Error($"Type : {convertType} 没有继承 IDataUnitConverter");
                continue;
            }

            var dataType = convert.GetDataType();
            DataType2DataUnitConverters[dataType] = convert;
        }
    }

    public IUnitData ToUnitData(object data)
    {
        var dataType = data.GetType();
        var converter = this.DataType2DataUnitConverters.GetValueOrDefault(dataType);
        if (converter == null)
        {
            Log.Error($"Type : {dataType.FullName} 没有找到 Converter 转换器");
            return null;
        }

        return converter.ToConvert(data);
    }
}