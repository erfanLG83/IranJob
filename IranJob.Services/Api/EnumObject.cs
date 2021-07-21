using System;
using System.Linq;

namespace IranJob.Services.Api
{
    public class EnumObject
    {
        public int Code { get; set; }
        public string Text { get; set; }


        public static implicit operator EnumObject(Enum e)
        {
            return new EnumObject
            {
                Code = e.GetInt16Value(),
                Text = e.ToDisplay().First()
            };
        }
    }
}
