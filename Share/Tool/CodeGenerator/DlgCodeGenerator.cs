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
            Console.WriteLine("开始检测生成 Component System");
            string rootPath = Path.GetFullPath("../");
            string TagTxt = File.ReadAllText(Path.Combine(rootPath, TagFile));
            
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
                    if (classDecl.AttributeLists.Count > 1)
                    {
                        Console.WriteLine(classDecl.AttributeLists);
                        GenerateFile(classDecl, TagTxt, rootPath);
                    }
                }
            }
            
            
            Console.WriteLine("开始检测生成 Dlg 完成");
        }

        private static void GenerateFile(ClassDeclarationSyntax classDecl, string tagTxt, string rootPath)
        {
            
        }
    }
}