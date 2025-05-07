import { FairyEditor, FairyGUI, System } from "csharp";

const MASK_LIST = "MaskList";

function FormatStr(...params: string[]): string {
  for (var a = params[0], b = 1; b < params.length; b++) a = a.replace(RegExp("\\{" + (b - 1) + "\\}", "ig"), params[b]);
  return a;
}

function IsEmptyStr(s: string) {
  return s === null || s === undefined || s === "";
}

//读取代码模板
function ReadTemplate(fileName: string, fileDir: string) {
  let filePath = System.IO.Path.Combine(fileDir, fileName);
  let content = System.IO.File.ReadAllText(filePath);
  return content;
}

function ReplaceAll(input: string, temp: string, dis: string): string {
  let reg = new RegExp(temp);
  let before = "";
  let max = 0;
  while (before !== input && max++ < 100) {
    before = input;
    input = input.replace(reg, dis);
  }

  return input;
}

function GetDisplayNodeByName(xml: FairyGUI.Utils.XML, name: string): FairyGUI.Utils.XML {
  var allNodes = xml.GetNode("displayList").Elements();
  for (let i = 0; i < allNodes.Count; i++) {
    var node = allNodes.get_Item(i);
    if (node.GetAttribute("name") === name) {
      return node;
    }
  }

  return null;
}

function GetTextVars(classXML: FairyGUI.Utils.XML, name: string): Array<string> {
  var node = GetDisplayNodeByName(classXML, name);
  if (node === null || !node.GetAttributeBool("vars")) {
    return [];
  }

  var txt = node.GetAttribute("text");
  if (IsEmptyStr(txt)) return [];

  var arr: Array<string> = [];
  for (let m of txt.matchAll(/\{\w+\=.*?\}/g)) {
    var keyName = m.toString().replace("{", "").replace("}", "").split("=")[0].trim();
    arr.push(keyName);
  }
  return arr;
}

//检查是否是遮罩列表
function IsMaskList(classXML: FairyGUI.Utils.XML, name: string): boolean {
  var node = GetDisplayNodeByName(classXML, name);
  if (node === null) {
    return false;
  }

  var attVal = node.GetAttribute("customData");
  if (!attVal) return false;

  var jObj = readJsonData(attVal);
  if (jObj[MASK_LIST]) return true;
  else return false;
}

function readJsonData(jsonStr: string): object {
  let data = jsonStr + "";
  let jObj = {};
  try {
    jObj = JSON.parse(data);
  } catch (error) {}

  return jObj;
}

function GetClassResXML(classInfo: FairyEditor.PublishHandler.ClassInfo): FairyGUI.Utils.XML {
  var xmlStr = System.IO.File.ReadAllText(classInfo.res.file);
  var xml = new FairyGUI.Utils.XML(xmlStr);
  return xml;
}

const CheckResultCache = {};

//检查组件是否可导出
function IsExportableItem(path: string): boolean {
  if (CheckResultCache[path] !== undefined) {
    return CheckResultCache[path];
  }

  var xmlStr = System.IO.File.ReadAllText(path);
  var xml = new FairyGUI.Utils.XML(xmlStr);

  var allNodes = xml.Elements();
  for (let i = 0; i < allNodes.Count; i++) {
    var node = allNodes.get_Item(i);
    if (node.name === "controller") {
      var cName = node.GetAttribute("name");
      if (cName !== "button" && cName !== "grayed" && cName !== "checked" && cName !== "expanded" && cName !== "leaf") {
        CheckResultCache[path] = true;
        return true;
      }
    } else if (node.name === "displayList") {
      var allDisplayNodes = node.Elements();
      for (let i = 0; i < allDisplayNodes.Count; i++) {
        var disNode = allDisplayNodes.get_Item(i);
        var cName = disNode.GetAttribute("name");
        if (cName !== "title" && cName !== "icon" && !cName.startsWith("n")) {
          CheckResultCache[path] = true;
          return true;
        }
      }
    } else if (node.name === "transition") {
      CheckResultCache[path] = true;
      return true;
    }
  }

  CheckResultCache[path] = false;
  return false;
}

function CheckLoaderUsage(xml: FairyGUI.Utils.XML, comName: string): boolean {
  var allNodes = xml.GetNode("displayList").Elements();
  for (let i = 0; i < allNodes.Count; i++) {
    var node = allNodes.get_Item(i);
    if (node.name === "loader") {
      var nodeName = node.GetAttribute("name");
      if (nodeName.startsWith("n")) {
        FairyEditor.App.Alert(`发布失败！${comName}->Loader【${nodeName}】如果代码不引用，请使用静态图片`);
        return false;
      }
    }
  }

  return true;
}

function isValidString(str: string): boolean {
  const regex = /^[a-zA-Z0-9]+$/;
  return regex.test(str);
}


export default {
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
