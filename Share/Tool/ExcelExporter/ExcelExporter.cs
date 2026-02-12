using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace ET
{
    public enum ConfigType
    {
        c = 0,
        s = 1,
        cs = 2,
    }

    class HeadInfo
    {
        [BsonElement]
        public string FieldCS;

        public string FieldDesc;
        public string FieldName;
        public string FieldType;
        public string FieldValue;
        public int FieldIndex;

        public HeadInfo(string cs, string desc, string name, string type, int index, string fieldValue)
        {
            this.FieldCS = cs;
            this.FieldDesc = desc;
            this.FieldName = name;
            this.FieldType = type;
            this.FieldIndex = index;
            this.FieldValue = fieldValue;
        }
    }

    // 这里加个标签是为了防止编译时裁剪掉protobuf，因为整个tool工程没有用到protobuf，编译会去掉引用，然后动态编译就会出错
    class Table
    {
        public int Index;
        public string ClassName;
        public ConfigType ConfigType { get; set; }
        public TableType TableType { get; set; }
        public Dictionary<string, HeadInfo> HeadInfos = new Dictionary<string, HeadInfo>();
        public List<ExcelPackage> ExcelPackages = new List<ExcelPackage>();
    }

    public enum TableType
    {
        IdTable,
        KeyTable,
    }

    public static class ExcelExporter
    {
        private static string template;
        private static string templateKey;

        private const string ClientClassDir = "../Unity/Assets/Scripts/Model/Client/Generate/Config";

        // 服务端因为机器人的存在必须包含客户端所有配置，所以单独的c字段没有意义,单独的c就表示cs
        private const string ServerClassDir = "../DotNet/Model/Server/Generate/Config";

        private const string CSClassDir = "../Unity/Assets/Scripts/Model/Share/Generate/Config";

        private const string excelDir = "../Config/Excel/";

        private const string jsonDir = "../ConfigExport/Json/{0}/";
        private const string serverProtoDir = "../ConfigExport/Excel/{0}/";
        
        
        
        private const string clientProtoDir = "../Unity/Assets/Bundles/Config";
        private const string replaceStr = "/{0}/{1}";
        private static Assembly[] configAssemblies = new Assembly[3];

        private static Dictionary<string, Table> tables = new Dictionary<string, Table>();
        private static Dictionary<string, ExcelPackage> packages = new Dictionary<string, ExcelPackage>();

        private static Table GetTable(string className)
        {
            if (!tables.TryGetValue(className, out var table))
            {
                table = new Table();
                table.ClassName = className;
                tables[className] = table;
            }

            return table;
        }

        public static ExcelPackage GetPackage(string filePath)
        {
            if (!packages.TryGetValue(filePath, out var package))
            {
                using Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                package = new ExcelPackage(stream);
                packages[filePath] = package;
            }

            return package;
        }

        public static void Export()
        {
            try
            {
                template = File.ReadAllText("Template.txt");
                templateKey = File.ReadAllText("Templatekey.txt");
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // 1.创建文件夹
                if (Directory.Exists(ClientClassDir))
                {
                    Directory.Delete(ClientClassDir, true);
                    Directory.CreateDirectory(ClientClassDir);
                }

                if (Directory.Exists(ServerClassDir))
                {
                    Directory.Delete(ServerClassDir, true);
                    Directory.CreateDirectory(ServerClassDir);
                }

                if (Directory.Exists(CSClassDir))
                {
                    Directory.Delete(CSClassDir, true);
                    Directory.CreateDirectory(CSClassDir);
                }

                string jsonProtoDirParent = jsonDir.Replace(replaceStr, string.Empty);
                if (Directory.Exists(jsonProtoDirParent))
                {
                    Directory.Delete(jsonProtoDirParent, true);
                }

                string serverProtoDirParent = serverProtoDir.Replace(replaceStr, string.Empty);
                if (Directory.Exists(serverProtoDirParent))
                {
                    Directory.Delete(serverProtoDirParent, true);
                }

                // 2.加载Excel Package
                List<string> files = FileHelper.GetAllFiles(excelDir);
                foreach (string path in files)
                {
                    string fileName = Path.GetFileName(path);
                    if (!fileName.EndsWith(".xlsx") || fileName.StartsWith("~$") || fileName.Contains("#"))
                    {
                        continue;
                    }

                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string className = fileNameWithoutExtension;
                    string cs = "cs";
                    if (fileNameWithoutExtension.Contains("@"))
                    {
                        string[] ss = fileNameWithoutExtension.Split("@");
                        var last = ss.Last();
                        if (last == "c")
                        {
                            cs = "c";
                        }
                        else if (last == "s")
                        {
                            cs = "s";
                        }

                        className = ss[0];
                    }

                    if (cs == "")
                    {
                        cs = "cs";
                    }

                    ExcelPackage p = GetPackage(Path.GetFullPath(path));
                    Table table = GetTable(className);

                    if (cs == "cs")
                    {
                        table.ConfigType = ConfigType.cs;
                    }
                    else if (cs == "c")
                    {
                        table.ConfigType = ConfigType.c;
                    }
                    else
                    {
                        table.ConfigType = ConfigType.s;
                    }

                    table.ExcelPackages.Add(p);
                }

                // 3.解析Excel生成Table数据
                foreach (var table in tables.Values)
                {
                    ParseTableWithExcel(table);
                }

                // 4.根据Table创建 Class
                foreach (var table in tables.Values)
                {
                    GenExcelClass(table);
                }

                // 动态编译生成的配置代码
                configAssemblies[(int)ConfigType.c] = DynamicBuild(ConfigType.c);
                configAssemblies[(int)ConfigType.s] = DynamicBuild(ConfigType.s);
                configAssemblies[(int)ConfigType.cs] = DynamicBuild(ConfigType.cs);

                // 5.导出Json
                foreach (var table in tables.Values)
                {
                    Console.WriteLine($"导出Json数据 : {table.ClassName} 方式 : {table.ConfigType}");
                    ExportExcelJson(table);
                }
                
                // 6.导出Proto
                foreach (var table in tables.Values)
                {
                    Console.WriteLine($"导出ProtoBuff数据 : {table.ClassName} 方式 : {table.ConfigType}");
                    ExportExcelProtobuf(table);
                }
                
                if (Directory.Exists(clientProtoDir))
                {
                    Directory.Delete(clientProtoDir, true);
                }

                // 7.拷贝proto数据加载 Unity文件
                FileHelper.CopyDirectory("../ConfigExport/Excel/c", clientProtoDir);
                FileHelper.CopyDirectory("../ConfigExport/Excel/cs", clientProtoDir);

                Log.Console("Export Excel Sucess!");
            }
            catch (Exception e)
            {
                Log.Console(e.ToString());
            }
            finally
            {
                tables.Clear();
                foreach (var kv in packages)
                {
                    kv.Value.Dispose();
                }

                packages.Clear();
            }
        }

        private static string GetProtoDir(ConfigType configType)
        {
            return string.Format(serverProtoDir, configType.ToString());
        }

        private static Assembly GetAssembly(ConfigType configType)
        {
            return configAssemblies[(int)configType];
        }

        private static string GetClassDir(ConfigType configType)
        {
            return configType switch
            {
                ConfigType.c => ClientClassDir,
                ConfigType.s => ServerClassDir,
                _ => CSClassDir
            };
        }

        // 动态编译生成的cs代码
        private static Assembly DynamicBuild(ConfigType configType)
        {
            string classPath = GetClassDir(configType);
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            List<string> protoNames = new List<string>();
            foreach (string classFile in Directory.GetFiles(classPath, "*.cs"))
            {
                protoNames.Add(Path.GetFileNameWithoutExtension(classFile));
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(classFile)));
            }

            List<PortableExecutableReference> references = new List<PortableExecutableReference>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    if (assembly.IsDynamic)
                    {
                        continue;
                    }

                    if (assembly.Location == "")
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                PortableExecutableReference reference = MetadataReference.CreateFromFile(assembly.Location);
                references.Add(reference);
            }

            CSharpCompilation compilation = CSharpCompilation.Create(null,
                syntaxTrees.ToArray(),
                references.ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using MemoryStream memSteam = new MemoryStream();

            EmitResult emitResult = compilation.Emit(memSteam);
            if (!emitResult.Success)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Diagnostic t in emitResult.Diagnostics)
                {
                    stringBuilder.Append($"{t.GetMessage()}\n");
                }

                throw new Exception($"动态编译失败:\n{stringBuilder}");
            }

            memSteam.Seek(0, SeekOrigin.Begin);

            Assembly ass = Assembly.Load(memSteam.ToArray());
            return ass;
        }

        #region 解析Excel生成Table数据

        static void ParseTableWithExcel(Table table)
        {
            var p = table.ExcelPackages[0];
            var worksheet = p.Workbook.Worksheets.FirstOrDefault();

            const int row = 2;

            TableType tableType = TableType.IdTable;
            string keyName = worksheet.Cells[row + 2, 3].Text.Trim();
            if (keyName.ToLower() == "key")
            {
                tableType = TableType.KeyTable;
            }

            var fieldCS = "cs";

            table.TableType = tableType;
            if (tableType == TableType.IdTable)
            {
                for (int col = 3; col <= worksheet.Dimension.End.Column; ++col)
                {
                    string fieldName = worksheet.Cells[row + 2, col].Text.Trim();
                    if (fieldName == "")
                    {
                        continue;
                    }

                    if (fieldName.Contains("#"))
                    {
                        continue;
                    }

                    if (table.HeadInfos.ContainsKey(fieldName))
                    {
                        continue;
                    }

                    string fcs = worksheet.Cells[row, col].Text.Trim();
                    if (fcs != "")
                    {
                        fieldCS = fcs;
                    }

                    string fieldDesc = worksheet.Cells[row + 1, col].Text.Trim();
                    string fieldType = worksheet.Cells[row + 3, col].Text.Trim();

                    var headInfo = new HeadInfo(fieldCS, fieldDesc, fieldName, fieldType, ++table.Index, "");
                    table.HeadInfos[fieldName] = headInfo;
                }
            }
            else if (tableType == TableType.KeyTable)
            {
                for (int r = row + 4; r <= worksheet.Dimension.End.Row; ++r)
                {
                    string fcs = worksheet.Cells[r, 2].Text.Trim();
                    string key = worksheet.Cells[r, 3].Text.Trim();
                    string type = worksheet.Cells[r, 4].Text.Trim();
                    string value = worksheet.Cells[r, 5].Text.Trim();
                    string desc = worksheet.Cells[r, 6].Text.Trim();
                    if (key == "")
                    {
                        continue;
                    }

                    if (key.Contains("#"))
                    {
                        continue;
                    }

                    if (fcs != "")
                    {
                        fieldCS = fcs;
                    }

                    if (table.HeadInfos.ContainsKey(key))
                    {
                        continue;
                    }

                    var headInfo = new HeadInfo(fieldCS, desc, key, type, ++table.Index, value);
                    table.HeadInfos[key] = headInfo;
                }
            }
        }

        static void GenExcelClass(Table table)
        {
            var configType = table.ConfigType;
            var className = table.ClassName;

            string dir = GetClassDir(configType);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var classField = table.HeadInfos;
            var tableType = table.TableType;

            string exportPath = Path.Combine(dir, $"{className}.cs");

            using FileStream txt = new FileStream(exportPath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(txt);

            if (tableType == TableType.IdTable)
            {
                StringBuilder sb = new StringBuilder();
                foreach ((string _, HeadInfo headInfo) in classField)
                {
                    if (headInfo == null)
                    {
                        continue;
                    }

                    if (configType != ConfigType.cs && !headInfo.FieldCS.Contains(configType.ToString()))
                    {
                        continue;
                    }

                    sb.Append($"\t\t/// <summary>{headInfo.FieldDesc}</summary>\n");
                    string fieldType = headInfo.FieldType;
                    sb.Append($"\t\tpublic {fieldType} {headInfo.FieldName} {{ get; set; }}\n");
                }

                string content = template.Replace("(ConfigName)", className).Replace(("(Fields)"), sb.ToString());
                sw.Write(content);
            }
            else if (tableType == TableType.KeyTable)
            {
                StringBuilder sb = new StringBuilder();
                foreach ((string _, HeadInfo headInfo) in classField)
                {
                    if (headInfo == null)
                    {
                        continue;
                    }

                    if (configType != ConfigType.cs && !headInfo.FieldCS.Contains(configType.ToString()))
                    {
                        continue;
                    }

                    sb.Append($"\t\t/// <summary>{headInfo.FieldDesc}</summary>\n");
                    string fieldType = headInfo.FieldType;
                    sb.Append($"\t\tpublic {fieldType} {headInfo.FieldName} {{ get; set; }}\n");
                }

                string content = templateKey.Replace("(ConfigName)", className).Replace(("(Fields)"), sb.ToString());
                sw.Write(content);
            }

            Console.WriteLine($"导出Class : {className} , 格式 : {tableType}");
        }

        #endregion

        #region 导出json

        static void ExportExcelJson(Table table)
        {
            StringBuilder sb = new StringBuilder();

            if (table.TableType == TableType.IdTable)
            {
                sb.Append("{\"dict\": [\n");
            }
            else if (table.TableType == TableType.KeyTable)
            {
                sb.Append("{\"Config\": {");
            }

            foreach (var p in table.ExcelPackages)
            {
                foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
                {
                    if (worksheet.Name.StartsWith("#"))
                    {
                        continue;
                    }

                    ExportSheetJson(table, worksheet, sb);
                }
            }

            if (table.TableType == TableType.IdTable)
            {
                sb.Append("]}\n");
            }
            else if (table.TableType == TableType.KeyTable)
            {
                sb.Append("}}");
            }

            string dir = string.Format(jsonDir, table.ConfigType);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string jsonPath = Path.Combine(dir, $"{table.ClassName}.txt");
            using FileStream txt = new FileStream(jsonPath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(txt);
            sw.Write(sb.ToString());
        }

        static void ExportSheetJson(Table table, ExcelWorksheet worksheet, StringBuilder sb)
        {
            var classField = table.HeadInfos;
            var className = table.ClassName;
            if (table.TableType == TableType.IdTable)
            {
                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    string prefix = worksheet.Cells[row, 2].Text.Trim();
                    if (prefix.Contains("#"))
                    {
                        continue;
                    }

                    if (worksheet.Cells[row, 3].Text.Trim() == "")
                    {
                        continue;
                    }

                    sb.Append($"[{worksheet.Cells[row, 3].Text.Trim()}, {{\"_t\":\"{className}\"");
                    for (int col = 3; col <= worksheet.Dimension.End.Column; ++col)
                    {
                        string fieldName = worksheet.Cells[4, col].Text.Trim();
                        if (!classField.ContainsKey(fieldName))
                        {
                            continue;
                        }

                        HeadInfo headInfo = classField[fieldName];

                        if (headInfo == null)
                        {
                            continue;
                        }

                        string fieldN = headInfo.FieldName;
                        if (fieldN == "Id")
                        {
                            fieldN = "_id";
                        }

                        sb.Append($",\"{fieldN}\":{Convert(headInfo.FieldType, worksheet.Cells[row, col].Text.Trim())}");
                    }

                    sb.Append("}],\n");
                }
            }
            else if (table.TableType == TableType.KeyTable)
            {
                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    string fcs = worksheet.Cells[row, 2].Text.Trim();
                    string key = worksheet.Cells[row, 3].Text.Trim();

                    var headInfo = classField.GetValueOrDefault(key);
                    if (headInfo == null)
                    {
                        continue;
                    }

                    if (fcs.Contains("#"))
                    {
                        continue;
                    }

                    sb.Append($",\"{headInfo.FieldName}\":{Convert(headInfo.FieldType, headInfo.FieldValue.Trim())}");
                }
            }
        }

        private static string Convert(string type, string value)
        {
            value = value.Trim('~');
            switch (type)
            {
                // 如果是数值类型
                case "int":
                case "uint":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    if (value == "")
                    {
                        return "0";
                    }

                    return value;
                // 如果是字符类型
                case "string":
                    
                    value = value.Replace("\\", "\\\\");
                    value = value.Replace("\"", "\\\"");
                    return $"\"{value}\"";
                default:
                {
                    // 如果是枚举类型
                    var enumType = Type.GetType($"ET.{type}");
                    if (enumType != null && enumType.IsEnum)
                    {
                        return $"\"{value}\"";
                    }
                    
                    // 其他类型 为原值
                    return value;
                }
            }
        }

        #endregion

        // 根据生成的类，把json转成protobuf
        private static void ExportExcelProtobuf(Table table)
        {
            var configType = table.ConfigType;
            var className = table.ClassName;
            
            
            string dir = GetProtoDir(configType);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Assembly ass = GetAssembly(configType);
            Type type = ass.GetType($"ET.{className}Category");
            Type subType = ass.GetType($"ET.{className}");

            IMerge final = Activator.CreateInstance(type) as IMerge;

            string p = Path.Combine(string.Format(jsonDir, configType));
            string[] ss = Directory.GetFiles(p, $"{className}*.txt");
            List<string> jsonPaths = ss.ToList();

            jsonPaths.Sort();
            jsonPaths.Reverse();
            foreach (string jsonPath in jsonPaths)
            {
                string json = File.ReadAllText(jsonPath);
                try
                {
                    object deserialize = BsonSerializer.Deserialize(json, type);
                    final.Merge(deserialize);
                }
                catch (Exception e)
                {
                    throw new Exception($"json : {jsonPath} error", e);
                }
            }

            string path = Path.Combine(dir, $"{className}Category.bytes");

            using FileStream file = File.Create(path);
            file.Write(final.ToBson());
        }
    }
}