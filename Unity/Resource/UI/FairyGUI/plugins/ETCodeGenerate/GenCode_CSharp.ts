import { FairyEditor, FairyGUI, System } from "csharp";
import CodeWriter from './CodeWriter';
import Utils from "./Utils";
import StringBuilder from './StringBuilder';


function genCode(handler: FairyEditor.PublishHandler) {
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    let namespaceName = codePkgName;
    let isMonoGame = handler.project.type == FairyEditor.ProjectType.MonoGame;

    //不是字母和数字
    if (!/^\w*$/i.test(codePkgName)) {
        FairyEditor.App.Alert("包名必须仅由字母和数字组成", null, null);
        return;
    }

    //非大写字母开头
    if (!/^[A-Z]/.test(codePkgName)) {
        FairyEditor.App.Alert("包名必须以大写字母开头", null, null);
        return;
    }

    if (settings.packageName)
        namespaceName = settings.packageName + '.' + namespaceName;

    let templateFileName = "ETComponent.template";
    let codeTemplatePath = "/ETCodeGenerate/template/Unity";
    let templatePath = Utils.FormatStr("{0}{1}", FairyEditor.App.pluginManager.projectPluginFolder, codeTemplatePath);

    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, null);
    handler.SetupCodeFolder(exportCodePath, "cs"); //check if target folder exists, and delete old files

    let getMemberByName = settings.getMemberByName;

    let classCnt = classes.Count;
    console.log("class count : ", classCnt)

    let writer = new CodeWriter();
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let resUrl = Utils.FormatStr("ui://{0}/{1}", handler.pkg.name, classInfo.resName);
        let template = Utils.ReadTemplate(templateFileName, templatePath);
        let classContent = Utils.ReplaceAll(template, "{packageName}", namespaceName);
        classContent = Utils.ReplaceAll(classContent, "{className}", classInfo.className);
        classContent = Utils.ReplaceAll(classContent, "{uiPkgName}", codePkgName);
        classContent = Utils.ReplaceAll(classContent, "{uiResName}", classInfo.resName);
        classContent = Utils.ReplaceAll(classContent, "{componentName}", classInfo.superClassName);
        classContent = Utils.ReplaceAll(classContent, "{uiResURL}", resUrl);


        let memberVarStr = new StringBuilder();
        let memberContent = new StringBuilder();
        let memberDispose = new StringBuilder();


        let members = classInfo.members;
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            let memberInfoType = memberInfo.type
            let memberInfoName = memberInfo.varName

            memberVarStr.Append("public " + memberInfoType + " " + memberInfoName);
            memberVarStr.Append("\r\n");

            writer.reset();
            //变量赋值
            if (memberInfo.group == 0) {
                if (getMemberByName)
                    writer.writeln('%s = (%s)GetChild("%s");', memberInfo.varName, memberInfo.type, memberInfo.name);
                else
                    writer.writeln('%s = (%s)GetChildAt(%s);', memberInfo.varName, memberInfo.type, memberInfo.index);
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    writer.writeln('%s = GetController("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('%s = GetControllerAt(%s);', memberInfo.varName, memberInfo.index);
            }
            else {
                if (getMemberByName)
                    writer.writeln('%s = GetTransition("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('%s = GetTransitionAt(%s);', memberInfo.varName, memberInfo.index);
            }
            memberContent.Append(writer.toString());
            memberContent.Append("\r\n");

            // 变量清理
            if (memberInfo.res != null) {
                memberDispose.Append("\t\t\t");
                memberDispose.Append(memberInfo.name + "?.Dispose();");
                memberDispose.Append("\r\n");
            }

            memberDispose.Append("\t\t\t");
            memberDispose.Append(memberInfo.name + " = null;");
            memberDispose.Append("\r\n");
        }

        classContent = Utils.ReplaceAll(classContent, "{variable}", memberVarStr.ToString());
        classContent = Utils.ReplaceAll(classContent, "{content}", memberContent.ToString());
        classContent = Utils.ReplaceAll(classContent, "{dispose}", memberDispose.ToString());
    
        let savePath = Utils.FormatStr("{0}/{1}.cs", exportCodePath, classInfo.className);
        System.IO.File.WriteAllText(savePath, classContent);
    }


    let packageTemplate = Utils.ReadTemplate("ETPackage.template", templatePath);
    let packageContext = Utils.ReplaceAll(packageTemplate, "{thisPackageName}", "PKG_" + codePkgName);
    packageContext = Utils.ReplaceAll(packageContext, "{namespace}", "ET.Client");
    packageContext = Utils.ReplaceAll(packageContext, "{uiPkgName}", codePkgName);
    packageContext = Utils.ReplaceAll(packageContext, "{urls}", "");

    let packageSavePath = Utils.FormatStr("{0}/FUIPackage.cs", exportCodePath);
    System.IO.File.WriteAllText(packageSavePath, packageContext);
}

export { genCode };