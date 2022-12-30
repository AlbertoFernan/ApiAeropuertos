using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiAeropuertos.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly DbContext context;
        public Repository(DbContext context)
        {
            this.context = context;
        }
        public  DbSet<T> Get()
        {
            return context.Set<T>();

        }
        public T? GetById(object id)
        {
            return context.Find<T>(id);
        }

        public void Insert(T entidad)
        {
            context.Add(entidad);
            context.SaveChanges();
        }


        public void Delete(T entidad)
        {
            context.Remove(entidad);
            context.SaveChanges();
        }
        public async Task Update(T entidad)
        {
            context.Update(entidad);
            context.SaveChanges();
          
        }
    }
}
