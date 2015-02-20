using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Katchulo.PowerDapper.Infra.Repository;
using System.Configuration;

namespace Katchulo.PowerDapper.Infra.DapperRepository
{
    public class DapperTable<T> : IDisposable, IRepository<T> where T : class
    {
        private DapperMapper _dapperMapper = new DapperMapper();
        private string _tableName;
        public String ConnectionString { get; set; }

        public DapperContext Context { get; private set; }

        public DapperTable(String connectionString, String tableName, DapperContext context)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                this.ConnectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            }
            else
            {
                this.ConnectionString = connectionString;
            }
            _tableName = tableName;
            Context = context;
        }
        public DapperTable(String tableName)
            : this(null,tableName, null)
        {
            Context = new DapperContext(this.ConnectionString);
            _dapperMapper.SetMapper<T>();
        }

        public DapperTable()
            : this(null,null, null)
        {
            
            Context = new DapperContext(this.ConnectionString);
            _dapperMapper.SetMapper<T>();
        }

        public virtual T GetSingle(String criteria, object param)
        {
            if (String.IsNullOrEmpty(_tableName))
            {
                var tableattr = typeof(T).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();

                if (tableattr != null)
                {
                    this._tableName = tableattr.Name;
                }
                else
                {
                    throw new ArgumentNullException("Você deve passar um nome da tabela representada pela entidade no construtor");
                }
            }
            T item = default(T);
            using (var cn = Context.Connection)
            {
                object oparam = param;
                item = cn.Query<T>(String.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE {1}", _tableName, criteria), oparam).FirstOrDefault();
            }
            return item;
        }
        public virtual List<T> GetAll(String criteria, dynamic param)
        {
            if (String.IsNullOrEmpty(_tableName))
            {
                var tableattr = typeof(T).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();

                if (tableattr != null)
                {
                    this._tableName = tableattr.Name;
                }
                else
                {
                    throw new ArgumentNullException("Você deve passar um nome da tabela representada pela entidade no construtor");
                }
            }
            List<T> items = default(List<T>);

            using (var cn = Context.Connection)
            {
                object oparam = param;
                if (string.IsNullOrEmpty(criteria))
                {
                    items = cn.Query<T>(String.Format("SELECT * FROM {0} WITH(NOLOCK)", _tableName)).ToList();
                }
                else
                {
                    items = cn.Query<T>(String.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE {1}", _tableName, criteria), oparam).ToList();
                }

            }

            return items;
        }

        public void Update(T myType)
        {
            using (var cn = Context.Connection)
            {
                cn.Update<T>(myType);
            }
        }

        public void Delete(T myType)
        {
            using (var cn = Context.Connection)
            {
                cn.Delete<T>(myType);
            }
        }

        public long Create(T myType)
        {
            using (var cn = Context.Connection)
            {
                return cn.Insert<T>(myType);
            }
        }


        public T GetById(int id)
        {
            using (var cn = Context.Connection)
            {
                return cn.Get<T>(id);
            }
        }

        public async Task<int> ExecuteCommand(string command, dynamic param)
        {
            using (var cn = Context.Connection)
            {
                return await this.Context.Connection.ExecuteAsync(command, (object)param);
            }
        }
        #region IDisposable Members

        // IDisposable implementation

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Connection.Dispose();
                }
                disposed = true;
            }

        }

        // Destructor
        ~DapperTable()
        {
            Dispose(false);
        }

        #endregion
    }
}
