"use strict";
//FYI: https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md
Object.defineProperty(exports, "__esModule", { value: true });
exports.onPublish = onPublish;
exports.onDestroy = onDestroy;
const GenCode_CSharp_1 = require("./GenCode_CSharp");
function onPublish(handler) {
    if (!handler.genCode)
        return;
    handler.genCode = false; //prevent default output
    console.log('Handling gen code in plugin');
    (0, GenCode_CSharp_1.genCode)(handler); //do it myself
}
function onDestroy() {
    //do cleanup here
}
