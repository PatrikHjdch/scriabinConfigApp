using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class MainModel
    {
        public ObservableCollection<AbstractLinkModel> Links { get; set; }
        public MainModel()
        {
            Links = new ObservableCollection<AbstractLinkModel>();
        }
    }
}
