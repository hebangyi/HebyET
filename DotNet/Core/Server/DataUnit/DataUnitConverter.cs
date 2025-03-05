namespace ET.Server;

public interface IDataUnitConverter
{
    Type GetDataType();
    Type GetUnitDataType();
    IUnitData ToConvert(object data);
}


[DataUnitConverter]
public abstract class DataUnitConverter<Data, UnitData> : IDataUnitConverter where UnitData: MessageObject, IUnitData 
{
    public abstract UnitData Convert(Data data);

    public Type GetDataType()
    {
        return typeof (Data);
    }
    
    public Type GetUnitDataType()
    {
        return typeof (UnitData);
    }

    public IUnitData ToConvert(object obj)
    {
        if (obj is not Data data)
        {
            Log.Error($"Convert 类型转换错误: {obj.GetType().FullName} to {typeof (Data).FullName}");
            return null;
        }
        
        return Convert(data);
    }
}