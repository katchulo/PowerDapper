using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katchulo.PowerDapper.Infra.Repository
{
    /// <summary>
    /// Critérios para execução das queries
    /// </summary>
    public class Criteria
    {
        /// <summary>
        /// Critério de seleção
        /// </summary>
        public string CriteriaText { get; set; }

        /// <summary>
        /// Parâmetros do critério de seleção
        /// </summary>
        public dynamic Parameters { get; set; }
    }
}
