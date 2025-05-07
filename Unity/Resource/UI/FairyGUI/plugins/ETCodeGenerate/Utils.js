"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const csharp_1 = require("csharp");
const MASK_LIST = "MaskList";
function FormatStr(...params) {
    for (var a = params[0], b = 1; b < params.length; b++)
        a = a.replace(RegExp("\\{" + (b - 1) + "\\}", "ig"), params[b]);
    return a;
}
function IsEmptyStr(s) {
    return s === null || s === undefined || s === "";
}
//读取代码模板
function ReadTemplate(fileName, fileDir) {
    let filePath = csharp_1.System.IO.Path.Combine(fileDir, fileName);
    let content = csharp_1.System.IO.File.ReadAllText(filePath);
    return content;
}
function ReplaceAll(input, temp, dis) {
    let reg = new RegExp(temp);
    let before = "";
    let max = 0;
    while (before !== input && max++ < 100) {
        before = input;
        input = input.replace(reg, dis);
    }
    return input;
}
function GetDisplayNodeByName(xml, name) {
    var allNodes = xml.GetNode("displayList").Elements();
    for (let i = 0; i < allNodes.Count; i++) {
        var node = allNodes.get_Item(i);
        if (node.GetAttribute("name") === name) {
            return node;
        }
    }
    return null;
}
function GetTextVars(classXML, name) {
    var node = GetDisplayNodeByName(classXML, name);
    if (node === null || !node.GetAttributeBool("vars")) {
        return [];
    }
    var txt = node.GetAttribute("text");
    if (IsEmptyStr(txt))
        return [];
    var arr = [];
    for (let m of txt.matchAll(/\{\w+\=.*?\}/g)) {
        var keyName = m.toString().replace("{", "").replace("}", "").split("=")[0].trim();
        arr.push(keyName);
    }
    return arr;
}
//检查是否是遮罩列表
function IsMaskList(classXML, name) {
    var node = GetDisplayNodeByName(classXML, name);
    if (node === null) {
        return false;
    }
    var attVal = node.GetAttribute("customData");
    if (!attVal)
        return false;
    var jObj = readJsonData(attVal);
    if (jObj[MASK_LIST])
        return true;
    else
        return false;
}
function readJsonData(jsonStr) {
    let data = jsonStr + "";
    let jObj = {};
    try {
        jObj = JSON.parse(data);
    }
    catch (error) { }
    return jObj;
}
function GetClassResXML(classInfo) {
    var xmlStr = csharp_1.System.IO.File.ReadAllText(classInfo.res.file);
    var xml = new csharp_1.FairyGUI.Utils.XML(xmlStr);
    return xml;
}
const CheckResultCache = {};
//检查组件是否可导出
function IsExportableItem(path) {
    if (CheckResultCache[path] !== undefined) {
        return CheckResultCache[path];
    }
    var xmlStr = csharp_1.System.IO.File.ReadAllText(path);
    var xml = new csharp_1.FairyGUI.Utils.XML(xmlStr);
    var allNodes = xml.Elements();
    for (let i = 0; i < allNodes.Count; i++) {
        var node = allNodes.get_Item(i);
        if (node.name === "controller") {
            var cName = node.GetAttribute("name");
            if (cName !== "button" && cName !== "grayed" && cName !== "checked" && cName !== "expanded" && cName !== "leaf") {
                CheckResultCache[path] = true;
                return true;
            }
        }
        else if (node.name === "displayList") {
            var allDisplayNodes = node.Elements();
            for (let i = 0; i < allDisplayNodes.Count; i++) {
                var disNode = allDisplayNodes.get_Item(i);
                var cName = disNode.GetAttribute("name");
                if (cName !== "title" && cName !== "icon" && !cName.startsWith("n")) {
                    CheckResultCache[path] = true;
                    return true;
                }
            }
        }
        else if (node.name === "transition") {
            CheckResultCache[path] = true;
            return true;
        }
    }
    CheckResultCache[path] = false;
    return false;
}
function CheckLoaderUsage(xml, comName) {
    var allNodes = xml.GetNode("displayList").Elements();
    for (let i = 0; i < allNodes.Count; i++) {
        var node = allNodes.get_Item(i);
        if (node.name === "loader") {
            var nodeName = node.GetAttribute("name");
            if (nodeName.startsWith("n")) {
                csharp_1.FairyEditor.App.Alert(`发布失败！${comName}->Loader【${nodeName}】如果代码不引用，请使用静态图片`);
                return false;
            }
        }
    }
    return true;
}
function isValidString(str) {
    const regex = /^[a-zA-Z0-9]+$/;
    return regex.test(str);
}
exports.default = {
    FormatStr,
    IsEmptyStr,
    ReadTemplate,
    ReplaceAll,
    GetDisplayNodeByName,
    GetTextVars,
    GetClassResXML,
    IsExportableItem,
    CheckLoaderUsage,
    IsMaskList,
    isValidString,
};
