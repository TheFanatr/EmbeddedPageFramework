using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmbeddedPageFramework.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Chicken.Click += (s, e) => MessageBox.Show("He likes trains.");

            Button ShowModalDialogButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Center,
                Height = 65,
                Content = "page 260"
            };
            ShowModalDialogButton.Click += (s, e) => PageContainer.LoadedPage = new EmbeddedPage { Children = { new Label { Content = "Modal Memse to ressq", Margin = new Thickness(10) } } };

            PageContainer.PageQueue.Add(new EmbeddedPage
            {
                Children =
                {
                    new Button
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        Height = 60,
                        Content = "Woooa broa",
                        Margin = new Thickness(10)
                    },
                    new Label
                    {
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(20)
                    }
                }
            });
            PageContainer.PageQueue.Add(new EmbeddedPage
            {
                Children =
                {
                    ShowModalDialogButton,
                    new Label
                    {
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(20),
                        Content = "As, teh button said (as in kinda implied), page 2, this one is."
                    }
                }
            });
            PageContainer.PageQueue.Add(new EmbeddedPage
            {
                Children =
                {
                    new Button
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        Height = 10,
                        Content = "Henyuh-ha!",
                        Margin = new Thickness(10)
                    },
                    new Label
                    {
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(20),
                        Content = "Pagina trei"
                    }
                }
            });

            ReverseQueueButton.Click += (s, e) => PageContainer.ReverseQueue();
            DismissUnqueuedPageButton.Click += (s, e) => PageContainer.DismissUnqueuedPage();
            AdvanceQueueButton.Click += (s, e) => 
            PageContainer.AdvanceQueue();
        }
    }
}
