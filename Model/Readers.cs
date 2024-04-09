using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Readers
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

		[Column("Phone Number")]
		public string Phone_Number { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

		[Column("House Number")]
		public string House_Number { get; set; }
    }
}
