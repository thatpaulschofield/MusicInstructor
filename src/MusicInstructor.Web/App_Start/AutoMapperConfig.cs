using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;

namespace MusicInstructor.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<RegistrationModel, Login>();
        }
    }
}