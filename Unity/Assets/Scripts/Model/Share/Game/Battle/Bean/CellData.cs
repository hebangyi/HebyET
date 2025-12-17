using System.Collections.Generic;
using Unity.Mathematics;
using Random = System.Random;

namespace ET
{
    // 地块生成上下文
    public class PlantGenContext
    {
        // 初始化参数
        public PlantGenInitData InitData = new PlantGenInitData();
        // 生成地块数据
        public PlantData PlantData = new PlantData();
    }

    // 初始化参数
    public class PlantGenInitData
    {
        // 地块宽度
        public int AreaSize;
        // 生成的 AllCell 点数
        public int CellPointCount;
        // 生成最小间隔的Point
        public int CellPointMinDistance;
        // 生成Cell的数量
        public int GenCellCount;
        
        // 随机器
        public Random Random = new Random();
    }


    public class PlantData
    {
        // 所有地块
        public List<CellData> AllCells = new ();
        // 生成地块
        public List<CellData> RealCells = new ();
    }


    public class CellData
    {
        public int Id;
        public float2 Center;
        public HashSet<float4> Borders = new ();
        public HashSet<CellData> NearCellDataSet = new ();
    }

    public class GenCellData
    {
        public int Id;
        public bool IsInMap;
        public HashSet<int> NearCellIds = new();
        public CellData CellData;
    }
}