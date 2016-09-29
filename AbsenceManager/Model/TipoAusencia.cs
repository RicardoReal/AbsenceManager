using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(TipoAusenciaMetadata))]
    public partial class TipoAusencia : EntityObject
    {
    }

    [DisplayName("Tipo Ausências")]
    public class TipoAusenciaMetadata
    {
        [Display(Name = "Descrição", Order = 1)]
        public object Descricao { get; set; }

        [ScaffoldColumn(false)]
        public object AusenciaFuncionarios { get; set; }
    }
}