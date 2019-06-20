using Common.Dtos;
using Data.Model;
using System.Collections.Generic;

namespace CoreTut1.ViewModels
{
    public class UserVM : BaseVM
    {
        public AspNetUsers AspNetUser { get; set; }
        public IEnumerable<AspNetUsers> List { get; set; }
    }
}
