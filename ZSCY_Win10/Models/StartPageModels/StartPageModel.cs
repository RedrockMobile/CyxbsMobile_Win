using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ZSCY.Models;

namespace ZSCY_Win10.Models.StartPageModels
{
    [DataContract]
    public class StartPageModel : BaseModel
    {
        private string _PictrueSource;
        private bool _HasPictrue;
        private HorizontalAlignment _HorMode;
        private VerticalAlignment _VerMode;
        private Stretch _StretchMode;
        public HorizontalAlignment HorMode
        {
            get { return _HorMode; }
            set
            {
                _HorMode = value;
                RaisePropertyChanged(nameof(HorMode));
            }
        }
        public VerticalAlignment VerMode
        {
            get { return _VerMode; }
            set
            {
                _VerMode = value;
                RaisePropertyChanged(nameof(VerMode));
            }
        }
        public Stretch StretchMode
        {
            get { return _StretchMode; }
            set
            {
                _StretchMode = value;
                RaisePropertyChanged(nameof(StretchMode));
            }
        }
        public string PictrueSource
        {
            get
            {
                return _PictrueSource;
            }
            set
            {
                _PictrueSource = value;
                RaisePropertyChanged(nameof(PictrueSource));
            }
        }
        public bool HasPictrue
        {
            get
            {
                return _HasPictrue;
            }
            set
            {
                _HasPictrue = value;
                RaisePropertyChanged(nameof(HasPictrue));
            }
        }


    }
}
