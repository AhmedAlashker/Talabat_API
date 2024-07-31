using AdminDashboard.Models;
using AutoMapper;
using Talabat.Core.Entities.Product;

namespace AdminDashboard.Helper
{
    public class MappinProfiles : Profile
    {
        public MappinProfiles()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
