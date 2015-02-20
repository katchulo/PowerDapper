using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;

namespace Katchulo.PowerDapper.Infra.DapperRepository
{
    public class DapperContext : IDisposable
    {
        private IDbConnection _connection;
        public IDbConnection Connection
        {
            get
            {
                int attempts = 0;
                if (_connection != null)
                {
                    while ((_connection.State == ConnectionState.Fetching || _connection.State == ConnectionState.Executing || _connection.State == ConnectionState.Connecting) && attempts < 20)
                    {
                        System.Threading.Thread.Sleep(20);
                        attempts++;
                    }

                    if(attempts >= 20)
                    {
                        throw new Exception("Max attempts exceeded.");
                    }
                }

                if (_connection == null || _connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
                {
                    this.OpenConnection();
                }                
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public String ConnectionString { get; set; }

        public DapperContext(String connectionString)
        {
            
            this.ConnectionString = connectionString;
        }

        private void OpenConnection()
        {
            try
            {
                _connection = new SqlConnection(this.ConnectionString);
                _connection.Open();
            }
            catch (NullReferenceException ex) { throw new NullReferenceException("Não foi encontrada nenhuma configuração de conexão para esta aplicação", ex); }
            catch (Exception ex) { throw ex; }
        }
        public virtual void Execute(String sqlQuery, object param)
        {
            OpenConnection();

            Connection.Execute(sqlQuery, param);
        }
        public virtual IEnumerable<T> List<T>(String sqlQuery, object param)
        {
            OpenConnection();

            return Connection.Query<T>(sqlQuery, param);
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
                    Connection.Dispose();
                }
                disposed = true;
            }

        }

        // Destructor
        ~DapperContext()
        {
            Dispose(false);
        }

        #endregion
    }

}
