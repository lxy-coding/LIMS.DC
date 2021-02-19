using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Common.CommonMethod
{
    /// <summary>
    /// 获取枚举属性
    /// 作者: 周张智
    /// 日期: 2019/09/19
    /// </summary>
    public static class GetEnumAttribute
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="enumValue">枚举</param>
        /// <returns>string</returns>
        public static string  run(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
            { return value; }
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="enumValue">枚举</param>
        /// <returns>string</returns>
        public static string run(object enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
            { return value; }
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
    }
}
