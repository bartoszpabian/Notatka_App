using System.Windows;

namespace Notatka_App
{
    public partial class AddNoteWindow : Window
    {
        public string NoteContent { get; private set; }

        public AddNoteWindow()
        {
            InitializeComponent();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            NoteContent = TxtContent.Text;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}