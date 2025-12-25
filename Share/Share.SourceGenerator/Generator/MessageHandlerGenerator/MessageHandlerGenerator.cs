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
                                                       [ComponentOf]
                                                       [EnableMethod]
                                                       [FGUIDLG(WindowID.{className}, typeof({className}))]
                                                       public class Dlg{className} : Entity,IAwake
                                                       {
                                                           public {className} View { get => this.GetComponent<{className}>(); }
                                                           
                                                       }
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


            string path = "../DotNet/Hotfix/Server/Game/MessageHandler";
            string fileName = $"{className}Handler.cs";
            var filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filePath))
            {
                return;
            }

        
            if (classTypeSymbol.HasInterface(Definition.ISessionRequest))
            {
                string template = SessionHandlerTemplate;
                var code = template.Replace("{namespaceName}", namespaceName);
                code = code.Replace("{className}", className);
                File.WriteAllText(filePath, code);
            }
        }
        catch (Exception e)
        {
            File.AppendAllText("C:\\Users\\Administrator\\Desktop\\abc.txt", $"{e} \n");
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

