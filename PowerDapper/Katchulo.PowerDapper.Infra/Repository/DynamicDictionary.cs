using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katchulo.PowerDapper.Infra.Repository
{
    /// <summary>
    /// Container para objetos dinâmicos utilizados para critérios
    /// Implementação proposta em:
    /// http://msdn.microsoft.com/pt-br/library/system.dynamic.dynamicobject(v=vs.110).aspx
    /// </summary>
    public class DynamicDictionary : DynamicObject
    {
        /// <summary>
        /// Dicionário de armazenento de propriedades e valores
        /// </summary>
        private readonly Dictionary<string, object> dictionary;

        /// <summary>
        /// Construtor do container
        /// </summary>
        public DynamicDictionary()
            : this(new Dictionary<string, object>())
        {
        }

        /// <summary>
        /// Construtor do container
        /// </summary>
        /// <param name="dictionary">dicionário de armazenamento</param>
        public DynamicDictionary(Dictionary<string, object> dictionary)
        {
            this.dictionary = dictionary;
        }

        /// <summary>
        /// Tenta obter o valor de uma propriedade
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return dictionary.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        /// Tenta estabelecer o valor em uma propriedade
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;

            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            List<string> memberNames = new List<string>();
            foreach (string key in this.dictionary.Keys)
            {
                memberNames.Add(key);
            }
            return memberNames;
        }        

        public void AddNewMember(string memberName, object value)
        {
            dictionary.Add(memberName, value);
        }
    }
}
