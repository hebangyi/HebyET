using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ET
{
    internal class OpcodeInfo
    {
        public string Name;
        public int Opcode;
    }

    public static class Proto2CS
    {
        public static void Export()
        {
            InnerProto2CS.Proto2CS();
            Log.Console("proto2cs succeed!");
        }
    }

    public static partial class InnerProto2CS
    {
        private const string clientProtoDir = "../Config/Proto/Client";
        private const string clientOutputTempFilePath = "../Config/Proto/Temp/Client.proto";
        
        
        
        private const string serverProtoDir = "../Config/Proto/Server";
        private const string serverOutputTempFilePath = "../Config/Proto/Temp/Server.proto";
        
        
        private const string clientMessagePath = "../Unity/Assets/Scripts/Model/Client/Generate/Message/";
        private const string serverMessagePath = "../DotNet/Model/Server/Generate/Message/";
        private const string clientServerMessagePath = "../Unity/Assets/Scripts/Model/Share/Generate/Message/";
        private static readonly char[] splitChars = [' ', '\t'];
        private static readonly List<OpcodeInfo> msgOpcode = [];

        public static void Proto2CS()
        {
            /*
    
             
             foreach (string s in list)
            {
                if (!s.EndsWith(".proto"))
                {
                    continue;
                }

                string fileName = Path.GetFileNameWithoutExtension(s);
                string[] ss2 = fileName.Split('_');
                string protoName = ss2[0];
                string cs = ss2[1];
                int startOpcode = int.Parse(ss2[2]);
                ProtoFile2CS(fileName, protoName, cs, startOpcode);
            }
            
                        
            RemoveUnusedMetaFiles(clientMessagePath);
            RemoveUnusedMetaFiles(serverMessagePath);
            RemoveUnusedMetaFiles(clientServerMessagePath);
            */
            
            msgOpcode.Clear();

            RemoveAllFilesExceptMeta(clientMessagePath);
            RemoveAllFilesExceptMeta(serverMessagePath);
            RemoveAllFilesExceptMeta(clientServerMessagePath);
            
            // 生成合成Proto
            GenerateServerProto();
            GenerateClientProto();
            
            ProtoFile2CS(clientOutputTempFilePath, "ClientMessage", "C", 10000);
            ProtoFile2CS(serverOutputTempFilePath, "ServerMessage", "S", 20000);
            
            
            RemoveUnusedMetaFiles(clientMessagePath);
            RemoveUnusedMetaFiles(serverMessagePath);
            RemoveUnusedMetaFiles(clientServerMessagePath);
        }


        public static void GenerateClientProto()
        {
            List<string> fileList = FileHelper.GetAllFiles(clientProtoDir, "*proto");
            StringBuilder sb = new ();
            sb.AppendLine("syntax = \"proto3\";");
            sb.AppendLine("package ET;");
            
            foreach (string filePath in fileList)
            {
                if (!filePath.EndsWith(".proto"))
                {
                    continue;
                }
                
                string content = File.ReadAllText(filePath);
                sb.AppendLine(content);
                sb.AppendLine();
            }
            File.WriteAllText(clientOutputTempFilePath, sb.ToString());
        }
        
        public static void GenerateServerProto()
        {
            List<string> fileList = FileHelper.GetAllFiles(serverProtoDir, "*proto");
            StringBuilder sb = new ();
            sb.AppendLine("syntax = \"proto3\";");
            sb.AppendLine("package ET;");
            
            foreach (string filePath in fileList)
            {
                if (!filePath.EndsWith(".proto"))
                {
                    continue;
                }
                
                string content = File.ReadAllText(filePath);
                sb.AppendLine(content);
                sb.AppendLine();
            }
            
            File.WriteAllText(serverOutputTempFilePath, sb.ToString());
        }
        
        
        private static void ProtoFile2CS(string filePath, string className, string cs, int startOpcode)
        {
            msgOpcode.Clear();
            
            string s = File.ReadAllText(filePath);

            StringBuilder sb = new();
            sb.Append("// This Is Auto Generate, Do Not Edit!\n");
            sb.Append("using MemoryPack;\n");
            sb.Append("using System.Collections.Generic;\n\n");
            sb.Append($"namespace ET\n");
            sb.Append("{\n");

            bool isMsgStart = false;
            bool isUnitElementData = false;
            string msgName = "";
            string responseTypeStr = "";
            string responseType = "";
            string scene = "";
            string entity = "";
            
            
            StringBuilder sbDispose = new();
            Regex responseTypeRegex = ResponseTypeRegex();
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();
                if (string.IsNullOrEmpty(newline))
                {
                    continue;
                }

                if (responseTypeRegex.IsMatch(newline))
                {
                    responseTypeStr = responseTypeRegex.Replace(newline, string.Empty);
                    string[] strArr = responseTypeStr.Trim().Split(' ');

                    for (int i = 0; i < strArr.Length; i++)
                    {
                        if (i == 0)
                        {
                            responseType = strArr[i];    
                        }

                        if (i == 1)
                        {
                            scene = strArr[i];
                        }

                        if (i == 2)
                        {
                            entity = strArr[i];
                        }
                    }
                    continue;
                }

                if (!isMsgStart && newline.StartsWith("//"))
                {
                    if (newline.StartsWith("///"))
                    {
                        sb.Append("\t/// <summary>\n");
                        sb.Append($"\t/// {newline.TrimStart('/', ' ')}\n");
                        sb.Append("\t/// </summary>\n");
                    }
                    else
                    {
                        sb.Append($"\t// {newline.TrimStart('/', ' ')}\n");
                    }

                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    isMsgStart = true;
                    isUnitElementData = false;

                    string parentClass = "";
                    msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(["//"], StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    msgOpcode.Add(new OpcodeInfo() { Name = msgName, Opcode = ++startOpcode });

                    sb.Append($"\t[MemoryPackable]\n");
                    sb.Append($"\t[Message({className}.{msgName})]\n");

                    if (!string.IsNullOrEmpty(entity))
                    {
                        sb.Append($"\t[ResponseType(nameof({responseType}), \"{scene}\", \"{entity}\")]\n");
                    }
                    else if (!string.IsNullOrEmpty(scene))
                    {
                        sb.Append($"\t[ResponseType(nameof({responseType}), \"{scene}\")]\n");
                    }
                    else if (!string.IsNullOrEmpty(responseType))
                    {
                        sb.Append($"\t[ResponseType(nameof({responseType}))]\n");
                    }

                    sb.Append($"\tpublic partial class {msgName} : MessageObject");

                    if (parentClass is "IActorMessage" or "IActorRequest" or "IActorResponse")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass is "IUnitEntityElemData")
                    {
                        isUnitElementData = true;
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass != "")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else
                    {
                        sb.Append('\n');
                    }

                    continue;
                }

                if (isMsgStart)
                {
                    if (newline.StartsWith('{'))
                    {
                        sbDispose.Clear();
                        sb.Append("\t{\n");
                        if (isUnitElementData)
                        {
                            sb.AppendLine("\t\tprivate IDirtyHandler m_DirtyHandler;");    
                        }
                        sb.AppendLine("\t\tprivate long m_InstanceId;");
                        sb.Append("\n");
                        
                        if (isUnitElementData)
                        {
                            sb.Append($"\t\tpublic static {msgName} Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)\n");
                            sb.AppendLine($"\t\t{{");
                            sb.AppendLine($"\t\t\tvar instance = ObjectPool.Instance.Fetch(typeof({msgName}), isFromPool) as {msgName};");
                            sb.AppendLine($"\t\t\tinstance.m_DirtyHandler = dirtyHandler;");
                            sb.AppendLine($"\t\t\tinstance.m_InstanceId = instanceId;");
                            sb.Append($"\t\t\treturn instance;");
                            sb.Append($"\n\t\t}}\n\n");
                            
                            sbDispose.AppendLine($"\t\t\tthis.m_DirtyHandler = null;");
                            sbDispose.AppendLine($"\t\t\tthis.m_InstanceId = default;\n");
                        }
                        else
                        {
                            sb.Append($"\t\tpublic static {msgName} Create(bool isFromPool = false)\n\t\t{{\n\t\t\treturn ObjectPool.Instance.Fetch(typeof({msgName}), isFromPool) as {msgName};\n\t\t}}\n\n");    
                        }
                        continue;
                    }

                    if (newline.StartsWith('}'))
                    {
                        isMsgStart = false;
                        responseTypeStr = "";
                        responseType = "";
                        scene = "";
                        entity = "";

                        // 加了no dispose则自己去定义dispose函数，不要自动生成
                        if (!newline.Contains("// no dispose"))
                        {
                            sb.Append($"\t\tpublic override void Dispose()\n\t\t{{\n\t\t\tif (!this.IsFromPool)\n\t\t\t{{\n\t\t\t\treturn;\n\t\t\t}}\n{sbDispose.ToString().TrimEnd('\t')}\n\t\t\tObjectPool.Instance.Recycle(this);\n\t\t}}\n");
                        }

                        if (isUnitElementData)
                        {
                            sb.AppendLine();
                            sb.AppendLine("""
                                                  public void Dirty()
                                                  {
                                                      this.m_DirtyHandler?.Dirty(m_InstanceId, this);
                                                  }
                                          """);
                                    
                            
                            sb.AppendLine();
                        }

                        sb.Append("\t}\n\n");
                        continue;
                    }

                    if (newline.StartsWith("//"))
                    {
                        sb.Append("\t\t/// <summary>\n");
                        sb.Append($"\t\t/// {newline.TrimStart('/', ' ')}\n");
                        sb.Append("\t\t/// </summary>\n");
                        continue;
                    }

                    string memberStr;
                    if (newline.Contains("//"))
                    {
                        string[] lineSplit = newline.Split("//");
                        memberStr = lineSplit[0].Trim();
                        sb.Append("\t\t/// <summary>\n");
                        sb.Append($"\t\t/// {lineSplit[1].Trim()}\n");
                        sb.Append("\t\t/// </summary>\n");
                    }
                    else
                    {
                        memberStr = newline;
                    }

                    if (memberStr.StartsWith("map<"))
                    {
                        if (isUnitElementData)
                        {
                            UnitEntityElemDataMap(sb, memberStr, sbDispose);
                        }
                        else
                        {
                            Map(sb, memberStr, sbDispose);    
                        }
                    }
                    else if (memberStr.StartsWith("repeated"))
                    {
                        if (isUnitElementData)
                        {
                            UnitEntityElemDataRepeated(sb, memberStr, sbDispose);
                        }
                        else
                        {
                            Repeated(sb, memberStr, sbDispose);   
                        }
                    }
                    else
                    {
                        if (isUnitElementData)
                        {
                            UnitEntityElemDataMembers(sb, memberStr, sbDispose);
                        }
                        else
                        {
                            Members(sb, memberStr, sbDispose);   
                        }
                    }
                }
            }

            sb.Append("\tpublic static class " + className + "\n\t{\n");
            foreach (OpcodeInfo info in msgOpcode)
            {
                sb.Append($"\t\tpublic const ushort {info.Name} = {info.Opcode};\n");
            }

            sb.Append("\t}\n");

            sb.Append('}');

            sb.Replace("\t", "    ");
            string result = sb.ToString().ReplaceLineEndings("\r\n");

            if (cs.Contains('C'))
            {
                GenerateCS(result, clientServerMessagePath, className);
            }

            if (cs.Contains('S'))
            {
                GenerateCS(result, serverMessagePath, className);
            }
        }

        private static void GenerateCS(string result, string path, string proto)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string csPath = Path.Combine(path, Path.GetFileNameWithoutExtension(proto) + ".cs");
            using FileStream txt = new(csPath, FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter sw = new(txt);
            sw.Write(result);
        }

        
        private static void UnitEntityElemDataMap(StringBuilder sb, string newline, StringBuilder sbDispose)
        {
            int start = newline.IndexOf('<') + 1;
            int end = newline.IndexOf('>');
            string types = newline.Substring(start, end - start);
            string[] ss = types.Split(',');
            string keyType = ConvertType(ss[0].Trim());
            string valueType = ConvertType(ss[1].Trim());
            string tail = newline[(end + 1)..];
            ss = tail.Trim().Replace(";", "").Split(' ');
            string v = ss[0];
            int n = int.Parse(ss[2]);

            sb.AppendLine($"\t\tprivate Dictionary<{keyType}, {valueType}> _{v} = new();\n");
            sb.Append("\t\t[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]\n");
            sb.Append($"\t\t[MemoryPackOrder({n - 1})]\n");
            sb.AppendLine($$"""
                                public Dictionary<{{keyType}}, {{valueType}}> {{v}} 
                                {
                                    get => _{{v}};
                                    set {
                                        _{{v}} = value;
                                        this.Dirty();
                                    }
                                }
                        """);

            sbDispose.AppendLine($"\t\t\tthis._{v}.Clear();");
        }
        
        
        private static void Map(StringBuilder sb, string newline, StringBuilder sbDispose)
        {
            int start = newline.IndexOf('<') + 1;
            int end = newline.IndexOf('>');
            string types = newline.Substring(start, end - start);
            string[] ss = types.Split(',');
            string keyType = ConvertType(ss[0].Trim());
            string valueType = ConvertType(ss[1].Trim());
            string tail = newline[(end + 1)..];
            ss = tail.Trim().Replace(";", "").Split(' ');
            string v = ss[0];
            int n = int.Parse(ss[2]);

            sb.Append("\t\t[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]\n");
            sb.Append($"\t\t[MemoryPackOrder({n - 1})]\n");
            sb.Append($"\t\tpublic Dictionary<{keyType}, {valueType}> {v} {{ get; set; }} = new();\n");

            sbDispose.AppendLine($"\t\t\tthis.{v}.Clear();");
        }

        private static void UnitEntityElemDataRepeated(StringBuilder sb, string newline, StringBuilder sbDispose)
        {
            try
            {
                int index = newline.IndexOf(';');
                newline = newline.Remove(index);
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[1];
                type = ConvertType(type);
                string name = ss[2];
                int n = int.Parse(ss[4]);

                
                sb.AppendLine($"\t\tprivate List<{type}> _{name} = new();\n");
                sb.Append($"\t\t[MemoryPackOrder({n - 1})]\n");
                sb.AppendLine($$"""
                                        public List<{{type}}> {{name}}
                                        {
                                            get => _{{name}};
                                            set {
                                                _{{name}} = value;
                                                this.Dirty();
                                            }
                                        }
                                """);
                
                
                sbDispose.AppendLine($"\t\t\tthis._{name}.Clear();");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{newline}\n {e}");
            }
        }

        private static void Repeated(StringBuilder sb, string newline, StringBuilder sbDispose)
        {
            try
            {
                int index = newline.IndexOf(';');
                newline = newline.Remove(index);
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[1];
                type = ConvertType(type);
                string name = ss[2];
                int n = int.Parse(ss[4]);

                sb.Append($"\t\t[MemoryPackOrder({n - 1})]\n");
                sb.Append($"\t\tpublic List<{type}> {name} {{ get; set; }} = new();\n\n");

                sbDispose.AppendLine($"\t\t\tthis.{name}.Clear();");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{newline}\n {e}");
            }
        }

        private static string ConvertType(string type)
        {
            return type switch
            {
                "int16" => "short",
                "int32" => "int",
                "bytes" => "byte[]",
                "uint32" => "uint",
                "long" => "long",
                "int64" => "long",
                "uint64" => "ulong",
                "uint16" => "ushort",
                _ => type
            };
        }
        
        private static void UnitEntityElemDataMembers(StringBuilder sb, string newline, StringBuilder sbDispose)
        {
            try
            {
                int index = newline.IndexOf(';');
                newline = newline.Remove(index);
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[0];
                string name = ss[1];
                int n = int.Parse(ss[3]);
                string typeCs = ConvertType(type);

                sb.AppendLine($"\t\tprivate {typeCs} _{name};\n");
                sb.Append($"\t\t[MemoryPackOrder({n - 1})]\n");
                sb.AppendLine($$"""
                                    public {{typeCs}} {{name}}
                                    {
                                        get => _{{name}};
                                        set {
                                            _{{name}} = value;
                                            this.Dirty();
                                        }
                                    }
                            """);
                
                
                
                switch (typeCs)
                {
                    case "bytes":
                    {
                        break;
                    }
                    default:
                        sbDispose.AppendLine($"\t\t\tthis._{name} = default;");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{newline}\n {e}");
            }
        }
                
        private static void Members(StringBuilder sb, string newline, StringBuilder sbDispose)
        {
            try
            {
                int index = newline.IndexOf(';');
                newline = newline.Remove(index);
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[0];
                string name = ss[1];
                int n = int.Parse(ss[3]);
                string typeCs = ConvertType(type);

                sb.Append($"\t\t[MemoryPackOrder({n - 1})]\n");
                sb.Append($"\t\tpublic {typeCs} {name} {{ get; set; }}\n\n");

                switch (typeCs)
                {
                    case "bytes":
                    {
                        break;
                    }
                    default:
                        sbDispose.AppendLine($"\t\t\tthis.{name} = default;");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{newline}\n {e}");
            }
        }

        /// <summary>
        /// 删除meta以外的所有文件
        /// </summary>
        static void RemoveAllFilesExceptMeta(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            DirectoryInfo targetDir = new(directory);
            FileInfo[] fileInfos = targetDir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo info in fileInfos)
            {
                if (!info.Name.EndsWith(".meta"))
                {
                    File.Delete(info.FullName);
                }
            }
        }

        /// <summary>
        /// 删除多余的meta文件
        /// </summary>
        static void RemoveUnusedMetaFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            DirectoryInfo targetDir = new(directory);
            FileInfo[] fileInfos = targetDir.GetFiles("*.meta", SearchOption.AllDirectories);
            foreach (FileInfo info in fileInfos)
            {
                string pathWithoutMeta = info.FullName.Remove(info.FullName.LastIndexOf(".meta", StringComparison.Ordinal));
                if (!File.Exists(pathWithoutMeta) && !Directory.Exists(pathWithoutMeta))
                {
                    File.Delete(info.FullName);
                }
            }
        }

        [GeneratedRegex(@"//\s*ResponseType")]
        private static partial Regex ResponseTypeRegex();
    }
}