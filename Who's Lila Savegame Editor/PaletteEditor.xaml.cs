/****************************************************************************************
 *   PaletteEditor.xaml.cs  --  This file is part of whos-lila-savegame-editor.         *
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

using ColorPicker.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for PaletteEditor.xaml
    /// </summary>
    public partial class PaletteEditor : Window
    {
        private readonly ObservableCollection<Palette> extraPalettes;
        private Palette? originalPalette;
        private PersistentSaveData saveData;
        public ColorState Light { get; set; }
        public ColorState Dark { get; set; }
        public PaletteEditor(ref ObservableCollection<Palette> paletteList, ref PersistentSaveData saveData)
        {
            InitializeComponent();
            extraPalettes = paletteList;
            this.Light = new ColorState(0, 0, 0, 1.0, 0, 0, 0, 0, 0, 0);
            this.Dark = new ColorState(0, 0, 0, 1.0, 0, 0, 0, 0, 0, 0);
            this.lightPicker.DataContext = this;
            this.darkPicker.DataContext = this;
            this.saveData = saveData;
        }

        public PaletteEditor(Palette p, ref ObservableCollection<Palette> paletteList, ref PersistentSaveData saveData)
        {
            InitializeComponent();
            extraPalettes = paletteList;
            this.Light = new ColorState(p.Light.ScR, p.Light.ScG, p.Light.ScB, p.Light.ScA, 0, 0, 0, 0, 0, 0);
            this.Dark = new ColorState(p.Dark.ScR, p.Dark.ScG, p.Dark.ScB, p.Dark.ScA, 0, 0, 0, 0, 0, 0);
            this.paletteName.Text = p.Name;
            this.lightPicker.DataContext = this;
            this.darkPicker.DataContext = this;
            this.saveData = saveData;
            this.originalPalette = p;
        }

        private void SavePalette(object sender, RoutedEventArgs e)
        {
            Palette p = new Palette(
                this.paletteName.Text,
                Color.FromScRgb(
                    (float)Light.A,
                    (float)Light.RGB_R,
                    (float)Light.RGB_G,
                    (float)Light.RGB_B
                ),
                Color.FromScRgb(
                    (float)Dark.A,
                    (float)Dark.RGB_R,
                    (float)Dark.RGB_G,
                    (float)Dark.RGB_B
                ),
                null,
                0,
                ref this.saveData
            );
            if(this.originalPalette is not null)
            {
                this.extraPalettes.Remove(this.originalPalette);
                this.originalPalette.IsObtained = false;
            }
            this.extraPalettes.Add(p);
            p.IsObtained = true;

            this.Close();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
