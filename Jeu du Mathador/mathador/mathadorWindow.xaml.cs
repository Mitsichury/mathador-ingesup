using System.ComponentModel;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour mathadorWindow.xaml
    /// </summary>
    public partial class mathadorWindow
    {        
        public mathadorWindow()
        {
            InitializeComponent();
            Content = new LaunchMenu();
        }
    }
}
