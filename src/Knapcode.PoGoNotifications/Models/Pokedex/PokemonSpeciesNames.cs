using System;
using System.Collections.Generic;

namespace Knapcode.PoGoNotifications.Models.Pokedex
{
    public partial class PokemonSpeciesNames
    {
        public int PokemonSpeciesId { get; set; }
        public int LocalLanguageId { get; set; }
        public string Name { get; set; }
        public string Genus { get; set; }

        public virtual Languages LocalLanguage { get; set; }
        public virtual PokemonSpecies PokemonSpecies { get; set; }
    }
}
