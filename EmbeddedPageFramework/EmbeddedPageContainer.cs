using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EmbeddedPageFramework
{
    /// <summary>
    /// Interaction logic for EmbeddedPageContainer.xaml
    /// </summary>
    public partial class EmbeddedPageContainer : UserControl
    {
        static bool EnableKeyboardShortcutsValue;

        public bool EnableKeyboardShortcuts
        {
            get => EnableKeyboardShortcutsValue;
            set
            {
                if (EnableKeyboardShortcutsValue != value)
                {
                    if (value)
                        Window.GetWindow(this).KeyDown += HandleShortcuts;
                    else
                        Window.GetWindow(this).KeyDown -= HandleShortcuts;
                    EnableKeyboardShortcutsValue = value;
                }
            }
        }

        static bool EnableKeyboardShortcutsToggleShortcutValue;

        public bool EnableKeyboardShortcutsToggleShortcut
        {
            get => EnableKeyboardShortcutsToggleShortcutValue;
            set
            {
                if (EnableKeyboardShortcutsToggleShortcutValue != value)
                {
                    if (value)
                        Window.GetWindow(this).KeyDown += HandleShortcutToggle;
                    else
                        Window.GetWindow(this).KeyDown -= HandleShortcutToggle;
                    EnableKeyboardShortcutsToggleShortcutValue = value;
                }
            }
        }

        void HandleShortcutToggle(object sender, KeyEventArgs e) { if (e.KeyboardDevice.IsKeyDown(Key.LeftAlt) && e.KeyboardDevice.IsKeyDown(Key.LeftShift) && e.KeyboardDevice.IsKeyDown(Key.T)) EnableKeyboardShortcuts = !EnableKeyboardShortcutsValue; }

        void HandleShortcuts(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                if (e.KeyboardDevice.IsKeyDown(Key.Left))
                    ReverseQueue();
                if (e.KeyboardDevice.IsKeyDown(Key.Right))
                    AdvanceQueue();
                if (e.KeyboardDevice.IsKeyDown(Key.Enter))
                    DismissUnqueuedPage();
            }
            else if (e.KeyboardDevice.IsKeyDown(Key.LeftAlt))
            {
                if (e.KeyboardDevice.IsKeyDown(Key.Left))
                    ReverseHistory();
                if (e.KeyboardDevice.IsKeyDown(Key.Right))
                    AdvanceHistory();
            }
        }

        public EmbeddedPageContainer()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                //InitializeComponent();
                Loaded += (s, e) =>
                {
                    EnableKeyboardShortcuts = true;
                    EnableKeyboardShortcutsToggleShortcut = true;
                    LoadedPage = LoadedPage; // DO NOT DELETE
                    StoredPage = LoadedPage;
                };
            }
        }

        EmbeddedPage StoredPage;

        public EmbeddedPage LoadedPage
        {
            get => Content as EmbeddedPage;
            set
            {
                if (!PageQueue.Exists(x => x.Identifier == value.Identifier))
                    StoredPage = LoadedPage;
                Content = value;
                if (PageHistoryIndex != PageHistory.Count - 1)
                    PageHistory.RemoveRange(PageHistoryIndex + 1, PageHistory.Count - 1 - PageHistoryIndex);
                PageHistoryIndexValue++;
                PageHistory.Add(value);
            }
        }

        public List<EmbeddedPage> PageQueue { get; set; } = new List<EmbeddedPage>();

        public int PageQueueIndex
        {
            get => PageQueue.FindIndex(x => x.Identifier == LoadedPage.Identifier);
            set => LoadedPage = PageQueue.ElementAt(value);
        }

        public void AdvanceQueue()
        {
            if (PageQueueIndex == -1 && PageQueue.Count >= 1)
            {
                var QueuedStoredPage = PageQueue.Find(x => x.Identifier == StoredPage.Identifier);
                if (StoredPage != null && QueuedStoredPage != null)
                {
                    var ModifiedPageQueueIndex = PageQueue.FindIndex(x => x.Identifier == StoredPage.Identifier) + 1;
                    if (ModifiedPageQueueIndex <= PageQueue.Count - 1)
                        PageQueueIndex = ModifiedPageQueueIndex;
                }
                else
                    PageQueueIndex = 0;
            }
            else if (PageQueue.Count > 1 && PageQueueIndex < PageQueue.Count - 1)
                PageQueueIndex++;
        }

        public void ReverseQueue()
        {
            if (PageQueueIndex == -1 && PageQueue.Count >= 1)
            {
                var QueuedStoredPage = PageQueue.Find(x => x.Identifier == StoredPage.Identifier);
                if (StoredPage != null && QueuedStoredPage != null)
                {
                    var ModifiedPageQueueIndex = PageQueue.FindIndex(x => x.Identifier == StoredPage.Identifier) - 1;
                    if (ModifiedPageQueueIndex >= 0)
                        PageQueueIndex = ModifiedPageQueueIndex;
                }
                else
                    PageQueueIndex = PageQueue.Count - 1;
            }
            else if (PageQueue.Count > 1 && PageQueueIndex > 0)
                PageQueueIndex--;
        }

        public void DismissUnqueuedPage()
        {
            if (PageQueueIndex == -1 && PageQueue.Count >= 1)
            {
                var QueuedStoredPage = PageQueue.Find(x => x.Identifier == StoredPage.Identifier);
                if (StoredPage != null && QueuedStoredPage != null)
                {
                    var ModifiedPageQueueIndex = PageQueue.FindIndex(x => x.Identifier == StoredPage.Identifier);
                    if (ModifiedPageQueueIndex <= PageQueue.Count - 1)
                        PageQueueIndex = ModifiedPageQueueIndex;
                }
                else
                    PageQueueIndex = 0;
            }
        }

        EmbeddedPage LoadedHistoryPage
        {
            get => Content as EmbeddedPage;
            set
            {
                if (!PageQueue.Exists(x => x.Identifier == value.Identifier))
                    StoredPage = LoadedHistoryPage;
                Content = value;
            }
        }

        public List<EmbeddedPage> PageHistory = new List<EmbeddedPage>();

        int PageHistoryIndexValue = -1;

        public int PageHistoryIndex
        {
            get => PageHistoryIndexValue;
            set
            {
                LoadedHistoryPage = PageHistory.ElementAt(value);
                PageHistoryIndexValue = value;
            }
        }

        public void AdvanceHistory() { if (PageHistory.Count >= 1 && PageHistoryIndex < PageHistory.Count - 1) PageHistoryIndex++; }

        public void ReverseHistory() { if (PageHistory.Count >= 1 && PageHistoryIndex > 0) PageHistoryIndex--; }
    }
}
