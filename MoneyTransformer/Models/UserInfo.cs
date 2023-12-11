using Microsoft.EntityFrameworkCore;

namespace MoneyTransformer.Models
{
    [Keyless]
    public class UserInfo
    {
       
        public Useraccount useraccount { get; set; }
        public Userlogin userlogin { get; set; }
    }
}
