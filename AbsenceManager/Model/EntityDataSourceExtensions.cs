using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace AbsenceManager
{
    public static class EntityDataSourceExtensions
    {
        public static TEntity GetEntityAs<TEntity>(this object dataItem)
            where TEntity : class
        {
            var entity = dataItem as TEntity;

            if (entity != null)
                return entity;

            var td = dataItem as ICustomTypeDescriptor;

            if (td != null)
                return (TEntity)td.GetPropertyOwner(null);

            return null;
        }

        public static Object GetEntity(this object dataItem)
        {
            var td = dataItem as ICustomTypeDescriptor;

            if (td != null)
                return td.GetPropertyOwner(null);

            // Com EnableFlatenning a true na DataSource
            // na Row está a entidade e não o wrapper
            // Fix para esse caso
            if (dataItem != null)
                if (dataItem.GetType().IsClass)
                    return dataItem;

            return null;
        }
    }
}