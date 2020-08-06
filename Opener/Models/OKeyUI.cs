using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Opener.Models
{
    public class KeyUIObjects : ObservableCollection<OKeyUI>
    {
        protected override void InsertItem(int index, OKeyUI item)
        {
            base.InsertItem(index, item);

            // handle any EndEdit events relating to this item
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }

        void ItemEndEditHandler(IEditableObject sender)
        {
            ItemEndEdit?.Invoke(sender);
        }

        #region events

        public event ItemEndEditEventHandler ItemEndEdit;

        #endregion
    }
    public delegate void ItemEndEditEventHandler(IEditableObject sender);
    public class OKeyUI : IEditableObject, INotifyPropertyChanged
    {
        private OKey keyObj;
        public OKeyUI()
        {
            keyObj = new OKey();
        }
        public OKeyUI(OKey keyObj)
        {
            this.keyObj = keyObj;
        }

        public OKey GetDataObject()
        {
            return keyObj;
        }

        public string Id
        {
            get
            {
                return keyObj.Id;
            }
            set
            {
                keyObj.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public string Key
        {
            get
            {
                return keyObj.Key;
            }
            set
            {
                keyObj.Key = value;
                RaisePropertyChanged("Key");
            }
        }

        public string Path
        {
            get
            {
                return keyObj.Path;
            }
            set
            {
                keyObj.Path = value;
                RaisePropertyChanged("Path");
            }
        }

        public OKeyType KeyType
        {
            get
            {
                return keyObj.KeyType;
            }
            set
            {
                keyObj.KeyType = value;
                RaisePropertyChanged("KeyType");
            }
        }
        #region events

        public event ItemEndEditEventHandler ItemEndEdit;

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
        }

        public void CancelEdit()
        {
        }

        public void EndEdit()
        {
            ItemEndEdit?.Invoke(this);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
