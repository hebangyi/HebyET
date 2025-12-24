using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class ComponentSystemGenerator: ISourceGenerator
{
    public List<SolutionLoader> SolutionLoaders = new List<SolutionLoader>();
    
    
    public class SolutionLoader
    {
        public string SearchFolder;
        public HashSet<string> SearchFiles = new HashSet<string>();
        public string OutFolder;

        public void Search()
        {
            var filePaths = Directory.GetFiles(SearchFolder, "*.cs", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                SearchFiles.Add(Path.GetFileNameWithoutExtension(filePath));
            }
        }
    }
    
    
    public const string Template = $$"""
                                                   namespace {namespaceName}
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
        
        string fileName = $"{className}Handler.cs";
        var filePath = Path.Combine(solutionLoader.OutFolder, fileName);
        if (File.Exists(filePath))
        {
            return;
        }
        
        string template = Template;
        string namespaceName = "ET";
        var code = template.Replace("{namespaceName}", namespaceName);
        code = code.Replace("{className}", className);
        File.WriteAllText(filePath, code);
    }

    class ComponentClassReceiver : ISyntaxContextReceiver
    {
        // ComponentType
        public HashSet<ClassDeclarationSyntax> CompnentTypes = new ();
        
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
            
            // Entity | Component
            if (classTypeSymbol.HasAttribute(Definition.ChildOfAttribute) || classTypeSymbol.HasAttribute(Definition.ComponentOfAttribute))
            {
                if(classTypeSymbol.BaseTypes().Any(x=> x.Name == "Entity"))
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