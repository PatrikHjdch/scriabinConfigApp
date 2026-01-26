using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class LinkProfileTabModel
    {
        public ObservableCollection<AbstractLinkModel> Links { get; set; }
        public LinkProfileTabModel(int id)
        {
            Links = new ObservableCollection<AbstractLinkModel>();
            this.id = id;
        }

        public int id;
    }
}
