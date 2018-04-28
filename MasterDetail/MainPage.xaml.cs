﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MasterDetail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            Emails = new List<Email>
            {
                new Email
                {
                    From = "Steve Johnson",
                    Subject = "Lunch Tomorrow",
                    Body = "Are you available for lunch tomorrow? A client would like to discuss a project with you.",
                    Thumbnail = new Uri("ms-appx:///Assets/People/shen.png")
                },
                new Email
                {
                    From = "Pete Davidson",
                    Subject = "Kids game",
                    Body = "Don't forget the kids have their soccer game this Friday. We have to supply end of game snacks.",
                    Thumbnail = new Uri("ms-appx:///Assets/People/pete.png")
                },
                new Email
                {
                    From = "OneDrive",
                    Subject = "Check out your event recap",
                    Body = "Your new album.\r\nYou uploaded some photos to yuor OneDrive and automatically created an album for you.",
                    Thumbnail = new Uri("ms-appx:///SamplePages/MasterDetailsView/OneDriveLogo.png")
                },
                new Email
                {
                    From = "Twitter",
                    Subject = "Follow randomPerson, APersonYouMightKnow",
                    Body = "Here are some people we think you might like to follow:\r\n.@randomPerson\r\nAPersonYouMightKnow",
                    Thumbnail = new Uri("ms-appx:///SamplePages/Twitter Service/icon.png")
                },
            };
            this.InitializeComponent();
        }

        public ICollection<Email> Emails { get; set; }

        public void OnXamlRendered(FrameworkElement control)
        {
            // Transfer Data Context so we can access Emails Collection.
            control.DataContext = this;
        }

    }

    public class Email
    {
        public string From { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public Uri Thumbnail { get; set; }
    }
}
