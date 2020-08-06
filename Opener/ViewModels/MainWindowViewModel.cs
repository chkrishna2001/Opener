using Opener.Helpers;
using Opener.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Opener.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly OpenerDataHelper openerDataHelper;
        public MainWindowViewModel(OpenerDataHelper openerDataHelper)
        {
            this.openerDataHelper = openerDataHelper;
            KeyTypes = new ObservableCollection<OKeyType>(this.openerDataHelper.GetKeyTypes());
            Keys = new KeyUIObjects();
            var keys = openerDataHelper.GetKeys();
            Keys.AddRange(keys.Select(k => new OKeyUI(k)));
            Keys.ItemEndEdit += Keys_ItemEndEdit;
            Keys.CollectionChanged += Keys_CollectionChanged;
        }

        private void Keys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                try
                {
                    foreach (object item in e.OldItems)
                    {
                        var uiObject = sender as OKeyUI;
                        openerDataHelper.DeletyeKey(uiObject.GetDataObject());
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void Keys_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            var uiObject = sender as OKeyUI;
            if (!string.IsNullOrWhiteSpace(uiObject.GetDataObject().Key) && !string.IsNullOrWhiteSpace(uiObject.GetDataObject().Path))
            {
                openerDataHelper.SaveKey(uiObject.GetDataObject()); 
            }
        }
        
        #region Properties
        private string _title = "Opener Key Manager";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _SearchTerm;
        public string SearchTerm
        {
            get { return _SearchTerm; }
            set { SetProperty(ref _SearchTerm, value); }
        }

        private OKeyType _SelectedKeyType;
        public OKeyType SelectedKeyType
        {
            get { return _SelectedKeyType; }
            set { SetProperty(ref _SelectedKeyType, value); }
        }

        private KeyUIObjects _Keys;
        public KeyUIObjects Keys
        {
            get { return _Keys; }
            set { SetProperty(ref _Keys, value); }
        }

        private ObservableCollection<OKeyType> _KeyTypes;
        public ObservableCollection<OKeyType> KeyTypes
        {
            get { return _KeyTypes; }
            set { SetProperty(ref _KeyTypes, value); }
        }
        #endregion
    }
}
