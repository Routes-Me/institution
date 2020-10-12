﻿namespace InstitutionService.Helper.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Host { get; set; }
        public int Prime { get; set; }
        public int PrimeInverse { get; set; }
    }
}
