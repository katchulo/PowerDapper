using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katchulo.PowerDapper.Infra.Repository
{
    public interface IRepository<T> where T : class
    {
        void Update(T myType);
        void Delete(T myType);
        long Create(T myType);
        T GetById(int id);
        T GetSingle(string criteria, dynamic param);
        List<T> GetAll(string criteria, dynamic param);
        Task<int> ExecuteCommand(string command, dynamic param);
    }
}
