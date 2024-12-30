using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.ApiLogicExceptionHandling
{
    public class CommonApiLogicException : Exception
    {
        public ApiLogicExceptionCode ExceptionCode { get; private set; }
        public string Message { get; set; }
        public CommonApiLogicException(ApiLogicExceptionCode code, string message = "")
        {
            ExceptionCode = code;
            Message = message;
        }

        public string GetExceptionCodeDescription()
        {
            Type type = ExceptionCode.GetType();
            MemberInfo[] memberInfo = type.GetMember(ExceptionCode.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return ExceptionCode.ToString();
        }
    }
}
