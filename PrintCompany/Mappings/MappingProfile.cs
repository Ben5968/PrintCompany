using AutoMapper;
using PrintCompany.Core;
using PrintCompany.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintCompany.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();
            CreateMap<OrderLine, OrderLineViewModel>()
                .ForMember(dest => dest.SupplierName, 
                opt => opt.MapFrom(src => src.Supplier == null ? "" : src.Supplier.Name));
            CreateMap<OrderLineViewModel, OrderLine>();
        }
    }
}
