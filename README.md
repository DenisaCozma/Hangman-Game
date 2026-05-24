![header](https://capsule-render.vercel.app/api?type=waving&color=0:2E0249,50:A91079,100:F806CC&height=230&section=header&text=Hangman%20Game&fontSize=55&fontColor=ffffff&animation=fadeIn&fontAlignY=38&desc=C%23%20WPF%20Desktop%20Word%20Guessing%20Game&descAlignY=58&descSize=20)

<div align="center">

![C#](https://img.shields.io/badge/C%23-100%25-682BD7?style=for-the-badge&logo=csharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-Desktop%20App-A91079?style=for-the-badge&logo=windows&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-Windows-2E0249?style=for-the-badge&logo=dotnet&logoColor=white)
![Status](https://img.shields.io/badge/Status-In%20Development-F806CC?style=for-the-badge)

</div>

## About

**Hangman Game** is a desktop word-guessing game built with **C#** and **WPF**.

The player must guess the hidden word letter by letter before the hangman drawing is completed.  
The game uses different word categories loaded from a JSON file, making each round more varied and fun.

---

## ✨ Features

- Classic Hangman gameplay
- Multiple word categories
- Letter guessing system
- Hangman image progression
- Words loaded from `Words.json`
- WPF desktop interface
- MVVM-style project structure

---

## 🕹️ How to Play

1. Start the game.
2. A hidden word is selected from a category.
3. Guess letters one by one.
4. Correct letters are revealed in the word.
5. Wrong guesses advance the hangman drawing.
6. Win by guessing the word before the drawing is completed.

---

## 🛠️ Built With

<div align="center">

| Technology | Purpose |
|---|---|
| **C#** | Main programming language |
| **WPF** | Desktop user interface |
| **XAML** | UI layout |
| **JSON** | Word storage |
| **.NET** | Application platform |

</div>

---

## 📁 Project Structure

```text
Hangman-Game/
│
├── Commands/        # UI command classes
├── Models/          # Game models and logic
├── Pictures/        # Interface images
├── Spanzurat/       # Hangman drawing assets
├── ViewModels/      # ViewModel logic
├── Views/           # WPF views
│
├── App.xaml
├── App.xaml.cs
├── Spanzuratoare.csproj
└── Words.json       # Word categories and words
```

---

## 🚀 How to Run

```bash
git clone https://github.com/DenisaCozma/Hangman-Game.git
```

Then:

1. Open the project in **Visual Studio**.
2. Open the `.sln` or `.csproj` file.
3. Build the project.
4. Run the application.

---

## 🔮 Future Improvements

- Add difficulty levels
- Add score tracking
- Improve animations
- Add sound effects
- Add a final win / lose screen
- Add more word categories

<div align="center">

</div>

![footer](https://capsule-render.vercel.app/api?type=waving&color=0:F806CC,50:A91079,100:2E0249&height=120&section=footer)
