using System;
using System.Collections.Generic;
using System.IO;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class MessageHandlerGenerator : ISourceGenerator
{
    // static string FileLog = "C:\\Users\\56312\\Desktop\\a.txt";
    
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => MessageHandlerReceiver.Create());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not MessageHandlerReceiver receiver || receiver.Types.Count == 0)
        {
            return;
        }

        foreach (var type in receiver.Types)
        {
            this.GenerateFiles(type, context);
        }
    }

    public const string SessionHandlerTemplate = $$"""
                                                   namespace {namespaceName}
                                                   {
                                                   }
                                                   """;

    private void GenerateFiles(ClassDeclarationSyntax classDeclarationSyntax, GeneratorExecutionContext context)
    {
        try
        {
            string className = classDeclarationSyntax.Identifier.Text;

            SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

            INamedTypeSymbol? classTypeSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            INamespaceSymbol? namespaceSymbol = classTypeSymbol?.ContainingNamespace;
            string namespaceName = "ET.Server";
            if (classTypeSymbol == null)
            {
                return;
            }
            
            string rootPath = Path.GetFullPath("./");
            if (rootPath.EndsWith("Unity\\"))
            {
                rootPath = rootPath.Substring(0, rootPath.Length - "Unity\\".Length);
            }

            string path = Path.Combine(rootPath, "Hotfix/Server/Game/MessageHandler");
            string fileName = $"{className}Handler.cs";
            var filePath = Path.Combine(path, fileName);

            // File.AppendAllText(FileLog, $"Start Class {classTypeSymbol.Name}, File Path {filePath} \n");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filePath))
            {
                // File.AppendAllText(FileLog, $"File Class {classTypeSymbol.Name}, File Exists \n");
                return;
            }

            if (classTypeSymbol.HasInterface(Definition.ISessionRequest))
            {
                string template = SessionHandlerTemplate;
                var code = template.Replace("{namespaceName}", namespaceName);
                code = code.Replace("{className}", className);
                File.WriteAllText(filePath, code);
                // File.AppendAllText(FileLog, $"File Save At {filePath} \n");
            }
            

            // File.AppendAllText(FileLog, $"Finish {classTypeSymbol.Name} \n");
        }
        catch (Exception e)
        {
            // File.AppendAllText(FileLog, $"Class Exception {e}  \n");
        }
    }

    class MessageHandlerReceiver : ISyntaxContextReceiver
    {
        public HashSet<ClassDeclarationSyntax> Types = new HashSet<ClassDeclarationSyntax>();

        internal static ISyntaxContextReceiver Create()
        {
            return new MessageHandlerReceiver();
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

            if (!classTypeSymbol.HasAttribute(Definition.ResponseTypeAttribute))
            {
                return;
            }

            Types.Add(classDeclarationSyntax);
        }
    }
}