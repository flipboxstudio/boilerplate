using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Model {
    [Table("GeoNames")]
    public class GeoNames {
        [Key]
        [Column("geonameid")]
        [JsonProperty("geoname_id")]
        public int GeoNameId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("asciiname")]
        public string AsciiName { get; set; }

        [Column("alternatenames")]
        public string AlternateNames { get; set; }

        [Column("latitude")]
        public decimal Latitude { get; set; }

        [Column("longitude")]
        public decimal Longitude { get; set; }

        [Column("feature_class")]
        public string FeatureClass { get; set; }

        [Column("feature_code")]
        public string FeatureCode { get; set; }

        [Column("country_code")]
        public string CountryCode { get; set; }

        [Column("cc2")]
        public string AlternateCountryCode { get; set; }

        [Column("admin1_code")]
        public string FirstAdminCode { get; set; }

        [Column("admin2_code")]
        public string SecondAdminCode { get; set; }

        [Column("admin3_code")]
        public string ThirdAdminCode { get; set; }

        [Column("admin4_code")]
        public string FourthAdminCode { get; set; }

        [Column("population")]
        public long Population { get; set; }

        [Column("elevation")]
        public int Elevation { get; set; }

        [Column("gtopo30")]
        public int DigitalElevationModel { get; set; }

        [Column("timezone")]
        public string TimeZone { get; set; }

        [Column("modification_date")]
        public DateTime ModificationDate { get; set; }
    }
}