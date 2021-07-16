using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDVaccineIND.Models
{
    public class CoWinResponseModel
    {
        public Session[] sessions { get; set; }
    }

    public class Session
    {
        public int center_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string state_name { get; set; }
        public string district_name { get; set; }
        public string block_name { get; set; }
        public int pincode { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int lat { get; set; }
        public int _long { get; set; }
        private string _fee_type;
        public string fee_type
        {
            get
            {
                return _fee_type;
            }
            set 
            {
                _fee_type = value;
                if (string.Compare(value, "Paid", true) == 0)
                {
                    fontColor = "#2146db";
                }
                else
                {
                    fontColor = "#18b512";
                }
            }
        }
        public string session_id { get; set; }
        public string date { get; set; }
        public int available_capacity { get; set; }
        public int available_capacity_dose1 { get; set; }
        public int available_capacity_dose2 { get; set; }
        public string fee { get; set; }
        public int min_age_limit { get; set; }
        public bool allow_all_age { get; set; }
        public string vaccine { get; set; }
        public string[] slots { get; set; }
        public int max_age_limit { get; set; }
        public string fontColor { get; set; }
    }
}
