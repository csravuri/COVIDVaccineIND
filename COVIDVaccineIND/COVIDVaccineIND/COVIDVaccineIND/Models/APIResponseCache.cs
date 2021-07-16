using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDVaccineIND.Models
{
    public class APIResponseCache
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Unique]
        public string RequestURI { get; set; }
        public string Response { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
