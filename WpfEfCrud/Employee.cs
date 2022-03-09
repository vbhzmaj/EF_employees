namespace WpfEfCrud
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]


        public int EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }


        public DateTime? BirthDate { get; set; }


        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

    }

}
