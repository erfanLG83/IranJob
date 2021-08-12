using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace IranJob.Services.AutoMapper
{
    //public class Parent
    //{
    //    public int Id { get; set; }
    //    public string CategoryName { get; set; }
    //}
    //public class Child
    //{
    //    public int Id { get; set; }
    //    public string CategoryName { get; set; }
    //    public Parent Parent { get; set; }
    //}
    //public class ChildViewModel
    //{
    //    public int Id { get; set; }
    //    public string CategoryName { get; set; }
    //    public int ParentId { get; set; }
    //    public string ParentCategoryName { get; set; }
    //}


    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            //CreateMap<Child, ChildViewModel>();
        }
    }
}
