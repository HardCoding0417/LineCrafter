using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TranslationLine> lines = new ObservableCollection<TranslationLine>();

    public IRelayCommand LoadFileCommand { get; }

    public MainViewModel()
    {
        LoadFileCommand = new RelayCommand(async () => await LoadFileAsync());
    }

    private async Task LoadFileAsync()
    {
        try
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.iOS, new[] { "public.plain-text" } }, // iOS
            { DevicePlatform.Android, new[] { "text/plain" } },    // Android
            { DevicePlatform.WinUI, new[] { ".txt" } }             // Windows
        });

            var options = new PickOptions
            {
                PickerTitle = "Select a TXT File",
                FileTypes = customFileType
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var reader = new StreamReader(stream);

                Lines.Clear();
                int lineNumber = 1;
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    Lines.Add(new TranslationLine
                    {
                        LineNumber = lineNumber++,
                        Content = line
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

}

