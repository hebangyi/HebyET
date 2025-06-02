using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class DlgGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => DlgSyntaxContextReceiver.Create());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not DlgSyntaxContextReceiver receiver || receiver.ViewTypes.Count == 0)
        {
            return;
        }

        foreach (var type in receiver.ViewTypes)
        {
            this.GenerateCSFiles(type, context);
        }
    }

    private void GenerateCSFiles(ClassDeclarationSyntax classDeclarationSyntax, GeneratorExecutionContext context)
    {
        string className = classDeclarationSyntax.Identifier.Text;

        SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
        INamedTypeSymbol? classTypeSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
        INamespaceSymbol? namespaceSymbol = classTypeSymbol?.ContainingNamespace;
        string? namespaceName = namespaceSymbol?.Name;
        while (namespaceSymbol?.ContainingNamespace != null)
        {
            namespaceSymbol = namespaceSymbol.ContainingNamespace;
            if (string.IsNullOrEmpty(namespaceSymbol.Name))
            {
                break;
            }

            namespaceName = $"{namespaceSymbol.Name}.{namespaceName}";
        }

        if (namespaceName == null)
        {
            throw new Exception($"{className} namespace is null");
        }

        this.GenerateDlgCodeByTemplate(namespaceName, className, context);
        this.GenerateDlgSystemByTemplate(namespaceName, className, context);
        this.GenerateDlgEventByTemplate(namespaceName, className, context);
    }

    private void GenerateDlgCodeByTemplate(string namespaceName, string className,
    GeneratorExecutionContext context)
    {
        string path = "../Assets/Scripts/ModelView/Client/Game/UI/FGUI/Dlg";
        string fileName = $"Dlg{className}.cs";
        var filePath = Path.Combine(path, fileName);
        if (File.Exists(filePath))
        {
            return;
        }
        
        var code = DlgTemplate.Replace("{namespaceName}", namespaceName);
        code = code.Replace("{className}", className);
        File.WriteAllText(filePath, code);
    }

    private void GenerateDlgEventByTemplate(string namespaceName, string className,
    GeneratorExecutionContext context)
    {
        string path = "../Assets/Scripts/HotfixView/Client/Game/UI/FGUI/DlgEventHandler";
        string fileName = $"Dlg{className}EventHandler.cs";
        var filePath = Path.Combine(path, fileName);
        
        if (File.Exists(filePath))
        {
            return;
        }
        
        var code = DlgSystemTemplate.Replace("{namespaceName}", namespaceName);
        code = code.Replace("{className}", className);
        
        File.WriteAllText(filePath, code);
    }

    private void GenerateDlgSystemByTemplate(string namespaceName, string className,
    GeneratorExecutionContext context)
    {
        string path = "../Assets/Scripts/HotfixView/Client/Game/UI/FGUI/DlgSystem";
        string fileName = $"Dlg{className}System.cs";
        var filePath = Path.Combine(path, fileName);
        if (File.Exists(filePath))
        {
            return;
        }
        
        var code = DlgEventHandlerTemplate.Replace("{namespaceName}", namespaceName);
        code = code.Replace("{className}", className);
        File.WriteAllText(filePath, code);
    }
    
    public const string DlgTemplate = $$"""
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

    public const string DlgSystemTemplate = $$"""
                                              using System;
                                              using FairyGUI;

                                              namespace {namespaceName}
                                              {
                                                  public static class Dlg{className}System
                                                  {
                                                      public static void Init(this Dlg{className} self)
                                                      {
                                                      }
                                                      
                                                      public static void RegisterUIEvent(this Dlg{className} self)
                                                      {
                                                      }
                                                      
                                                      public static void ShowWindow(this Dlg{className} self, ShowWindowData showWindowData = null)
                                                      {
                                                      }
                                                      
                                                      public static void HideWindow(this Dlg{className} self)
                                                      {
                                                      }
                                                      
                                                      public static void BeforeUnload(this Dlg{className} self)
                                                      {
                                                      }
                                                  }
                                              }
                                              """;

    public const string DlgEventHandlerTemplate = $$"""
                                                    using FairyGUI;
                                                    
                                                    namespace {namespaceName}
                                                    {
                                                        [FGUIEvent(typeof({className}))]
                                                        public class Dlg{className}EventHandler : IFGUIEventHandler
                                                        {
                                                            public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
                                                            {
                                                                uiBaseWindow.WindowType = UIWindowType.Normal;
                                                            }
                                                    
                                                            public void OnInitComponent(UIBaseWindow uiBaseWindow)
                                                            {
                                                                uiBaseWindow.AddComponent<Dlg{className}>().AddComponent<{className}, GObject>(uiBaseWindow.GObject);
                                                                uiBaseWindow.GetComponent<Dlg{className}>().Init();
                                                            }
                                                    
                                                            public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
                                                            {
                                                                uiBaseWindow.GetComponent<Dlg{className}>().RegisterUIEvent();
                                                            }
                                                    
                                                            public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
                                                            {
                                                                uiBaseWindow.GetComponent<Dlg{className}>().ShowWindow(showWindowData);
                                                            }
                                                    
                                                            public void OnHideWindow(UIBaseWindow uiBaseWindow)
                                                            {
                                                                uiBaseWindow.GetComponent<Dlg{className}>().HideWindow();
                                                            }
                                                    
                                                            public void BeforeUnload(UIBaseWindow uiBaseWindow)
                                                            {
                                                                uiBaseWindow.GetComponent<Dlg{className}>().BeforeUnload();
                                                            }
                                                        }
                                                    }
                                                    """;
    
    
    class DlgSyntaxContextReceiver : ISyntaxContextReceiver
    {
        public HashSet<ClassDeclarationSyntax> ViewTypes = new HashSet<ClassDeclarationSyntax>();

        internal static ISyntaxContextReceiver Create()
        {
            return new DlgSyntaxContextReceiver();
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

            if (!classTypeSymbol.HasAttribute(Definition.FGUITagAttribute))
            {
                return;
            }

            if (!classTypeSymbol.Name.EndsWith(Definition.ViewEndFix))
            {
                return;
            }

            ViewTypes.Add(classDeclarationSyntax);
        }
    }
}