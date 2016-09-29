using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(EmpresaMetadata))]
    public partial class Empresa : EntityObject
    {
    }

    [DisplayName("Empresas")]
    public class EmpresaMetadata
    {
        [Display(Name = "Nome", Order = 1)]
        public object Nome { get; set; }

        [Display(Name = "Valor por Funcionário", Order = 2)]
        public object ValorPorFunc { get; set; }

        [ScaffoldColumn(false)]
        public object Users { get; set; }

        [ScaffoldColumn(false)]
        public object Roles { get; set; }
    }
}