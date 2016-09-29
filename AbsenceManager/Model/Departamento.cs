using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(DepartamentoMetadata))]
    public partial class Departamento : EntityObject
    {
    }

    [DisplayName("Departamentos")]
    public class DepartamentoMetadata
    {
        [Display(Name = "Nome", Order = 1)]
        public object Nome { get; set; }

        [Display(Name = "Descrição", Order = 2)]
        public object Descricao { get; set; }

        [Display(Name = "Responsável", Order = 3)]
        public object User { get; set; }

        [ScaffoldColumn(false)]
        public object Users { get; set; }

        [ScaffoldColumn(false)]
        public object HorasFuncionarios { get; set; }
    }
}