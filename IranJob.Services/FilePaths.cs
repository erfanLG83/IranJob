namespace IranJob.Services
{
    public class FilePaths
    {
        private const string BasePath = "https://erfanlashgar.ir/files/";

        private const string ImagesPath = "images/";
        //public ImagePaths(IHostEnvironment env)
        //{
        //    _basePath = env.ContentRootPath;
        //}
        public const string CompaniesPath = BasePath + ImagesPath + "companies/";
        public const string ResumeFilesPath = BasePath + "resumes/";
    }
}
