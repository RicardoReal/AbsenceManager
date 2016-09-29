using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Objects.DataClasses;

namespace AbsenceManager
{
    [MetadataType(typeof(TrackUserMetadata))]
    public partial class TrackUser : EntityObject
    {
        public TrackUser() { }

        public TrackUser(long UserID, string NomeFunc, string NrFunc, string Alteração, string ValorAntigo, string ValorNovo, string AlteradoPor, DateTime ChangedDateTime)
        {
            this.UserID = UserID;
            this.NomeFuncionario = NomeFunc;
            this.NrFunc = NrFunc;
            this.Alteração = Alteração;
            this.ValorAntigo = ValorAntigo;
            this.ValorNovo = ValorNovo;
            this.AlteradoPor = AlteradoPor;
            this.DataAlteração = ChangedDateTime;
        }
    }

    [DisplayName("Registo Funcionário")]
    public class TrackUserMetadata
    {
        [Display(Name = "Nome", Order = 1)]
        public object NomeFuncionario { get; set; }

        [Display(Name = "Nr. Func.", Order = 2)]
        public object NrFunc { get; set; }

        [Display(Name = "Alteração", Order = 3)]
        public object Alteração { get; set; }

        [Display(Name = "Valor Antigo", Order = 4)]
        public object ValorAntigo { get; set; }

        [Display(Name = "Valor Novo", Order = 5)]
        public object ValorNovo { get; set; }

        [Display(Name = "Alterado Por", Order = 6)]
        public object AlteradoPor { get; set; }

        [Display(Name = "Data Alteração", Order = 7)]
        [FilterUIHint("DateGreaterThan", null, "Mode", "Today")]
        public object DataAlteração { get; set; }

        [ScaffoldColumn(false)]
        public object UserID { get; set; }
    }
}