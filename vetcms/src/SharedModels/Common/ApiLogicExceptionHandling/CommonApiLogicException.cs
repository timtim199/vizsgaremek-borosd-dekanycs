using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.ApiLogicExceptionHandling
{
    /// <summary>
    /// Közös API logikai kivétel osztály.
    /// </summary>
    public class CommonApiLogicException : Exception
    {
        /// <summary>
        /// Az API logikai kivétel kódja.
        /// </summary>
        public ApiLogicExceptionCode ExceptionCode { get; private set; }

        /// <summary>
        /// Az API logikai kivétel üzenete.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Létrehoz egy új CommonApiLogicException példányt a megadott kóddal és üzenettel.
        /// </summary>
        /// <param name="code">Az API logikai kivétel kódja.</param>
        /// <param name="message">Az API logikai kivétel üzenete.</param>
        public CommonApiLogicException(ApiLogicExceptionCode code, string message = "")
        {
            ExceptionCode = code;
            Message = message;
        }

        /// <summary>
        /// Visszaadja az API logikai kivétel kódjának leírását.
        /// </summary>
        /// <returns>Az API logikai kivétel kódjának leírása.</returns>
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
