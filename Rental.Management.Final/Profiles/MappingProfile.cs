using AutoMapper;
using Rental.Management.Final.Controllers;
using Rental.Management.Final.Models;
using Rental.Management.Final.Views.RentalProperties;

namespace Rental.Management.Final.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RentalProperty, PropertyVm>();
            CreateMap<PropertyVm, RentalProperty>();
        }
    }
}
