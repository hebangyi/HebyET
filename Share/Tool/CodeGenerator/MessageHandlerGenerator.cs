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
            var searchFolder = Path.Combine(rootPath, "Unity\\Assets\\Scripts\\Model");
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
                            GenerateFile(rootPath, handlerText, classDecl);
                        }
                    }
                }
            }

            Console.WriteLine("检测生成 Handler 完成");
        }

        private static void GenerateFile(string rootPath, string handlerText, ClassDeclarationSyntax classDecl)
        {
            string className = classDecl.Identifier.Text;
            if (handlerText.Contains(className))
            {
                return;
            }
            
            File.AppendAllText(Path.Combine(rootPath, HandlerFile), $"{className}\n");

            string namespaceName = "ET";
            if (classDecl.Parent is NamespaceDeclarationSyntax namespaceDecl)
            {
                namespaceName = namespaceDecl.Name.ToString();
            }

            string path = "../DotNet/Hotfix/Server/Game/MessageHandler";
            string fileName = $"{className}Handler.cs";
            var filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string template = SessionHandlerTemplate;
            var code = template.Replace("{namespaceName}", namespaceName);
            code = code.Replace("{className}", className);
            File.WriteAllText(filePath, code);
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
    }
}