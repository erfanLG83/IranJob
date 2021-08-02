using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IranJob.WebApi
{
    public class ImagePaths
    {
        private const string BasePath = "https://erfanlashgar.ir/files/images/";
        //public ImagePaths(IHostEnvironment env)
        //{
        //    _basePath = env.ContentRootPath;
        //}
        public const string CompaniesPath = BasePath + "companies/" ;
    }
}
