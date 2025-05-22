import { FairyEditor, FairyGUI, System } from "csharp";
import CodeWriter from './CodeWriter';
import Utils from "./Utils";
import StringBuilder from './StringBuilder';


function genCode(handler: FairyEditor.PublishHandler) {
    let isMonoGame = handler.project.type == FairyEditor.ProjectType.MonoGame;
    let codePkgName = handler.ToFilename(handler.pkg.name);

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

    genPackage(handler);
    genComponent(handler);
    genComponentSystem(handler);

    // let packageTemplate = Utils.ReadTemplate("ETPackage.template", templatePath);
    // let packageContext = Utils.ReplaceAll(packageTemplate, "{thisPackageName}", "PKG_" + codePkgName);
    // packageContext = Utils.ReplaceAll(packageContext, "{namespace}", "ET.Client");
    // packageContext = Utils.ReplaceAll(packageContext, "{uiPkgName}", codePkgName);
    // packageContext = Utils.ReplaceAll(packageContext, "{urls}", "");

    // let packageSavePath = Utils.FormatStr("{0}/FUIPackage.cs", exportCodePath);
    // System.IO.File.WriteAllText(packageSavePath, packageContext);
}

function genPackage(handler: FairyEditor.PublishHandler){
    let codePkgName = handler.ToFilename(handler.pkg.name);
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    exportCodePath = Utils.ReplaceAll(exportCodePath,"HotfixView","ModelView")
    let templateFileName = "ETPackage.template";
    let codeTemplatePath = "/ETCodeGenerate/template/Unity";
    let templatePath = Utils.FormatStr("{0}{1}", FairyEditor.App.pluginManager.projectPluginFolder, codeTemplatePath);
    let template = Utils.ReadTemplate(templateFileName, templatePath);

    let classes = handler.CollectClasses(false, false, null);
    handler.SetupCodeFolder(exportCodePath, "cs"); //check if target folder exists, and delete old files

    let packageContent = new StringBuilder();
    let resContent = new StringBuilder();

    packageContent.Append("\t\t");
    packageContent.Append(Utils.FormatStr('public const string PKG_{0} = "{1}";', codePkgName, codePkgName));

    let classCnt = classes.Count;
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        resContent.Append("\t\t");
        resContent.Append(Utils.FormatStr('public const string RES_{0}_{1} = "{2}";', codePkgName, classInfo.resName, classInfo.resName));
        resContent.Append("\r\n");
    }


    let classContent = Utils.ReplaceAll(template, "{package_variable}", packageContent.ToString());
    classContent = Utils.ReplaceAll(classContent, "{res_variable}", resContent.ToString());

    let componentSavePath = Utils.FormatStr("{0}/{1}.cs", exportCodePath, "FGUIPackage");
    System.IO.File.WriteAllText(componentSavePath, classContent);
}

function genComponent(handler: FairyEditor.PublishHandler) {
    let codePkgName = handler.ToFilename(handler.pkg.name);
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    exportCodePath = Utils.ReplaceAll(exportCodePath,"HotfixView","ModelView")
    let templateFileName = "ETComponent.template";
    let codeTemplatePath = "/ETCodeGenerate/template/Unity";
    let templatePath = Utils.FormatStr("{0}{1}", FairyEditor.App.pluginManager.projectPluginFolder, codeTemplatePath);
    let template = Utils.ReadTemplate(templateFileName, templatePath);

    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(false, false, null);
    handler.SetupCodeFolder(exportCodePath, "cs"); //check if target folder exists, and delete old files

    let getMemberByName = settings.getMemberByName;
    // System.IO.File.Delete(exportCodePath)
    let classCnt = classes.Count;

    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let resUrl = Utils.FormatStr("ui://{0}/{1}", handler.pkg.name, classInfo.resName);

        let classContent = Utils.ReplaceAll(template, "{className}", classInfo.className);
        classContent = Utils.ReplaceAll(classContent, "{uiPkgName}", codePkgName);
        classContent = Utils.ReplaceAll(classContent, "{uiResName}", classInfo.resName);
        classContent = Utils.ReplaceAll(classContent, "{componentName}", classInfo.superClassName);
        classContent = Utils.ReplaceAll(classContent, "{uiResURL}", resUrl);

        let memberVarStr = new StringBuilder();
        let members = classInfo.members;
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            let [typeEnum, typeName] = ClassExportDefine(settings, memberInfo);
            let memberInfoType = typeName
            let memberInfoName = memberInfo.varName

            memberVarStr.Append("\t\t");
            memberVarStr.Append("public " + memberInfoType + " " + memberInfoName + ";");
            memberVarStr.Append("\r\n");
        }

        classContent = Utils.ReplaceAll(classContent, "{variable}", memberVarStr.ToString());
        // classContent = Utils.ReplaceAll(classContent, "{content}", memberContent.ToString());
        // classContent = Utils.ReplaceAll(classContent, "{dispose}", memberDispose.ToString());
    
        let componentSavePath = Utils.FormatStr("{0}/{1}.cs", exportCodePath, classInfo.className);
        System.IO.File.WriteAllText(componentSavePath, classContent);
    }
}

