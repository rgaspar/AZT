using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLL = Azure.Test.Project.Domain.Models.Core;
using DTO = Azure.TestProject.DataTransfer.Core;

namespace Azure.TestProject.Services.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapEmailAttribute();
        }

        private void MapEmailAttribute()
        {
            CreateMap<DTO.EmailAttribute, BLL.EmailAttribute>();
            CreateMap<BLL.EmailAttribute, DTO.EmailAttribute>();
        }
    }
}
