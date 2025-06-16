using ET;

class Program
{
    static void Main(string[] args)
    {
        ApplicationContext.Instance.AddSingleton<TimeInfo>();
        ApplicationContext.Instance.AddSingleton<RSATokenManager>();
        
        AccountLoginRSA rsa = new ();
        rsa.RoleId = 123;
        var token = RSATokenManager.Instance.MakeToken(rsa);
        Console.WriteLine(token);
        token += "sxFvkAXgCTnRUWkGjnt1NSEyMfnD+GBvKMgeTWeHsvUXh8BwKRntiJqU1X4uxwGfw6P21v2LnkGem1ZxKNX/krCWDmQpQk5Q127uzlE8BhYhYCXGDzRkNhh9FDSuQ8RkTsGB1zsV/l6pi9BlzWsJALpCi2bzWFAN3mY2I6U+ums=";
        Console.WriteLine(RSATokenManager.Instance.VerifyToken<AccountLoginRSA>(token));
        // rsa.RoleId = testAccount.roleItem.RoleId;
    }
}