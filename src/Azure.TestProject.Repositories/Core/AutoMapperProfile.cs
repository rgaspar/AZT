using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLL = Azure.Test.Project.Domain.Models.Core;
using DAL = Azure.TestProject.Data.Models.Core;

namespace Azure.TestProject.Repositories.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapEmailAttribute();
        }

        private void MapEmailAttribute()
        {
            CreateMap<DAL.EmailAttribute, BLL.EmailAttribute>();
            CreateMap<BLL.EmailAttribute, DAL.EmailAttribute>();
        }
    }
}
