using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class ComponentSystemGenerator : ISourceGenerator
{
    public List<SolutionLoader> SolutionLoaders = new List<SolutionLoader>();
    public string SystemTxt = "";
    public string RootPath = "";
    public const string SystemFile = "System.txt";
    

    public class SolutionLoader
    {
        public string SearchFolder;
        public HashSet<string> SearchFiles = new HashSet<string>();
        public string OutFolder;

        public void Search()
        {
            if (!Directory.Exists(SearchFolder))
            {
                return;
            }

            var filePaths = Directory.GetFiles(SearchFolder, "*.cs", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                // File.AppendAllText("C:\\Users\\Administrator\\Desktop\\abc.txt", $"File Class {filePath}, File Exists \n");
                SearchFiles.Add(Path.GetFileNameWithoutExtension(filePath));
            }
        }
    }

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

    public void Initialize(GeneratorInitializationContext context)
    {
        string rootPath = Path.GetFullPath("./");
        if (rootPath.EndsWith("Unity\\"))
        {
            rootPath = rootPath.Substring(0, rootPath.Length - "Unity\\".Length);
        }

        this.RootPath = rootPath;

        var systemFilePath = Path.Combine(rootPath, SystemFile);
        if (File.Exists(systemFilePath))
        {
            this.SystemTxt = File.ReadAllText(Path.Combine(rootPath, SystemFile));    
        }
        
        SolutionLoader clientModelViewLoader = new SolutionLoader();
        clientModelViewLoader.SearchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\ModelView\\Client");
        clientModelViewLoader.OutFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\HotfixView\\Client\\Module\\System");
        clientModelViewLoader.Search();
        
        SolutionLoader shareModelLoader = new SolutionLoader();
        shareModelLoader.SearchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Model\\Share");
        shareModelLoader.OutFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Hotfix\\Share\\Module\\System");
        shareModelLoader.Search();
        
        SolutionLoader clientModelLoader = new SolutionLoader();
        clientModelLoader.SearchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Model\\Client");
        clientModelLoader.OutFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Hotfix\\Client\\Module\\System");
        clientModelLoader.Search();

        SolutionLoader serverModelLoader = new SolutionLoader();
        serverModelLoader.SearchFolder = Path.Combine(rootPath, "DotNet\\Model\\Server");
        serverModelLoader.OutFolder = Path.Combine(rootPath, "DotNet\\Hotfix\\Server\\Module\\System");
        serverModelLoader.Search();

    
        SolutionLoaders.Add(clientModelViewLoader);
        SolutionLoaders.Add(shareModelLoader);
        SolutionLoaders.Add(clientModelLoader);
        SolutionLoaders.Add(serverModelLoader);

        context.RegisterForSyntaxNotifications(() => ComponentClassReceiver.Create());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not ComponentClassReceiver receiver)
        {
            return;
        }

        if (receiver.CompnentTypes.Count == 0)
        {
            return;
        }

        foreach (var type in receiver.CompnentTypes)
        {
            this.GenerateFiles(type, context, receiver);
        }
    }

    private void GenerateFiles(ClassDeclarationSyntax classDeclarationSyntax, GeneratorExecutionContext context, ComponentClassReceiver receiver)
    {
        string className = classDeclarationSyntax.Identifier.Text;
        SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
        INamedTypeSymbol? classTypeSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        INamespaceSymbol? namespaceSymbol = classTypeSymbol?.ContainingNamespace;

        SolutionLoader solutionLoader = null;
        foreach (var s in this.SolutionLoaders)
        {
            if (s.SearchFiles.Contains(className))
            {
                // 找到对应的 loader 
                solutionLoader = s;
            }
        }

        if (solutionLoader == null)
        {
            return;
        }

        if (!Directory.Exists(solutionLoader.OutFolder))
        {
            Directory.CreateDirectory(solutionLoader.OutFolder);
        }

        string fileName = $"{className}System.cs";

        if (this.SystemTxt.Contains(fileName))
        {
            return;
        }
        
        var filePath = Path.Combine(solutionLoader.OutFolder, fileName);
        
        if (File.Exists(filePath))
        {
            return;
        }

        string template = Template;
        string namespaceName = namespaceSymbol.ToString();

        var code = template.Replace("{namespaceName}", namespaceName);
        code = code.Replace("{componentType}", className);


        StringBuilder methodBuilder = new StringBuilder();
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
                    for (int i = 0 ; i < paramTypes.Length ; i++)
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
        File.AppendAllText(Path.Combine(this.RootPath, SystemFile), $"{fileName}\n");
    }

    class ComponentClassReceiver : ISyntaxContextReceiver
    {
        // ComponentType
        public HashSet<ClassDeclarationSyntax> CompnentTypes = new();

        // 已经生成的Component 和System的对应关系
        public Dictionary<String, String> Component2Systems = new();

        internal static ISyntaxContextReceiver Create()
        {
            return new ComponentClassReceiver();
        }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return;
            }

            var classTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            if (classTypeSymbol == null)
            {
                return;
            }

            // 排除FGUI
            if (classTypeSymbol.HasAttribute(Definition.FGUITagAttribute))
            {
                return;
            }
            
            
            if (classTypeSymbol.HasAttribute(Definition.FGUITagAttribute))
            {
                return;
            }
            
            if (classTypeSymbol.HasAttribute(Definition.FGUIDLGAttribute))
            {
                return;
            }
            

            // Entity | Component
            if (classTypeSymbol.HasAttribute(Definition.ChildOfAttribute) || classTypeSymbol.HasAttribute(Definition.ComponentOfAttribute))
            {
                if (classTypeSymbol.BaseTypes().Any(x => x.Name == "Entity"))
                {
                    CompnentTypes.Add(classDeclarationSyntax);
                }
            }

            if (classTypeSymbol.HasAttribute(Definition.EntitySystemOfAttribute))
            {
                var attributeData = classTypeSymbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == "EntitySystemOfAttribute");

                if (attributeData != null && attributeData.ConstructorArguments.Length > 0)
                {
                    var arg = attributeData.ConstructorArguments.First();
                    if (arg.Value != null)
                    {
                        string typeName = arg.Value.ToString().Split('.').Last();
                        Component2Systems[typeName] = classTypeSymbol.Name;
                    }
                }
            }
        }
    }
}