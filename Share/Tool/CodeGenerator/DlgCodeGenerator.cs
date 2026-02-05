using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET
{
    public static class DlgCodeGenerator
    {
        public const string TagFile = "Dlg.txt";

        public static void ExportDlg()
        {
            Console.WriteLine("开始检测生成 Dlg");
            string rootPath = Path.GetFullPath("../");
            
            string tagTxt = File.ReadAllText(Path.Combine(rootPath, TagFile));
            string searchPath = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\ModelView\\Client");

            var allFiles = Directory.GetFiles(searchPath, "*.cs", SearchOption.AllDirectories);

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
                        if (attribute.ToString().Contains("FGUITag") && classDecl.Identifier.Text.EndsWith("View"))
                        {
                            GenerateFile(rootPath, tagTxt, classDecl);
                        }
                    }
                }
            }

            Console.WriteLine("开始检测生成 Dlg 完成");
        }

        private static void GenerateFile(string rootPath, string tagTxt, ClassDeclarationSyntax classDeclarationSyntax)
        {
            string className = classDeclarationSyntax.Identifier.Text;
            string nameSpaceName = "ET";

            if (classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDecl)
            {
                nameSpaceName = namespaceDecl.Name.ToString();
            }
            
            if (tagTxt.Contains(className))
            {
                return;
            }
            
            File.AppendAllText(Path.Combine(rootPath, TagFile), $"{className}\n");
            
            Console.WriteLine($"开始生成类 {className}");
            
            GenerateDlgCodeByTemplate(rootPath, nameSpaceName, className);
            GenerateDlgSystemByTemplate(rootPath, nameSpaceName, className);
            GenerateDlgEventByTemplate(rootPath, nameSpaceName, className);
            
            
        }
        
        private static void GenerateDlgCodeByTemplate(string rootPath, string namespaceName, string className)
        {
            string path = Path.Combine(rootPath, "Unity/Assets/Scripts/ModelView/Client/Game/UI/FGUI/Dlg");
            string fileName = $"Dlg{className}.cs";
            var filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filePath))
            {
                return;
            }

            var code = DlgTemplate.Replace("{namespaceName}", namespaceName);
            code = code.Replace("{className}", className);
            File.WriteAllText(filePath, code);
        }

        private static void GenerateDlgEventByTemplate(string rootPath, string namespaceName, string className)
        {
            string path = Path.Combine(rootPath, "Unity/Assets/Scripts/HotfixView/Client/Game/UI/FGUI/DlgEventHandler");
            Directory.CreateDirectory(path);

            string fileName = $"Dlg{className}EventHandler.cs";
            var filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filePath))
            {
                return;
            }

            var code = DlgEventHandlerTemplate.Replace("{namespaceName}", namespaceName);
            code = code.Replace("{className}", className);
            File.WriteAllText(filePath, code);
        }

        private static void GenerateDlgSystemByTemplate(string rootPath, string namespaceName, string className)
        {
            string path = Path.Combine(rootPath, "Unity/Assets/Scripts/HotfixView/Client/Game/UI/FGUI/DlgSystem");
            Directory.CreateDirectory(path);
            string fileName = $"Dlg{className}System.cs";
            var filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filePath))
            {
                return;
            }

            var code = DlgSystemTemplate.Replace("{namespaceName}", namespaceName);
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
                                                    public static Dlg{className} Instance { get; set; }
                                                
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
                                                                    var dlgComponent = uiBaseWindow.AddComponent<Dlg{className}>();
                                                                    Dlg{className}.Instance = dlgComponent;
                                                                    dlgComponent.AddComponent<{className}, GObject>(uiBaseWindow.GObject);
                                                                    dlgComponent.Init();
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
    }
}