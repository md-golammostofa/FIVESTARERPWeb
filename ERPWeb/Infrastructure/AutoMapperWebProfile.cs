using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DTOModels;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPWeb.Infrastructure
{
    public class AutoMapperWebProfile:AutoMapper.Profile
    {
        public static void Run() {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperWebProfile>();
            });
        }
        public AutoMapperWebProfile() {
            CreateMap<WarehouseDTO, WarehouseViewModel>();
            CreateMap<WarehouseViewModel, WarehouseDTO>();
        }
    }
}