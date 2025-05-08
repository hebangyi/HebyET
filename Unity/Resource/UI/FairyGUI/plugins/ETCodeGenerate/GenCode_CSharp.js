"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.genCode = genCode;
const csharp_1 = require("csharp");
const Utils_1 = require("./Utils");
const StringBuilder_1 = require("./StringBuilder");
function genCode(handler) {
    let isMonoGame = handler.project.type == csharp_1.FairyEditor.ProjectType.MonoGame;
    let codePkgName = handler.ToFilename(handler.pkg.name);
    //不是字母和数字
    if (!/^\w*$/i.test(codePkgName)) {
        csharp_1.FairyEditor.App.Alert("包名必须仅由字母和数字组成", null, null);
        return;
    }
    //非大写字母开头
    if (!/^[A-Z]/.test(codePkgName)) {
        csharp_1.FairyEditor.App.Alert("包名必须以大写字母开头", null, null);
        return;
    }
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
function genComponent(handler) {
    let codePkgName = handler.ToFilename(handler.pkg.name);
    let settings = handler.project.GetSettings("Publish").codeGeneration;
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    exportCodePath = Utils_1.default.ReplaceAll(exportCodePath, "HotfixView", "ModelView");
    let templateFileName = "ETComponent.template";
    let codeTemplatePath = "/ETCodeGenerate/template/Unity";
    let templatePath = Utils_1.default.FormatStr("{0}{1}", csharp_1.FairyEditor.App.pluginManager.projectPluginFolder, codeTemplatePath);
    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(null, null, null);
    handler.SetupCodeFolder(exportCodePath, "cs"); //check if target folder exists, and delete old files
    let getMemberByName = settings.getMemberByName;
    let classCnt = classes.Count;
    for (let i = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let resUrl = Utils_1.default.FormatStr("ui://{0}/{1}", handler.pkg.name, classInfo.resName);
        let template = Utils_1.default.ReadTemplate(templateFileName, templatePath);
        let classContent = Utils_1.default.ReplaceAll(template, "{className}", classInfo.className);
        classContent = Utils_1.default.ReplaceAll(classContent, "{uiPkgName}", codePkgName);
        classContent = Utils_1.default.ReplaceAll(classContent, "{uiResName}", classInfo.resName);
        classContent = Utils_1.default.ReplaceAll(classContent, "{componentName}", classInfo.superClassName);
        classContent = Utils_1.default.ReplaceAll(classContent, "{uiResURL}", resUrl);
        let memberVarStr = new StringBuilder_1.default();
        let members = classInfo.members;
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            let memberInfoType = memberInfo.type;
            let memberInfoName = memberInfo.varName;
            memberVarStr.Append("\t\t");
            memberVarStr.Append("public " + memberInfoType + " " + memberInfoName + ";");
            memberVarStr.Append("\r\n");
        }
        classContent = Utils_1.default.ReplaceAll(classContent, "{variable}", memberVarStr.ToString());
        // classContent = Utils.ReplaceAll(classContent, "{content}", memberContent.ToString());
        // classContent = Utils.ReplaceAll(classContent, "{dispose}", memberDispose.ToString());
        let componentSavePath = Utils_1.default.FormatStr("{0}/{1}.cs", exportCodePath, classInfo.className);
        csharp_1.System.IO.File.WriteAllText(componentSavePath, classContent);
    }
}
function genComponentSystem(handler) {
    let codePkgName = handler.ToFilename(handler.pkg.name);
    let settings = handler.project.GetSettings("Publish").codeGeneration;
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    let templateFileName = "ETComponentSystem.template";
    let codeTemplatePath = "/ETCodeGenerate/template/Unity";
    let templatePath = Utils_1.default.FormatStr("{0}{1}", csharp_1.FairyEditor.App.pluginManager.projectPluginFolder, codeTemplatePath);
    let classes = handler.CollectClasses(null, null, null);
    let classCnt = classes.Count;
    let getMemberByName = settings.getMemberByName;
    for (let i = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let resUrl = Utils_1.default.FormatStr("ui://{0}/{1}", handler.pkg.name, classInfo.resName);
        let template = Utils_1.default.ReadTemplate(templateFileName, templatePath);
        let classContent = Utils_1.default.ReplaceAll(template, "{className}", classInfo.className);
        classContent = Utils_1.default.ReplaceAll(classContent, "{uiPkgName}", codePkgName);
        classContent = Utils_1.default.ReplaceAll(classContent, "{uiResName}", classInfo.resName);
        classContent = Utils_1.default.ReplaceAll(classContent, "{componentName}", classInfo.superClassName);
        classContent = Utils_1.default.ReplaceAll(classContent, "{uiResURL}", resUrl);
        let memberContent = new StringBuilder_1.default();
        let memberDispose = new StringBuilder_1.default();
        let members = classInfo.members;
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            memberContent.Append("\t\t\t");
            //变量赋值
            if (memberInfo.group == 0) {
                if (getMemberByName)
                    memberContent.Append(Utils_1.default.FormatStr('self.{0} = ({1})com.GetChild("{2}");', memberInfo.varName, memberInfo.type, memberInfo.name));
                else
                    memberContent.Append(Utils_1.default.FormatStr('self.{0} = ({1})com.GetChildAt({2});', memberInfo.varName, memberInfo.type, memberInfo.index.toString()));
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    memberContent.Append(Utils_1.default.FormatStr('self.{0} = com.GetController("{1}");', memberInfo.varName, memberInfo.name));
                else
                    memberContent.Append(Utils_1.default.FormatStr('self.{0} = com.GetControllerAt({1});', memberInfo.varName, memberInfo.index.toString()));
            }
            else {
                if (getMemberByName)
                    memberContent.Append(Utils_1.default.FormatStr('self.{0} = com.GetTransition("{1}");', memberInfo.varName, memberInfo.name));
                else
                    memberContent.Append(Utils_1.default.FormatStr('self.{0} = com.GetTransitionAt({1});', memberInfo.varName, memberInfo.index.toString()));
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
        classContent = Utils_1.default.ReplaceAll(classContent, "{content}", memberContent.ToString());
        classContent = Utils_1.default.ReplaceAll(classContent, "{dispose}", memberDispose.ToString());
        let componentSavePath = Utils_1.default.FormatStr("{0}/{1}System.cs", exportCodePath, classInfo.className);
        csharp_1.System.IO.File.WriteAllText(componentSavePath, classContent);
    }
}
