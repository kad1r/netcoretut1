using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Helpers
{
    public class EfHelper
    {
        public bool hasFlag<T>(DbSet<T> dbSet, string field) where T : class
        {
            var hasFlag = false;
            var genericTypeArguments = dbSet.GetType().GenericTypeArguments;

            if (genericTypeArguments.Any())
            {
                var fields = ((System.Reflection.TypeInfo)(dbSet.GetType().GenericTypeArguments.FirstOrDefault())).DeclaredFields;

                hasFlag = fields.Any(x => x.Name.Contains(field));
            }

            return hasFlag;
        }
    }
}
