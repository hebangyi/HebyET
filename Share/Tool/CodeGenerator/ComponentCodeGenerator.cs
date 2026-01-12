using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET
{
    public class SolutionLoader
    {
        public string SearchFolder;
        public string OutFolder;
    }

    public static class ComponentCodeGenerator
    {
        public const string SystemFile = "System.txt";

        public const string Template = $$"""
                                         namespace {namespaceName}
                                         {
                                            [FriendOf(typeof({componentType}))]
                                            [EntitySystemOf(typeof({componentType}))]
                                            public static partial class {componentType}System
                                            {
                                                {EntitySystem}
                                            }
                                         }
                                         """;

        public const string AwakeSystemTemplate = $$"""
                                                    
                                                           [EntitySystem]
                                                           private static void Awake(this {componentType} self{param})
                                                           {
                                                           }
                                                    """;

        public const string EntitySystemTemplate = $$"""
                                                     
                                                            [EntitySystem]
                                                            private static void {lifeCycle}(this {componentType} self)
                                                            {
                                                            }
                                                     """;

        public static void ExportComponentSystem()
        {
            Console.WriteLine("开始检测生成 Component System");
            string rootPath = Path.GetFullPath("../");
            string SystemTxt = File.ReadAllText(Path.Combine(rootPath, SystemFile));

            List<SolutionLoader> loaders = new List<SolutionLoader>();
            SolutionLoader clientModelViewLoader = new();
            clientModelViewLoader.SearchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\ModelView\\Client");
            clientModelViewLoader.OutFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\HotfixView\\Client\\Module\\System");

            SolutionLoader shareModelLoader = new();
            shareModelLoader.SearchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Model\\Share");
            shareModelLoader.OutFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Hotfix\\Share\\Module\\System");

            SolutionLoader clientModelLoader = new();
            clientModelLoader.SearchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Model\\Client");
            clientModelLoader.OutFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Hotfix\\Client\\Module\\System");

            SolutionLoader serverModelLoader = new();
            serverModelLoader.SearchFolder = Path.Combine(rootPath, "DotNet\\Model\\Server");
            serverModelLoader.OutFolder = Path.Combine(rootPath, "DotNet\\Hotfix\\Server\\Module\\System");

            loaders.Add(clientModelViewLoader);
            loaders.Add(shareModelLoader);
            loaders.Add(clientModelLoader);
            loaders.Add(serverModelLoader);

            foreach (var loader in loaders)
            {
                var allFiles = Directory.GetFiles(loader.SearchFolder, "*.cs", SearchOption.AllDirectories);
                foreach (var file in allFiles)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    // 遍历所有类
                    var classes = root.DescendantNodes()
                            .OfType<ClassDeclarationSyntax>();
                    foreach (var classDecl in classes)
                    {
                        GenerateSystemFile(loader, classDecl, SystemTxt, rootPath);
                    }
                }
            }
            
            Console.WriteLine("生成 Component System 完成");
        }

        public static void GenerateSystemFile(SolutionLoader solutionLoader, ClassDeclarationSyntax classDeclarationSyntax, string SystemTxt,
        string rootPath)
        {
            string className = classDeclarationSyntax.Identifier.Text;
            string nameSpaceName = "ET";

            if (classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDecl)
            {
                nameSpaceName = namespaceDecl.Name.ToString();
            }

            
            if (SystemTxt.Contains(className))
            {
                return;
            }
            
            File.AppendAllText(Path.Combine(rootPath, SystemFile), $"{className}\n");
            
            Console.WriteLine(className);
            
            if (!Directory.Exists(solutionLoader.OutFolder))
            {
                Directory.CreateDirectory(solutionLoader.OutFolder);
            }

            string targetFileName = $"{className}System.cs";
            var filePath = Path.Combine(solutionLoader.OutFolder, targetFileName);

            if (File.Exists(filePath))
            {
                return;
            }

            string template = Template;
            var code = template.Replace("{namespaceName}", nameSpaceName);
            code = code.Replace("{componentType}", className);

            if (classDeclarationSyntax.BaseList != null)
            {
                if (classDeclarationSyntax.BaseList.Types.FirstOrDefault(x => x.Type.ToString().Contains("Entity")) == null)
                {
                    return;
                }

                StringBuilder methodBuilder = new();
                foreach (var type in classDeclarationSyntax.BaseList.Types)
                {
                    if (type.ToString().Contains("IAwake"))
                    {
                        var typeStr = type.ToString();
                        int leftBracketIndex = typeStr.IndexOf('<');
                        int rightBracketIndex = typeStr.LastIndexOf('>');

                        if (leftBracketIndex == -1 || rightBracketIndex == -1 || rightBracketIndex < leftBracketIndex)
                        {
                            string part = AwakeSystemTemplate.Replace("{componentType}", className).Replace("{param}", "");
                            methodBuilder.Append(part);
                            methodBuilder.AppendLine();
                        }
                        else
                        {
                            string contentInsideBrackets = typeStr.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
                            var paramTypes = contentInsideBrackets.Split(',');

                            string param = "";
                            for (int i = 0; i < paramTypes.Length; i++)
                            {
                                param = ", ";
                                param += $"{paramTypes[i]} p{i + 1}";
                            }

                            var part = AwakeSystemTemplate.Replace("{componentType}", className).Replace("{param}", param);
                            methodBuilder.Append(part);
                            methodBuilder.AppendLine();
                        }
                    }
                    else if (type.ToString().Contains("IUpdate"))
                    {
                        string part = EntitySystemTemplate.Replace("{lifeCycle}", "Update").Replace("{componentType}", className);
                        methodBuilder.Append(part);
                        methodBuilder.AppendLine();
                    }
                    else if (type.ToString().Contains("IDestroy"))
                    {
                        string part = EntitySystemTemplate.Replace("{lifeCycle}", "Destroy").Replace("{componentType}", className);
                        methodBuilder.Append(part);
                        methodBuilder.AppendLine();
                    }
                }

                code = code.Replace("{EntitySystem}", methodBuilder.ToString());
                File.WriteAllText(filePath, code);

                Console.WriteLine($"生成System文件: {filePath}");
            }

            
        }
    }
}