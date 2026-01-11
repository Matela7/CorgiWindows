# 🐕 Corgi Focus

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)
![WinUI 3](https://img.shields.io/badge/WinUI-3-purple)
![Windows](https://img.shields.io/badge/Windows-10%2B-0078D4)
![License](https://img.shields.io/badge/license-MIT-green)

**Corgi Focus** to aplikacja desktopowa dla systemu Windows, która pomaga utrzymać koncentrację podczas pracy blokując dostęp do rozpraszających stron internetowych. Gdy użytkownik próbuje odwiedzić zablokowaną stronę

---

## Spis treści

- [Funkcjonalności](#-funkcjonalności)
- [Wymagania systemowe](#-wymagania-systemowe)
- [Instalacja](#-instalacja)
- [Użytkowanie](#-użytkowanie)
- [Architektura projektu](#-architektura-projektu)
- [Struktura kodu](#-struktura-kodu)
- [Technologie](#-technologie)
- [Kompilacja z kodu źródłowego](#-kompilacja-z-kodu-źródłowego)
- [Jak to działa](#-jak-to-działa)
- [Konfiguracja](#-konfiguracja)
- [Znane ograniczenia](#-znane-ograniczenia)
- [Autor](#-autor)
- [Licencja](#-licencja)

---

## Funkcjonalności

### Tryb Focus
- **Włącz/wyłącz** tryb skupienia jednym przełącznikiem
- **Ciągłe monitorowanie** aktywnych okien przeglądarki
- **Natychmiastowa reakcja** na próbę otwarcia zablokowanej strony
- **Minimalistyczny interfejs** z pieskiem

### Zarządzanie blokadami
- **Dodawanie stron** do listy blokowanych
- **Usuwanie stron** z listy
- **Edycja istniejących** wpisów
- **Domyślne blokady**: YouTube i Facebook (przykładowe dane)
- **Wyszukiwanie częściowe**: blokada działa na podstawie fragmentu nazwy strony w tytule okna

### Interfejs użytkownika
- **Trzy widoki**:
  - **Focus**: Główny widok z przełącznikiem trybu
  - **Blocker**: Zarządzanie listą zablokowanych stron
  - **About**: Informacje o aplikacji i autorze
- **Nawigacja boczna** z ikonami
- **Ciemny motyw**: Elegancka kolorystyka (#0F0B12, #15101A, #9B84F7)
- **Responsywny design**: Płynne animacje i przejścia

---

## Wymagania systemowe

### Minimalne wymagania:
- **System operacyjny**: Windows 10 (wersja 1809 / build 17763) lub nowszy



## Użytkowanie

### Pierwsze uruchomienie

1. **Uruchom aplikację** z menu Start lub ikony na pulpicie
2. Zobaczysz widok **Focus**
3. Domyślnie zablokowane są strony: `youtube` i `facebook`

### Włączenie trybu Focus

1. W widoku **Focus** kliknij przełącznik **Focus Mode**
2. Gdy przełącznik jest **włączony** (niebieski), aplikacja monitoruje aktywne okna
3. Gdy próbujesz otworzyć zablokowaną stronę, automatycznie otwiera się https://corgiorgy.com
4. Aplikacja może być zminimalizowana - działa w tle

### Dodawanie stron do blokady

1. Kliknij ikonę **blokady** (🛡️) w bocznym menu
2. W polu tekstowym wpisz **nazwę strony** (np. `reddit`, `twitter`, `instagram`)
3. Kliknij przycisk **"Dodaj"**
4. Strona pojawi się na liście

**Ważne**: Wpisuj tylko nazwę domeny bez `https://` lub `www.` (np. `youtube` zamiast `https://www.youtube.com`)

### Usuwanie stron z blokady

1. Przejdź do widoku **Blocker**
2. Znajdź stronę na liście
3. Kliknij ikonę **❌** po prawej stronie wpisu
4. Strona zostanie usunięta

### Edycja wpisów

1. W widoku **Blocker** kliknij w pole tekstowe z nazwą strony
2. Edytuj tekst bezpośrednio
3. Zmiany są zapisywane automatycznie

### Informacje o aplikacji

1. Kliknij ikonę **informacji** (ℹ️) w bocznym menu
2. Zobacz dane o autorze i wersji aplikacji

---

## Architektura projektu

Aplikacja wykorzystuje architekturę **MVVM-lite** z wyraźnym podziałem na warstwy:

```
CorgiWindows/
├── Views (XAML)
│   └── MainWindow.xaml          # Interfejs użytkownika
├── ViewModels / Code-behind
│   └── MainWindow.xaml.cs       # Logika prezentacji
├── Models
│   ├── BlockedWebsite.cs        # Model danych zablokowanej strony
│   └── WebsiteBlockerModel.cs   # Model zarządzania listą
├── Services
│   └── WebsiteBlockerService.cs # Usługa monitorowania
└── Business Logic
    └── FocusSession.cs          # Zarządzanie sesją Focus
```

---

## Struktura kodu

### `MainWindow.xaml`
**Plik XAML** definiujący interfejs użytkownika:
- **Resources**: Style, kolory, pędzle
- **Grid Layout**: Dwukolumnowy układ (sidebar + content)
- **Three Views**: Focus, Blocker, About (przełączane przez `Visibility`)
- **Data Binding**: `{x:Bind}` dla wydajnego bindingu danych

**Główne sekcje**:
```xaml
<Grid Background="#0F0B12">
    <!-- Resources: Style NavButtonStyle, kolory -->
    
    <!-- Sidebar (Column 0): Nawigacja -->
    <Border>
        <StackPanel>
            <!-- 3 przyciski nawigacyjne -->
        </StackPanel>
    </Border>
    
    <!-- Content (Column 1) -->
    <Grid>
        <!-- FocusView: Maskota + Toggle -->
        <!-- BlockerView: Lista + Add -->
        <!-- AboutView: Info -->
    </Grid>
</Grid>
```

### `MainWindow.xaml.cs`
**Code-behind** z logiką prezentacji:

```csharp
public sealed partial class MainWindow : Window
{
    private readonly WebsiteBlockerModel _model;
    private readonly FocusSession _focusSession;
    public ObservableCollection<BlockedWebsite> BlockedSites { get; }
    
    // Inicjalizacja, seedowanie danych
    // Obsługa nawigacji
    // Obsługa toggle Focus Mode
    // CRUD dla zablokowanych stron
}
```

**Kluczowe metody**:
- `NavButton_Click()`: Przełączanie widoków
- `FocusToggle_Toggled()`: Start/stop sesji monitorowania
- `AddWebsite_Click()`: Dodawanie strony do blokady
- `RemoveWebsite_Click()`: Usuwanie strony

### `BlockedWebsite.cs`
**Model danych** reprezentujący zablokowaną stronę:

```csharp
public class BlockedWebsite
{
    public Guid Id { get; set; }           // Unikalny identyfikator
    public string Url { get; set; }        // Nazwa/URL strony
}
```

### `WebsiteBlockerModel.cs`
**Model zarządzający** kolekcją zablokowanych stron:

```csharp
public class WebsiteBlockerModel
{
    public List<BlockedWebsite> BlockedWebsites { get; set; }
    
    public void AddBlockedWebsite(string url)      // Dodaj z walidacją
    public void RemoveWebsite(Guid id)             // Usuń po ID
}
```

**Funkcjonalności**:
- Zarządzanie listą `List<BlockedWebsite>`
- Walidacja duplikatów (case-insensitive)
- Null checks

### `FocusSession.cs`
**Logika biznesowa** sesji skupienia:

```csharp
public class FocusSession
{
    private readonly WebsiteBlockerService _blockerService;
    public bool IsActive { get; private set; }
    
    public void StartStop()  // Toggle sesji
}
```

**Odpowiedzialności**:
- Przechowywanie stanu sesji (`IsActive`)
- Kontrola serwisu monitorowania
- Start/stop wątku monitorującego

### `WebsiteBlockerService.cs`
**Główny serwis** monitorowania przeglądarek:

```csharp
public class WebsiteBlockerService
{
    private Thread? _monitoringThread;
    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;
    
    // Windows API imports
    [DllImport("user32.dll")] GetForegroundWindow()
    [DllImport("user32.dll")] GetWindowText()
    
    public void RunThread()                // Start monitorowania
    public void StopThread()               // Stop monitorowania
    private void MonitorBrowsers()         // Pętla monitorująca
    private string GetActiveWindowTitle()  // Pobierz tytuł okna
    private bool IsBlockedWebsiteOpen()    // Sprawdź blokadę
    private void OpenCorgiOrgyPopup()      // Otwórz popup
}
```

**Kluczowe mechanizmy**:

1. **Wątek w tle** (`Thread`, `IsBackground = true`)
2. **Polling co 100ms**: Sprawdzanie aktywnego okna
3. **Windows API**: `GetForegroundWindow()` + `GetWindowText()`
4. **Porównanie stringów**: `Contains()` z `StringComparison.OrdinalIgnoreCase`
5. **Popup delay**: 500ms po wykryciu blokady (throttling)
6. **Graceful shutdown**: `CancellationToken` + `Join(1000)`

---

## Technologie

### Główne frameworki
- **WinUI 3**: Nowoczesny framework UI dla Windows
- **Windows App SDK 1.8**: Pakiet SDK dla aplikacji Windows


### Windows API
- `user32.dll`:
  - `GetForegroundWindow()`: Pobieranie aktywnego okna
  - `GetWindowText()`: Pobieranie tytułu okna
  - `GetWindowThreadProcessId()`: Identyfikacja procesu (niewykorzystane aktualnie)

### Języki i formaty
- **C# 12**: Najnowsze funkcje języka
- **XAML**: Definicja interfejsu użytkownika
---

## Kompilacja z kodu źródłowego

### Wymagania dla developerów:
- **Visual Studio 2022** (wersja 17.8 lub nowsza)
- **Workloads**:
  - .NET Desktop Development
  - Universal Windows Platform development
  - Windows Application Development
- **Windows 11 SDK** (10.0.26100 lub nowszy)
- **Git** (do klonowania repozytorium)

---

## Jak to działa

### Mechanizm monitorowania

1. **Start sesji Focus**:
   - Użytkownik włącza przełącznik `FocusToggle`
   - `FocusSession.StartStop()` wywołuje `WebsiteBlockerService.RunThread()`
   - Uruchamiany jest wątek w tle

2. **Pętla monitorująca** (co 100ms):
```csharp
while (!cancellationToken.IsCancellationRequested)
{
    string windowTitle = GetActiveWindowTitle();  // Windows API
    
    if (IsBlockedWebsiteOpen(windowTitle))
    {
        OpenCorgiOrgyPopup();  // Process.Start("https://corgiorgy.com")
        Thread.Sleep(500);     // Throttling
    }
    
    Thread.Sleep(100);  // Polling interval
}
```

3. **Pobieranie tytułu okna**:
```csharp
IntPtr handle = GetForegroundWindow();  // Uchwyt aktywnego okna
GetWindowText(handle, buffer, 256);     // Tytuł okna → buffer
```

4. **Sprawdzanie blokady**:
```csharp
foreach (var blockedSite in _model.BlockedWebsites)
{
    if (windowTitle.Contains(blockedSite.Url, OrdinalIgnoreCase))
        return true;  // ZABLOKOWANE!
}
```

5. **Reakcja na blokadę**:
   - Otwieranie https://corgiorgy.com w domyślnej przeglądarce
   - Delay 500ms zapobiega spamowaniu zakładek

### Synchronizacja danych

- `ObservableCollection<BlockedWebsite>` automatycznie aktualizuje UI
- Zmiany w `BlockedSites` są propagowane do `_model.BlockedWebsites`
- Two-way binding w `TextBox` dla edycji nazw stron

### Threading

- **UI Thread**: Interfejs użytkownika (WinUI 3)
- **Background Thread**: Monitorowanie okien (własny wątek)
- **Graceful Shutdown**: `CancellationToken` + `Thread.Join()`

---

## Konfiguracja

### Zmiana motywu kolorów

Edytuj plik `MainWindow.xaml`, sekcja `<Grid.Resources>`:

```xaml
<SolidColorBrush x:Key="BackgroundBrush" Color="#0F0B12" />  <!-- Ciemne tło -->
<SolidColorBrush x:Key="PanelBrush" Color="#15101A" />       <!-- Panele -->
<SolidColorBrush x:Key="AccentBrush" Color="#9B84F7" />      <!-- Akcent (fiolet) -->
<SolidColorBrush x:Key="MutedBrush" Color="#7E758A" />       <!-- Wyciszone -->
```

### Zmiana URL motywacyjnego

Edytuj `WebsiteBlockerService.cs`, metoda `OpenCorgiOrgyPopup()`:

```csharp
string url = "https://corgiorgy.com";  // Zmień na dowolny URL
```

### Zmiana interwału monitorowania

Edytuj `WebsiteBlockerService.cs`, metoda `MonitorBrowsers()`:

```csharp
Thread.Sleep(100);  // 100ms = 10 razy na sekundę
                    // Mniejsza wartość = szybsza reakcja, wyższe CPU
```


## Autor

**Michał Mateła**

- GitHub: [@Matela7](https://github.com/Matela7)
- Projekt: [CorgiWindows](https://github.com/Matela7/CorgiWindows)

