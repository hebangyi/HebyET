using System.Collections.Generic;

namespace ET.Server;

public class RoleItem
{
    // 角色数据库中的唯一ID
    public long RoleId { get; set; }
    // 等级
    public int Level { get; set; }
    // 昵称，方便登陆查询到
    public string NickName { get; set; }
}


public class AccountBaseInfo : MongoEntity
{
    public RoleItem roleItem;
    // 角色列表 (一个账号关联多个角色)
    // public List<RoleItem> RoleItems = new List<RoleItem>();
}

public class TestAccount : AccountBaseInfo
{
    public string Account;
}