using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using Microsoft.Win32;

namespace Notatka_App
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();
        private const string DefaultFileName = "notes.json";

        public MainWindow()
        {
            InitializeComponent();
            NotesListBox.ItemsSource = Notes;
            LoadDefaultNotes();
        }

        // Sprawdza czy plik istnieje przy starcie i wczytuje dane
        private void LoadDefaultNotes()
        {
            if (File.Exists(DefaultFileName))
            {
                try
                {
                    string json = File.ReadAllText(DefaultFileName);
                    var loadedNotes = JsonSerializer.Deserialize<ObservableCollection<Note>>(json);
                    if (loadedNotes != null)
                    {
                        Notes.Clear();
                        foreach (var note in loadedNotes)
                        {
                            Notes.Add(note);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas wczytywania notatek: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Otwiera okno dodawania nowej notatki
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddNoteWindow();
            if (addWindow.ShowDialog() == true)
            {
                if (!string.IsNullOrWhiteSpace(addWindow.NoteContent))
                {
                    Notes.Add(new Note { Content = addWindow.NoteContent });
                    AutoSaveDefault(); // Automatyczny zapis stanu
                }
            }
        }

        // Zapisuje listę do wybranego przez użytkownika pliku (SaveFileDialog) w formacie JSON
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki JSON (*.json)|*.json|Wszystkie pliki (*.*)|*.*";
            saveFileDialog.DefaultExt = "json";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = JsonSerializer.Serialize(Notes, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(saveFileDialog.FileName, json);
                    MessageBox.Show("Notatki zostały pomyślnie zapisane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas zapisu pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Pomocnicza metoda do autozapisu w domyślnym pliku
        private void AutoSaveDefault()
        {
            try
            {
                string json = JsonSerializer.Serialize(Notes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DefaultFileName, json);
            }
            catch (Exception)
            {
                // Ignorujemy błędy w tle
            }
        }
    }
}