function genComponentSystem(handler: FairyEditor.PublishHandler) {
    let codePkgName = handler.ToFilename(handler.pkg.name);
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;

    let templateFileName = "ETComponentSystem.template";
    let codeTemplatePath = "/ETCodeGenerate/template/Unity";
    let templatePath = Utils.FormatStr("{0}{1}", FairyEditor.App.pluginManager.projectPluginFolder, codeTemplatePath);
    let classes = handler.CollectClasses(false, false, null);
    let classCnt = classes.Count;
    let getMemberByName = settings.getMemberByName;
    handler.SetupCodeFolder(exportCodePath, "cs");  //check if target folder exists, and delete old files
    let template = Utils.ReadTemplate(templateFileName, templatePath);
    // System.IO.File.Delete(exportCodePath)

    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let resUrl = Utils.FormatStr("ui://{0}/{1}", handler.pkg.name, classInfo.resName);
        let classContent = Utils.ReplaceAll(template, "{className}", classInfo.className);
        classContent = Utils.ReplaceAll(classContent, "{uiPkgName}", codePkgName);
        classContent = Utils.ReplaceAll(classContent, "{uiResName}", classInfo.resName);
        classContent = Utils.ReplaceAll(classContent, "{componentName}", classInfo.superClassName);
        classContent = Utils.ReplaceAll(classContent, "{uiResURL}", resUrl);

        let memberContent = new StringBuilder();
        let memberDispose = new StringBuilder();

        let members = classInfo.members;
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            let [typeEnum, typeName] = ClassExportDefine(settings, memberInfo);


            memberContent.Append("\t\t\t");
            //变量赋值
            if (memberInfo.group == 0) {
                if (getMemberByName)
                {
                    if (typeEnum == ExportClassType.Normal)
                    {
                        memberContent.Append(Utils.FormatStr('self.{0} = ({1})com.GetChild("{2}");',memberInfo.varName, memberInfo.type, memberInfo.name))
                    }
                    else
                    {
                        memberContent.Append(Utils.FormatStr('self.{0} = self.AddChild<{1},GObject>(com.GetChild("{2}"));',memberInfo.varName, typeName, memberInfo.name))
                    }
                }
                else
                {
                    if (typeEnum == ExportClassType.Normal)
                    {
                        memberContent.Append(Utils.FormatStr('self.{0} = ({1})com.GetChildAt({2});',memberInfo.varName, memberInfo.type, memberInfo.index.toString()))
                    }
                    else
                    {
                        memberContent.Append(Utils.FormatStr('self.{0} = self.AddChild<{1},GObject>(com.GetChildAt("{2}"));',memberInfo.varName, typeName, memberInfo.name))
                    }
                }
                    
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    memberContent.Append(Utils.FormatStr('self.{0} = com.GetController("{1}");',memberInfo.varName, memberInfo.name))
                else
                    memberContent.Append(Utils.FormatStr('self.{0} = com.GetControllerAt({1});',memberInfo.varName, memberInfo.index.toString()))
            }
            else {
                if (getMemberByName)
                    memberContent.Append(Utils.FormatStr('self.{0} = com.GetTransition("{1}");',memberInfo.varName, memberInfo.name))
                else
                    memberContent.Append(Utils.FormatStr('self.{0} = com.GetTransitionAt({1});',memberInfo.varName, memberInfo.index.toString()))
            }
            memberContent.Append("\r\n");

            // 变量清理
            if (memberInfo.res != null) {
                memberDispose.Append("\t\t\t");
                memberDispose.Append("self." + memberInfo.varName + "?.Dispose();");
                memberDispose.Append("\r\n");
            }

            memberDispose.Append("\t\t\t");
            memberDispose.Append("self." + memberInfo.varName + " = null;");
            memberDispose.Append("\r\n");
        }
        
        classContent = Utils.ReplaceAll(classContent, "{content}", memberContent.ToString());
        classContent = Utils.ReplaceAll(classContent, "{dispose}", memberDispose.ToString());

        let componentSavePath = Utils.FormatStr("{0}/{1}System.cs", exportCodePath, classInfo.className);
        System.IO.File.WriteAllText(componentSavePath, classContent);
    
    }
}


enum ExportClassType
{
    Normal = 0,
    SelfDefineClass = 1,
}

function ClassExportDefine(setting: FairyEditor.GlobalPublishSettings.CodeGenerationConfig, memberInfo: FairyEditor.PublishHandler.MemberInfo) : [ExportClassType, string]
{
    if(memberInfo.res != null && memberInfo.res.name != null)
    {
        console.log("=================")
        console.log(memberInfo.res.name)
        return [ExportClassType.SelfDefineClass, setting.classNamePrefix + memberInfo.res.name]
    }
    
    return [ExportClassType.Normal, memberInfo.type]
}

export { genCode };