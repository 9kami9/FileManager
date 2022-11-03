using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManager.Commands;
using FileManager.Models;
using FileManager.Services;
using FileManager.ViewModels.Helpers;

namespace FileManager.ViewModels
{
    public class FileViewModel : BaseViewModel
    {
        private IModel _model;
        private RelayCommand _openFileCommand;
        private RelayCommand _selectFileCommand;

        public FileViewModel(IModel model)
        {
            _model = model;
        }

        public string Icon => _model.Icon; 
        public string Name => _model.Name;
        public string Path => _model.Path;

        public RelayCommand OpenFileCommand
            => _openFileCommand ??= new RelayCommand(o => OpenItem());

        public RelayCommand SelectFileCommand
            => _selectFileCommand ??= new RelayCommand(o => SelectFile(_model));

        private void OpenItem()
        {
            NavigationService.Default.ChangeFolderPathOrOpenFile(Path);
            ActionService.Default.ChangeInfo(null);
            //SelectFile(null);
        }

        private void SelectFile(IModel model)
        {
            var info = InformationHelper.GetInfo(model);
            ActionService.Default.ChangeInfo(info);
        }
    }
}
