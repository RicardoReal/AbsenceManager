using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(FuncionarioHorasFuncionarioMetadata))]
    public partial class FuncionarioHorasFuncionario : EntityObject
    {
    }

    [DisplayName("Horas Funcionários")]
    public class FuncionarioHorasFuncionarioMetadata
    {
        [Display(Name = "Funcionário", Order = 1)]
        [FilterUIHint("Autocomplete")]
        [UIHint("Autocomplete")]
        public object User { get; set; }

        [Display(Name = "Nr. Horas", Order = 2)]
        public object NrHoras { get; set; }

        [Display(Name = "Tipo Horas", Order = 3)]
        [HideColumnIn(PageTemplate.Insert)]
        public object TipoHora { get; set; }

        [ScaffoldColumn(false)]
        public object HorasFuncionario { get; set; }

        [ScaffoldColumn(false)]
        public object Data { get; set; }
    }
}