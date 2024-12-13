using System.Text.RegularExpressions;

namespace ET.Server;

public static class AccountHelper
{
    public static bool CheckNormalAccountValidate(string account)
    {
        string pattern = @"^[a-zA-Z0-9_]{2,15}$";
        return Regex.IsMatch(account, pattern);    
    }
}