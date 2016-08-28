using System;
using System.Collections.Generic;

namespace Knapcode.PoGoNotifications.Models.Pokedex
{
    public partial class Languages
    {
        public Languages()
        {
            PokemonSpeciesNames = new HashSet<PokemonSpeciesNames>();
        }

        public int Id { get; set; }
        public string Iso639 { get; set; }
        public string Iso3166 { get; set; }
        public string Identifier { get; set; }
        public bool Official { get; set; }
        public int? Order { get; set; }

        public virtual ICollection<PokemonSpeciesNames> PokemonSpeciesNames { get; set; }
    }
}
