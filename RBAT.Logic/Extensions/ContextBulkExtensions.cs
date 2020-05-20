using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using RBAT.Core;
using RBAT.Core.Clasess;

namespace RBAT.Logic.Extensions {

    public static class ContextBulkExtensions {

        public static Task BulkInsertAsyncExtended<T>( this RBATContext context, IList<T> entities, string userName ) where T : class {
            foreach ( var entity in entities ) {
                var baseEntity = entity as BaseEntity;
                if ( baseEntity != null ) {
                    baseEntity.Created = DateTime.UtcNow;
                    baseEntity.Modified = DateTime.UtcNow;
                    baseEntity.UserName = userName;
                }
            }

            return context.BulkInsertAsync( entities );
        }

        public static Task BulkUpdateAsyncExtended<T>( this RBATContext context, IList<T> entities, string userName ) where T : class {
            foreach ( var entity in entities ) {
                var baseEntity = entity as BaseEntity;
                if ( baseEntity != null ) {
                    baseEntity.Modified = DateTime.UtcNow;
                    baseEntity.UserName = userName;
                }
            }

            return context.BulkUpdateAsync( entities );
        }

    }

}
