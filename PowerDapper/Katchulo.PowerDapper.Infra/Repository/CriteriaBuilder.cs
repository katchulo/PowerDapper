using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katchulo.PowerDapper.Infra.Repository
{
    /// <summary>
    /// Ferramenta de construção de critérios
    /// </summary>
    public class CriteriaBuilder: IGetCriteria
    {

        public CriteriaBuilder()
        {
            this.Container = new DynamicDictionary();
            this.CriteriaText = new StringBuilder();
        }

        public DynamicDictionary Container { get; private set; }
        public StringBuilder CriteriaText { get; private set; }

        private bool addAnd = false;

        public void AddCriteria(string field, object value)
        {
            this.Container.AddNewMember(field, value);
            if (addAnd)
            {
                this.CriteriaText.Append(" AND ");
            }
            else
            {
                addAnd = true;
            }

            this.CriteriaText.AppendFormat(" {0} = @{0} ", field);
        }
        public void AddCustom(string customCriteria, List<KeyValuePair<string, object>> fieldsValues)
        {
            foreach (KeyValuePair<string, object> item in fieldsValues)
            {
                this.Container.AddNewMember(item.Key, item.Value);
            }
            
            if (addAnd)
            {
                this.CriteriaText.Append(" AND");
            }
            else
            {
                addAnd = true;
            }

            this.CriteriaText.Append(customCriteria);
        }

        public Criteria GetCriteria()
        {
            return new Criteria()
            {
                CriteriaText = this.CriteriaText.ToString(),
                Parameters = this.Container
            };
        }
    }
}
