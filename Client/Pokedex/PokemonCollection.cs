﻿// This file is part of Mystery Dungeon eXtended.

// Copyright (C) 2015 Pikablu, MDX Contributors, PMU Staff

// Mystery Dungeon eXtended is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Mystery Dungeon eXtended is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Mystery Dungeon eXtended.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Logic.Pokedex
{
    /// <summary>
    /// Description of PokemonCollection.
    /// </summary>
    class PokemonCollection
    {
        #region Fields

        private PMDCP.Core.ListPair<int, Pokemon> mPokemon;

        #endregion Fields

        #region Constructors

        internal PokemonCollection()
        {
            mPokemon = new PMDCP.Core.ListPair<int, Pokemon>();
        }

        #endregion Constructors

        #region Indexers

        public Pokemon this[int index]
        {
            get { return mPokemon[index]; }
            set { mPokemon[index] = value; }
        }

        #endregion Indexers

        #region Methods

        public void AddPokemon(int index, Pokemon pokemonToAdd)
        {
            mPokemon.Add(index, pokemonToAdd);
        }

        public void ClearPokemon()
        {
            mPokemon.Clear();
        }

        #endregion Methods
    }
}
