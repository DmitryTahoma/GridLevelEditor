﻿using Catel.Data;
using Catel.MVVM;
using GridLevelEditor.Models;
using GridLevelEditor.Objects;
using System.Collections.ObjectModel;

namespace GridLevelEditor.ViewModels.Controls
{
    class OpenLevelFormViewModel : ViewModelBase
    {
        public delegate void LoadLevelHandler(Level level);

        private LoadLevelHandler onLevelSelected;

        public OpenLevelFormViewModel()
        {
            Levels = new ObservableCollection<Level>();

            LoadLevel = new Command(OnLoadLevelExecute);

            onLevelSelected = null;
        }

        #region Properties

        public ObservableCollection<Level> Levels { set; get; }

        public Level SelectedLevel
        {
            get => GetValue<Level>(SelectedLevelProperty);
            set => SetValue(SelectedLevelProperty, value);
        }
        public static readonly PropertyData SelectedLevelProperty = RegisterProperty(nameof(SelectedLevel), typeof(Level), null);

        #endregion

        #region Commands

        public Command LoadLevel { get; private set; }
        private void OnLoadLevelExecute()
        {
            onLevelSelected?.Invoke(SelectedLevel);
        }

        #endregion

        public void LoadLevels()
        {
            Levels.Clear();
            FileIO.GetAllLevels(Levels);
        }

        public void SetLevelSelected(LoadLevelHandler hanlder)
        {
            if(hanlder != null)
            {
                onLevelSelected = hanlder;
            }
        }
    }
}
