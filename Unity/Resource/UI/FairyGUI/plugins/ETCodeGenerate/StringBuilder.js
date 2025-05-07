"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class StringBuilder {
    strings = [];
    // 向 StringBuilder 中追加字符串
    Append(str) {
        this.strings.push(str);
    }
    // 将 StringBuilder 中的所有字符串拼接成一个字符串
    ToString() {
        return this.strings.join('');
    }
}
exports.default = StringBuilder;
