using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessRecords.Domain.Entities;

namespace FitnessRecords.Infrastructure.DTO
{
    public class FilmDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public Film ToEntity()
        {
            return new Film()
            {
                Name = name,
                Description = description
            };
        }
    }
}
