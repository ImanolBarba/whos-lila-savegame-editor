/****************************************************************************************
 *   PalettesPage.xaml.cs  --  This file is part of whos-lila-savegame-editor.          *
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

using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization;
using System.Windows;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for PallettesPage.xaml
    /// </summary>

    // Can't use the Palette class defined in the game assembly because it uses unity classes I can't use in a standalone application
    public class Palette 
    {
        private readonly PersistentSaveData saveData;

        public string Name { get; set; }
        public Color Light { get; set; }
        public Color Dark { get; set; }
        public string Path { get; set; }
        public ulong SteamWorkshopID { get; set; }

        public Palette(string name, Color light, Color dark, string path, ulong workshopID, ref PersistentSaveData saveData)
        {
            this.saveData = saveData;
            this.Name = name;
            this.Light = light;
            this.Dark = dark;
            this.Path = path;
            this.SteamWorkshopID = workshopID;
        }

        public static Color ConvertToColor(SerializableColor sC)
        {
            return Color.FromScRgb(sC.A, sC.R, sC.G, sC.B);
        }

        public static SerializableColor ConvertFromColor(Color c)
        {
            SerializableColor sC = (SerializableColor)FormatterServices.GetUninitializedObject(typeof(SerializableColor));
            sC.A = c.ScA;
            sC.R = c.ScR;
            sC.G = c.ScG;
            sC.B = c.ScB;
            return sC;
        }

        public SerializablePalette ToSerializablePalette()
        {
            SerializablePalette sP = (SerializablePalette)FormatterServices.GetUninitializedObject(typeof(SerializablePalette));
            sP.name = this.Name;
            sP.light = ConvertFromColor(this.Light);
            sP.dark = ConvertFromColor(this.Dark);
            sP.Path = this.Path;
            sP.id = new PublishedFileId_t(this.SteamWorkshopID);
            return sP;
        }

        public Palette(PersistentSaveData saveData, SerializablePalette sP)
        {
            this.saveData = saveData;
            this.Name = sP.name;
            this.Light = ConvertToColor(sP.light);
            this.Dark = ConvertToColor(sP.dark);
            this.Path = sP.Path;
            this.SteamWorkshopID = sP.id.m_PublishedFileId;
        }

        public bool IsObtained
        {
            get
            {
                try
                {
                    // In order to not confuse the user with two palettes of the same name with different
                    // colours, I'm considering them unique by name, whereas the game considers them unique
                    // by name and colour.
                    SerializablePalette sP = this.saveData.UnlockedPalettes.First(p => p.name == this.Name);
                    return sP != null;
                } catch (InvalidOperationException)
                {
                    
                }
                return false;
            }

            set
            {
                // If it exists, we're either removing or overwriting, in both cases we remove first.
                if (this.IsObtained)
                {
                    this.saveData.UnlockedPalettes.RemoveAll(p => p.name == this.Name);
                    MainWindow.MarkChanges();
                }
                if (value)
                {
                    this.saveData.UnlockedPalettes.Add(this.ToSerializablePalette());
                    MainWindow.MarkChanges();
                }
            }
        }
    }

    public partial class PalettesPage : UserControl
    {
        private ObservableCollection<Palette> extraPalettes;
        private PersistentSaveData saveData;
        public PalettesPage(ref PersistentSaveData saveData)
        {
            InitializeComponent();
            List<Palette> palettes = new List<Palette>
            {
                new Palette("Blue Rose",        Color.FromScRgb(1f, 0.7518957f,   0.764151f,   0.7028747f),   Color.FromScRgb(1f, 0.16153437f,  0.19712037f,  0.2830189f),   null, 0, ref saveData),
                new Palette("Boyle",            Color.FromScRgb(1f, 0.34901962f,  0.08235294f, 0.098039225f), Color.FromScRgb(1f, 0.3529412f,   0.3803922f,   0.22352943f),  null, 0, ref saveData),
                new Palette("Classic",          Color.FromScRgb(1f, 1f,           1f,          1f),           Color.FromScRgb(1f, 0f,           0f,           0f),           null, 0, ref saveData),
                new Palette("Cofee beans",      Color.FromScRgb(1f, 0.8679245f,   0.8038305f,  0.63456744f),  Color.FromScRgb(1f, 0.38679248f,  0.22666572f,  0.20616768f),  null, 0, ref saveData),
                new Palette("Default",          Color.FromScRgb(1f, 0.90334636f,  0.9433962f,  0.93389016f),  Color.FromScRgb(1f, 0.2264151f,   0.050978277f, 0.041651838f), null, 0, ref saveData),
                new Palette("Electrified Meat", Color.FromScRgb(1f, 0.5471698f,   0.49512348f, 0.45167318f),  Color.FromScRgb(1f, 0.31132078f,  0.06314526f,  0.11272008f),  null, 0, ref saveData),
                new Palette("KILFUC",           Color.FromScRgb(1f, 0.017205596f, 1f,          0f),           Color.FromScRgb(1f, 1f,           0f,           0.09860468f),  null, 0, ref saveData),
                new Palette("Lilac Mania",      Color.FromScRgb(1f, 0.49047703f,  0.6226415f,  0.58659667f),  Color.FromScRgb(1f, 0.28688172f,  0.21026167f,  0.3301887f),   null, 0, ref saveData),
                new Palette("Lucid Dream",      Color.FromScRgb(1f, 0.6473626f,   0.16340333f, 0.6792453f),   Color.FromScRgb(1f, 0.09301885f,  0.08018868f,  0.3207547f),   null, 0, ref saveData),
                new Palette("Manful Soul",      Color.FromScRgb(1f, 0.9150943f,   0.8403112f,  0.77265036f),  Color.FromScRgb(1f, 0.13995194f,  0.31188244f,  0.3490566f),   null, 0, ref saveData),
                new Palette("Nightly Comforts", Color.FromScRgb(1f, 0.5803222f,   0.764151f,   0.7407723f),   Color.FromScRgb(1f, 0.33962262f,  0.13242015f,  0.09772159f),  null, 0, ref saveData),
                new Palette("Nowak",            Color.FromScRgb(1f, 0.6117647f,   0.6039216f,  0.22352943f),  Color.FromScRgb(1f, 0.11764707f,  0.08235294f,  0.12941177f),  null, 0, ref saveData),
                new Palette("Red Tears",        Color.FromScRgb(1f, 0.5754717f,   0.08414916f, 0.08414916f),  Color.FromScRgb(1f, 0f,           0f,           0f),           null, 0, ref saveData),
                new Palette("Ripe Pumpkin",     Color.FromScRgb(1f, 0.8301887f,   0.6614767f,  0.36418653f),  Color.FromScRgb(1f, 0.18867922f,  0.057849765f, 0.105884776f), null, 0, ref saveData),
                new Palette("Schastye",         Color.FromScRgb(1f, 0.9150943f,   0.8546821f,  0.617257f),    Color.FromScRgb(1f, 0.5566038f,   0.48306328f,  0.3964489f),   null, 0, ref saveData),
                new Palette("Tomiho",           Color.FromScRgb(1f, 0.8235294f,   0.92156863f, 0.5647059f),   Color.FromScRgb(1f, 0.007843138f, 0.3019608f,   0.24292454f),  null, 0, ref saveData),
                new Palette("Violet Violence",  Color.FromScRgb(1f, 0.9245283f,   0.763172f,   0.80188715f),  Color.FromScRgb(1f, 0.13001125f,  0.082102165f, 0.14150941f),  null, 0, ref saveData),
            };
            extraPalettes = new ObservableCollection<Palette>();
            List<SerializablePalette> extraSerializablePalettes = saveData.UnlockedPalettes.Where(eP => palettes.All(p => eP.name != p.Name)).ToList();
            foreach (SerializablePalette sP in extraSerializablePalettes) 
            {
                extraPalettes.Add(new Palette(saveData, sP));
            }
            this.paletteList.ItemsSource = palettes;
            this.extraPaletteList.ItemsSource = extraPalettes;
            this.saveData = saveData;
        }

        private void AddCustomPalette(object sender, RoutedEventArgs e)
        {
            PaletteEditor pE = new PaletteEditor(ref this.extraPalettes, ref this.saveData);
            pE.Owner = Window.GetWindow(this);
            pE.ShowDialog();
            this.UpdateColumnWidths();
        }

        private void EditCustomPalette(object sender, RoutedEventArgs e)
        {
            if (this.extraPaletteList.SelectedItem is Palette selectedItem)
            {
                PaletteEditor pE = new PaletteEditor(selectedItem, ref this.extraPalettes, ref this.saveData);
                pE.Owner = Window.GetWindow(this);
                pE.ShowDialog();
            }
            this.UpdateColumnWidths();
        }
        private void RemoveCustomPalette(object sender, RoutedEventArgs e)
        {
            if (this.extraPaletteList.SelectedItem is Palette selectedItem)
            {
                selectedItem.IsObtained = false;
                extraPalettes.Remove(selectedItem);
            }
            this.UpdateColumnWidths();
        }

        public void UpdateColumnWidths()
        {
            // For each column...
            foreach (var column in ((GridView)this.extraPaletteList.View).Columns)
            {
                // If this is an "auto width" column...
                if (double.IsNaN(column.Width))
                {
                    // Set its Width back to NaN to auto-size again
                    column.Width = 0;
                    column.Width = double.NaN;
                }
            }
        }

    }
}
