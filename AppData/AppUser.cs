using Microsoft.AspNetCore.Identity;

namespace StationShop.AppData
{
    public class AppUser: IdentityUser 
    {
        public String? FirstName { get; set; }
        public String? LastName { get; set; }


        //public AppUser()
        //{
        //    string s; nguuyen thuuy
        //    String abc;
            
        //}
    }
}
