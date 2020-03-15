using ERPDAL.ControlPanelDAL;
using ERPDAL.InventoryDAL;
using ERPDAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.InventoryDAL
{
    public class InventoryBaseRepository<T>: IBaseRepository<T> where T : class
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        internal DbSet<T> dbSet = null;
        public InventoryBaseRepository(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            if (inventoryUnitOfWork == null) throw new ArgumentNullException("DbContext is not assigned");
            this._inventoryUnitOfWork = inventoryUnitOfWork;
            dbSet = this._inventoryUnitOfWork.Db.Set<T>();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).AsEnumerable();
        }

        public T GetById(object Id)
        {
            return dbSet.Find(Id);
        }

        public T GetOneByOrg(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.FirstOrDefault(whereCondition);
        }

        public bool Save()
        {
            //throw new NotImplementedException();
            return _inventoryUnitOfWork.Db.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _inventoryUnitOfWork.Db.SaveChangesAsync() > 0;
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void InsertAll(IList<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            _inventoryUnitOfWork.Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void UpdateAll(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                _inventoryUnitOfWork.Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(object Id)
        {
            T entity = this.GetById(Id);
            dbSet.Remove(entity);
        }

        public void DeleteAll(Expression<Func<T, bool>> whereCondition)
        {
            IEnumerable<T> entities = this.GetAll(whereCondition);
            dbSet.RemoveRange(entities);
        }

        public void DeleteOneByOrg(Expression<Func<T, bool>> whereCondition)
        {
            T entity = this.GetOneByOrg(whereCondition);
            dbSet.Remove(entity);
        }

        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Any(whereCondition);
        }

        public int Count(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).Count();
        }

        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters);
        }

        public IEnumerable<dynamic> SqlQuery(string Sql, Dictionary<string, object> Parameters)
        {
            using (var cmd = _inventoryUnitOfWork.Db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }

        public IEnumerable<dynamic> SqlQuery(string Sql)
        {
            using (var cmd = _inventoryUnitOfWork.Db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }
        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
    }
}
