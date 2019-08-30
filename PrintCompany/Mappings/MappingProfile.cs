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
                opt => opt.MapFrom(src => src.Supplier == null ? "" : src.Supplier.Name))
                .ForMember(dest => dest.EmbroideryCompletedQuantity,
                opt => opt.MapFrom(src => src.EmbroideryCompletedQuantity ?? 0))
                .ForMember(dest => dest.PrintCompletedQuantity,
                opt => opt.MapFrom(src => src.PrintCompletedQuantity ?? 0));
            CreateMap<OrderLineViewModel, OrderLine>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<OrderCustomerContact, OrderCustomerContactViewModel>();
            CreateMap<OrderCustomerContactViewModel, OrderCustomerContact>();
            CreateMap<OrderLine, OrderLine>()
                .ForMember(x => x.Id, opt => opt.Ignore());   /*Copying orderline*/
        }
    }
}
