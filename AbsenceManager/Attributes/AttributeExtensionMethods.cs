using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel;

namespace AbsenceManager
{
    public static class AttributeExtensionMethods
    {
        /// <summary>
        /// Gets the attribute or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public static T GetAttributeOrDefault<T>(this AttributeCollection attributes) where T : Attribute, new()
        {
            return attributes.OfType<T>().DefaultIfEmpty(new T()).FirstOrDefault();
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this AttributeCollection attributes) where T : Attribute
        {
            return attributes.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Get the attribute or a default instance of the attribute
        /// if the Table attribute do not contain the attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="table">
        /// Table to search for the attribute on.
        /// </param>
        /// <returns>
        /// The found attribute or a default 
        /// instance of the attribute of type T
        /// </returns>
        public static T GetAttributeOrDefault<T>(this MetaTable table) where T : Attribute, new()
        {
            return table.Attributes.OfType<T>().DefaultIfEmpty(new T()).FirstOrDefault();
        }

        /// <summary>
        /// Get the attribute of type T or null if not found
        /// </summary>
        /// <typeparam name="T">
        /// Attribute type
        /// </typeparam>
        /// <param name="table">
        /// Table to search for the attribute on.
        /// </param>
        /// <returns>
        /// Returns the attribute T or null
        /// </returns>
        public static T GetAttribute<T>(this MetaTable table) where T : Attribute
        {
            return table.Attributes.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Get the attribute or a default instance of the attribute
        /// if the Column attribute do not contain the attribute
        /// </summary>
        /// <typeparam name="T">
        /// Attribute type
        /// </typeparam>
        /// <param name="table">
        /// Column to search for the attribute on.
        /// </param>
        /// <returns>
        /// The found attribute or a default 
        /// instance of the attribute of type T
        /// </returns>
        public static T GetAttributeOrDefault<T>(this MetaColumn column) where T : Attribute, new()
        {
            return column.Attributes.OfType<T>().DefaultIfEmpty(new T()).FirstOrDefault();
        }

        /// <summary>
        /// Get the attribute of type T or null if not found
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="table">Column to search for the attribute on.</param>
        /// <returns>Returns the attribute T or null</returns>
        public static T GetAttribute<T>(this MetaColumn column) where T : Attribute
        {
            return column.Attributes.OfType<T>().FirstOrDefault();
        }
    }
}