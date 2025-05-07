class StringBuilder {
    private strings: string[] = [];

    // 向 StringBuilder 中追加字符串
    Append(str: string): void {
        this.strings.push(str);
    }

    // 将 StringBuilder 中的所有字符串拼接成一个字符串
    ToString(): string {
        return this.strings.join('');
    }
}

export default StringBuilder;