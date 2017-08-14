using System;
namespace MVVMHelpersDemo.Models
{
    public class AuthenticatedUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
    }
}
