using System;
using System.Collections.Generic;

namespace Knapcode.PoGoNotifications.Models.Pokedex
{
    public partial class Pokemon
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public int? SpeciesId { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public int Order { get; set; }
        public bool IsDefault { get; set; }

        public virtual PokemonSpecies Species { get; set; }
    }
}
