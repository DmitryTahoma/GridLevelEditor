using Catel.MVVM;
using GridLevelEditor.Controls;
using GridLevelEditor.Models;
using GridLevelEditor.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GridLevelEditor.ViewModels.Controls
{
    class LevelContentViewModel : ViewModelBase
    {
        public delegate void ActionMgElemHandler(MgElem elem);
        public delegate void FillingGridHandler(Grid grid, MouseEventHandler imageChanger);

        private DialogManager dialogManager;
        private ControlCreator controlCreator;
        private ActionMgElemHandler additionMgElem;
        private ActionMgElemHandler deletionMgElem;
        private Action cleansingMgElems;
        private FillingGridHandler fillingGrid;

        private Grid grid;
        private StackPanel mgElemsStack;
        private List<MgElemControl> imagesSelect;

        public LevelContentViewModel()
        {
            dialogManager = new DialogManager();
            controlCreator = new ControlCreator();
            additionMgElem = null;
            deletionMgElem = null;
            cleansingMgElems = null;
            fillingGrid = null;

            grid = null;
            mgElemsStack = null;
            imagesSelect = new List<MgElemControl>();

            AddMgElem = new Command(OnAddMgElemExecute);
            RemoveMgElem = new Command(OnRemoveMgElemExecute);
            FillAllVoids = new Command(OnFillAllVoidsExecute);
            ClearField = new Command(OnClearFieldExecute);
            BindStackPanel = new Command<StackPanel>(OnBindStackPanelExecute);
            BindGrid = new Command<Grid>(OnBindGridExecute);
        }

        #region Properties

        public int CellSize { set; get; }
        public MgElemControl SelectedMgElem { private set; get; }

        #endregion

        #region Commands

        public Command AddMgElem { get; private set; }
        private void OnAddMgElemExecute()
        {
            if (additionMgElem != null)
            {
                string filename = dialogManager.GetImageFilepath();

                if (filename != "")
                {
                    BitmapImage image = new BitmapImage(new Uri(filename));
                    if (image.PixelWidth != image.PixelHeight && dialogManager.PictureNot1to1() == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }

                    MgElemControl control = controlCreator.CreateMgElemControl(50, image, SelectImage);
                    mgElemsStack.Children.Add(control);
                    imagesSelect.Add(control);
                    additionMgElem.Invoke(control.ViewModel.GetModel());

                    if (imagesSelect.Count == 1)
                    {
                        control.ViewModel.SelectVisibility = Visibility.Visible;
                        SelectedMgElem = control;
                    }
                }
            }
        }

        public Command RemoveMgElem { get; private set; }
        private void OnRemoveMgElemExecute()
        {
            if (deletionMgElem != null)
            {
                var selected = SelectedMgElem;
                if (selected != null)
                {
                    mgElemsStack.Children.Remove(selected);
                    imagesSelect.Remove(selected);
                    deletionMgElem.Invoke(selected.ViewModel.GetModel());

                    if (imagesSelect.Count != 0)
                    {
                        imagesSelect[0].ViewModel.SelectVisibility = Visibility.Visible;
                        SelectedMgElem = imagesSelect[0];
                    }
                }
            }
        }

        public Command FillAllVoids { get; private set; }
        private void OnFillAllVoidsExecute()
        {
            if(SelectedMgElem != null)
            {
                string voidImg = ResourceDriver.GetVoidBmp().UriSource.ToString();
                foreach (UIElement control in grid.Children)
                {
                    if(control is Image img)
                    {
                        if(img.Source.ToString() == voidImg)
                        {
                            img.Source = SelectedMgElem.ImgControl.Source;
                        }
                    }
                }
            }
        }

        public Command ClearField { get; private set; }
        private void OnClearFieldExecute()
        {
            foreach (UIElement control in grid.Children)
            {
                if(control is Image img)
                {
                    img.Source = ResourceDriver.GetVoidBmp();
                }
            }
        }

        public Command<StackPanel> BindStackPanel { get; private set; }
        private void OnBindStackPanelExecute(StackPanel stackPanel)
        {
            mgElemsStack = stackPanel;

            cleansingMgElems.Invoke();
            stackPanel.Children.Clear();
            foreach (MgElemControl control in imagesSelect)
            {
                stackPanel.Children.Add(control);
                additionMgElem.Invoke(control.ViewModel.GetModel());
            }
        }

        public Command<Grid> BindGrid { get; private set; }
        private void OnBindGridExecute(Grid grid)
        {
            try
            {
                fillingGrid.Invoke(grid, ChangeImage);
            }
            catch (Exception e)
            {
                dialogManager.GridCreationError(e.ToString());
            }
            this.grid = grid;
        }

        #endregion

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if (sender != null && sender is Image snd)
            {
                MgElemControl sndr = null;

                foreach (MgElemControl img in imagesSelect)
                {
                    img.ViewModel.SelectVisibility = Visibility.Hidden;
                    if (snd == img.ImgControl && sndr == null)
                    {
                        sndr = img;
                    }
                }

                if (sndr != null)
                {
                    sndr.ViewModel.SelectVisibility = Visibility.Visible;
                    SelectedMgElem = sndr;
                }
            }
        }

        private void ChangeImage(object sender, MouseEventArgs e)
        {
            if (sender != null && sender is Image img)
            {
                if (SelectedMgElem != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    img.Source = SelectedMgElem.ViewModel.ImageSource;
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    img.Source = ResourceDriver.GetVoidBmp();
                }
            }
        }

        public void SetAdditionMgElem(ActionMgElemHandler handler)
        {
            if(additionMgElem == null)
            {
                additionMgElem = handler;
            }
        }

        public void SetDeletionMgElem(ActionMgElemHandler handler)
        {
            if (deletionMgElem == null)
            {
                deletionMgElem = handler;
            }
        }

        public void SetCleansingMgElems(Action handler)
        {
            if(cleansingMgElems == null)
            {
                cleansingMgElems = handler;
            }
        }

        public void SetFillingGrid(FillingGridHandler handler)
        {
            if(fillingGrid == null)
            {
                fillingGrid = handler;
            }
        }

        public void CreateGrid(int rows, int cols, int size)
        {
            if(grid != null)
            {
                controlCreator.FillLevelGrid(ref grid, rows, cols, size, ChangeImage);
            }
        }

        public void CreateLoadedLevel(List<MgElem> elems)
        {
            foreach (MgElem elem in elems)
            {
                MgElemControl control = controlCreator.CreateMgElemControl(50, elem.Image, SelectImage);
                control.ViewModel.TextIndex = elem.Id;
                imagesSelect.Add(control);
            }
        }

        public void SetIdsUnique()
        {
            int uniqueId = 0;
            foreach(MgElemControl elem in imagesSelect)
            {
                if(elem.ViewModel.TextIndex == "")
                {
                    string id = "[]";
                    for (int count = -1; count != 0; uniqueId++)
                    {
                        id = '[' + uniqueId.ToString() + ']';
                        count = imagesSelect.Count((el) => el.ViewModel.TextIndex == id);
                    }
                    elem.ViewModel.TextIndex = id;
                }
            }

            foreach (MgElemControl elem in imagesSelect)
            {
                string id = elem.ViewModel.TextIndex;
                IEnumerable<MgElemControl> replicates = imagesSelect.Where((el) => el.ViewModel.TextIndex == id);
                if (replicates.Count() > 1)
                {
                    uniqueId = 1;
                    foreach (MgElemControl replicator in replicates)
                    {
                        replicator.ViewModel.TextIndex += ('(' + uniqueId.ToString() + ')');
                        uniqueId++;
                    }
                }
            }
        }

        public void ReInit()
        {
            imagesSelect.Clear();
            SelectedMgElem = null;
        }
    }
}
