using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET
{
    public static class MessageHandlerGenerator
    {
        public const string HandlerFile = "Handler.txt";

        public static void ExportHandler()
        {
            Console.WriteLine("检测生成 Handler");

            string rootPath = Path.GetFullPath("../");
            string handlerText = File.ReadAllText(Path.Combine(rootPath, HandlerFile));

            var floders = new string[]
            {
                "Unity\\Assets\\Scripts\\Model\\Share\\Generate",
                // "DotNet\\Model\\Server\\Generate"
            };
            
            foreach (var f in floders)
            {
                var searchFolder = Path.Combine(rootPath, f);
                var allFiles = Directory.GetFiles(searchFolder, "*.cs", SearchOption.AllDirectories);

                foreach (var file in allFiles)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

                    // 遍历所有类
                    var classes = root.DescendantNodes()
                            .OfType<ClassDeclarationSyntax>();
                    foreach (var classDecl in classes)
                    {
                        foreach (var attribute in classDecl.AttributeLists)
                        {
                            if (attribute.ToString().Contains("ResponseType"))
                            {
                                GenerateFile(rootPath, handlerText, classDecl, attribute);
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine("检测生成 Handler 完成");
        }

        private static void GenerateFile(string rootPath, string handlerText, ClassDeclarationSyntax classDecl, AttributeListSyntax attribute)
        {
            string className = classDecl.Identifier.Text;
            if (handlerText.Contains(className))
            {
                return;
            }

            string reqType = className;
            string resType = "";
            string entity = "Entity";
            string scene = "Scene";
            string floder = "";
            
            for (int i = 0; i < attribute.Attributes[0].ArgumentList.Arguments.Count; i++)
            {
                if (i == 0)
                {
                    resType = attribute.Attributes[0].ArgumentList.Arguments[i].ToString().Substring(7).TrimEnd(')');
                }
                else if (i == 1)
                {
                    scene = attribute.Attributes[0].ArgumentList.Arguments[i].ToString().Replace("\"", "");
                    floder = scene;
                }
                else if (i == 2)
                {
                    entity = attribute.Attributes[0].ArgumentList.Arguments[i].ToString().Replace("\"", "");
                }
            }
            
            
            File.AppendAllText(Path.Combine(rootPath, HandlerFile), $"{className}\n");

            string namespaceName = "ET.Server";

            string path = "../DotNet/Hotfix/Server/Game/MessageHandler";
            string fileName = $"{className}Handler.cs";
            var f = Path.Combine(path, scene);
            var filePath = Path.Combine(path, scene, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string template = HandlerTemplate;
            var code = template.Replace("{namespaceName}", namespaceName);
            code = code.Replace("{scene}", scene);
            code = code.Replace("{reqType}", reqType);
            code = code.Replace("{resType}", resType);
            code = code.Replace("{entity}", entity);
            code = code.Replace("{entityLower}", entity.ToLower());
            if (!Directory.Exists(f))
            {
                Directory.CreateDirectory(f);
            }
            File.WriteAllText(filePath, code);
        }

        public const string HandlerTemplate = $$"""
                                                namespace {namespaceName}
                                                {
                                                    [MessageClientHandler(SceneType.{scene})]
                                                    public class {reqType}Handler : MessageClientHandler<{entity}, {reqType}, {resType}>
                                                    {
                                                        protected override void Run({entity} {entityLower}, {reqType} request, {resType} response)
                                                        {
                                                        }
                                                    }
                                                }
                                                """;
    }
}