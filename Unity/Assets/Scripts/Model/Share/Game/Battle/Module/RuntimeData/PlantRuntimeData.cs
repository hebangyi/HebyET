namespace ET
{
    public class PlantRuntimeData : IUnitEntityLogicElemData
    {
        public bool[,] TitleMaps;

        // 最大的 MaxX TileCount
        public int MaxXTileCount;

        // 最大的 MaxY TileCount
        public int MaxYTileCount;
        public int TileMapUnitSize;
    }
}