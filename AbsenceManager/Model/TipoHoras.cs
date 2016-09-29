using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(TipoHorasMetadata))]
    public partial class TipoHora : EntityObject
    {
    }

    [DisplayName("Tipo Horas")]
    public class TipoHorasMetadata
    {
        [Display(Name = "Descrição", Order = 1)]
        public object Descricao { get; set; }

        [Display(Name = "Valor Hora", Order = 2)]
        public object ValorHora { get; set; }


        [Display(Name = "Horas Extra", Order = 3)]
        public object HorasExtra { get; set; }

        [ScaffoldColumn(false)]
        public object FuncionarioHorasFuncionarios { get; set; }
    }
}