using System;
using System.Collections.Generic;
using System.IO;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class ComponentSystemGenerator: ISourceGenerator
{
    // static string FileLog = "C:\\Users\\56312\\Desktop\\a.txt";
    
    
    public void Initialize(GeneratorInitializationContext context)
    {
        
        context.RegisterForSyntaxNotifications(() => MessageHandlerReceiver.Create());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        throw new NotImplementedException();
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