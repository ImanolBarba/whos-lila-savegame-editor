/****************************************************************************************
 *   AddressesPage.xaml.cs  --  This file is part of whos-lila-savegame-editor.         *
 *                                                                                      *
 *   Copyright (C) 2023 Imanol-Mikel Barba Sabariego                                    *
 *                                                                                      *
 *   whos-lila-savegame-editor is free software: you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by the Free      *
 *   Software Foundation, either version 3 of the License, or (at your option) any      *
 *   later version.                                                                     *
 *                                                                                      *
 *   whos-lila-savegame-editor is distributed in the hope that it will be useful, but   *
 *   WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or      *
 *   FITNESS FOR A PARTICULAR  PURPOSE. See the GNU General Public License for more     *
 *   details.                                                                           *
 *                                                                                      *
 *   You should have received a copy of the GNU General Public License along with this  *
 *   program. If not, see http://www.gnu.org/licenses/.                                 *
 *                                                                                      *
 ****************************************************************************************/

using System.Collections.Generic;
using System.Windows.Controls;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for AddressesPage.xaml
    /// </summary>
    public class Address
    {
        private readonly AddressIndex index;
        private readonly PersistentSaveData saveData;

        public Address(AddressIndex index, ref PersistentSaveData saveData)
        {
            this.index = index;
            this.saveData = saveData;
        }

        public bool IsObtained
        {
            get
            {
                return this.saveData.Addresses.Contains(this.index);
            }

            set
            {
                if (value && !this.saveData.Addresses.Contains(this.index))
                {
                    this.saveData.Addresses.Add(this.index);
                    MainWindow.MarkChanges();
                }
                else if (!value && this.saveData.Addresses.Contains(this.index))
                {
                    this.saveData.Addresses.Remove(this.index);
                    MainWindow.MarkChanges();
                }
            }
        }

        public string Name
        {
            get
            {
                switch (this.index)
                {
                    case AddressIndex.SchoolAddress:
                        return "School - 3011 E Goth Blvd";
                    case AddressIndex.TrainStationAddress:
                        return "Train station - Baltimore Street station 11 W";
                    case AddressIndex.PartyAddress:
                        return "Party - 29271 Fairfox St";
                    case AddressIndex.HomeAddress:
                        return "Home - 9031 Oakman St";
                    case AddressIndex.BurntAddress:
                        return "??? - Cortland St. ap. 33 (Lawrence Fraternity HQ)";
                    case AddressIndex.MarthasAddress:
                        return "Martha - 12740 Birthwood St";
                    case AddressIndex.SunshineCafeAddress:
                        return "??? - 200 Grande Baver Ave (Sunshine Cafe)";
                    case AddressIndex.TanyaAddress:
                        return "Tanya - 2524 Clearmount Ave";
                    case AddressIndex.RyibkinAddress:
                        return "??? - Ulitsa Lenina, 93 (Ryibkin's Apartment)";
                    default:
                        break;
                }
                return "UNKNOWN ADDRESS";
            }
        }
    }

    public partial class AddressesPage : UserControl
    {
        public AddressesPage(ref PersistentSaveData saveData)
        {
            InitializeComponent();
            List<Address> items = new List<Address>
            {
                new Address(AddressIndex.SchoolAddress, ref saveData),
                new Address(AddressIndex.TrainStationAddress, ref saveData),
                new Address(AddressIndex.PartyAddress, ref saveData),
                new Address(AddressIndex.HomeAddress, ref saveData),
                new Address(AddressIndex.BurntAddress, ref saveData),
                new Address(AddressIndex.MarthasAddress, ref saveData),
                new Address(AddressIndex.SunshineCafeAddress, ref saveData),
                new Address(AddressIndex.TanyaAddress, ref saveData),
                new Address(AddressIndex.RyibkinAddress, ref saveData),
            };
            this.addressesList.ItemsSource = items;
        }
    }
}
