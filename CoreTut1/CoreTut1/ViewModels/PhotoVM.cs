using Common.Dtos;
using Data.Model;
using System.Collections.Generic;

namespace CoreTut1.ViewModels
{
    public class PhotoVM : BaseVM
    {
        public Photos Photo { get; set; }
        public IEnumerable<Photos> List { get; set; }
    }
}
