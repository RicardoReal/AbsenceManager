using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Objects.DataClasses;

namespace AbsenceManager
{
    [MetadataType(typeof(RoleMetadata))]
    [DisplayColumn("Description", "Description", false)]
    public partial class Role : EntityObject
    {
        public override string ToString()
        {

            return this.Role1;
        }
    }

    [DisplayName("Roles")]
    public class RoleMetadata
    {
        [Display(Name = "Role")]
        public object Role1 { get; set; }

        [Display(Name = "Descrição")]
        public object Description { get; set; }

        [Display(Name = "Screens")]
        [UIHint("GridViewScreens")]
        [HideColumnIn(PageTemplate.List)]
        public object RoleApplicationScreens { get; set; }

        [ScaffoldColumn(false)]
        public object Users { get; set; }

        [Display(Name = "Empresa")]
        public object Empresa { get; set; }
    }
}