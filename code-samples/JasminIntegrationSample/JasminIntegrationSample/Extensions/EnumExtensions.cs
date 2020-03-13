using System;
using System.ComponentModel;
using System.Linq;

namespace Jasmin.IntegrationSample.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// This extension method is broken out so you can use a similar pattern with 
        /// other MetaData elements in the future. This is your base method for each.
        /// </summary>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }

        /// <summary>
        /// This method creates a specific call to the above method, requesting the
        /// Description MetaData attribute.
        /// </summary>
        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Returns the maximum int value of the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>The enum maximum int value.</returns>
        public static int Max(this Enum enumType)
        {
            return Enum.GetValues(enumType.GetType()).Cast<int>().Max();
        }
    }
}
