using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BikeTouringGIS.Controls
{
    public class HyperLinkControl : Hyperlink
    {
        protected override void OnClick()
        {
            base.OnClick();

            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = this.NavigateUri.AbsoluteUri
                }
            };
            p.Start();
        }
    }
}
