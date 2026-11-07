using BullsAndCowsApp.Models;  // BullsAndCowsGame, GuessResult
using System;
using System.Collections.Generic;
using System.ComponentModel;   // INotifyPropertyChanged
using System.Windows.Input;    // ICommand

namespace BullsAndCowsApp.ViewModels
{
    // =====================================================================
    // GameViewModel - the "middle man" between MainPage (View) and the Models.
    // It has no buttons or labels of its own. It only keeps data that the
    // page shows, and methods that the buttons call.
    // The game rules stay in BullsAndCowsGame and GuessResult.
    // =====================================================================
    public class GameViewModel : INotifyPropertyChanged
    {
        // ----- INotifyPropertyChanged -----

        // The page listens to this event. When we raise it with a property
        // name, the page reads that property again and updates the screen.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Call this every time a property changes.
        // nameof(Bulls) is just the text "Bulls", but the compiler checks the spelling.
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // ----- Private fields -----

        private BullsAndCowsGame _game;     // the Model: secret number, checks, scoring
        private int _bestScore = 0;         // fewest attempts in a win (0 = no win yet), like in Program.cs

        // ----- Constructor -----

        public GameViewModel()
        {
            // The three levels shown in the Picker. Easy = 4, Medium = 5, Hard = 6 digits.
            _difficulties = new List<string>();
            _difficulties.Add("Easy (4 digits)");
            _difficulties.Add("Medium (5 digits)");
            _difficulties.Add("Hard (6 digits)");

            // Create the commands. Each one calls one method of this ViewModel.
            _submitGuessCommand = new GuessCommand(this);
            _newGameCommand = new RestartCommand(this);
            _newNumberCommand = new NumberCommand(this);

            // Start with Easy. We set the field (not the property) so the game starts only once.
            _selectedDifficulty = _difficulties[0];
            _game = new BullsAndCowsGame(4);
            StartNewGame();
        }

        // =================================================================
        // Properties that MainPage.xaml binds to
        // =================================================================

        // the generated number from the model function.
        private int _generatedNumber;
        public int GeneratedNumber
        {
            get 
            {
                return _generatedNumber;
            }

            set 
            {
                _generatedNumber = value;
                OnPropertyChanged(nameof(GeneratedNumber));
            }
        }

        // "Welcome! Guess the 4 digit number" - the number changes with the difficulty.
        private string _welcomeMessage = "";
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        // The items of the Picker. This list never changes, so it has no setter.
        private List<string> _difficulties;
        public List<string> Difficulties
        {
            get { return _difficulties; }
        }

        // The level the player chose in the Picker.
        // Choosing a level starts a new game, like the console app did
        // (there you chose the difficulty only when a new game started).
        private string _selectedDifficulty = "";
        public string SelectedDifficulty
        {
            get { return _selectedDifficulty; }
            set
            {
                _selectedDifficulty = value;
                OnPropertyChanged(nameof(SelectedDifficulty));
                StartNewGame();
            }
        }

        // How many digits to guess (4, 5 or 6). The Entry uses it as MaxLength.
        private int _codeLength;
        public int CodeLength
        {
            get { return _codeLength; }
            set
            {
                _codeLength = value;
                OnPropertyChanged(nameof(CodeLength));
            }
        }

        // Number of valid guesses in this game (the "Attempts" box).
        private int _attempts;
        public int Attempts
        {
            get { return _attempts; }
            set
            {
                _attempts = value;
                OnPropertyChanged(nameof(Attempts));
            }
        }

        // Text of the "Best Score" box: "-" until the first win, then a number.
        private string _bestScoreText = "-";
        public string BestScoreText
        {
            get { return _bestScoreText; }
            set
            {
                _bestScoreText = value;
                OnPropertyChanged(nameof(BestScoreText));
            }
        }

        // What the player typed in the guess Entry (two-way binding).
        private string _guessText = "";
        public string GuessText
        {
            get { return _guessText; }
            set
            {
                _guessText = value;
                OnPropertyChanged(nameof(GuessText));
            }
        }

        // Bulls of the last valid guess.
        private int _bulls;
        public int Bulls
        {
            get { return _bulls; }
            set
            {
                _bulls = value;
                OnPropertyChanged(nameof(Bulls));
            }
        }

        // Cows of the last valid guess.
        private int _cows;
        public int Cows
        {
            get { return _cows; }
            set
            {
                _cows = value;
                OnPropertyChanged(nameof(Cows));
            }
        }

        // The red error text. Empty text means "no error".
        private string _errorMessage = "";
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasError));   // HasError depends on ErrorMessage
            }
        }

        // True when there is an error to show. It is calculated from ErrorMessage,
        // so it has no setter. The ErrorMessage setter tells the page it changed.
        public bool HasError
        {
            get { return ErrorMessage != ""; }
        }

        // The message shown after a win.
        private string _winMessage = "";
        public string WinMessage
        {
            get { return _winMessage; }
            set
            {
                _winMessage = value;
                OnPropertyChanged(nameof(WinMessage));
            }
        }

        // True after the player wins. It shows the win message.
        private bool _isGameOver;
        public bool IsGameOver
        {
            get { return _isGameOver; }
            set
            {
                _isGameOver = value;
                OnPropertyChanged(nameof(IsGameOver));
                OnPropertyChanged(nameof(CanGuess));   // CanGuess depends on IsGameOver
            }
        }

        // True while the player may still guess. It enables the Entry and the Submit button.
        // It is the opposite of IsGameOver, so it has no setter.
        public bool CanGuess
        {
            get { return !IsGameOver; }
        }

        // ----- Commands (the buttons call these) -----
        //Genrate a number button -> GenerateNumber()
        private ICommand _newNumberCommand;
        public ICommand NewNumberCommand
        {
            get { return _newNumberCommand; }
        }


        // Submit button and the Enter key -> SubmitGuess()
        private ICommand _submitGuessCommand;
        public ICommand SubmitGuessCommand
        {
            get { return _submitGuessCommand; }
        }

        // New Game button -> NewGame()
        private ICommand _newGameCommand;
        public ICommand NewGameCommand
        {
            get { return _newGameCommand; }
        }

        // =================================================================
        // Methods called by the commands
        // =================================================================


        //Gets the generated number
        public void GetNewNumber()
        {
            _game.GenerateNumber();
            GeneratedNumber = _game.GeneratedNumber;
        }

        // Checks the guess and shows the result. Same steps as the loop in Program.cs.
        public void SubmitGuess()
        {
            string guess = GuessText;

            // Empty input: do nothing (the console app also ignored it).
            if (string.IsNullOrEmpty(guess))
            {
                return;
            }

            // No more guesses after a win.
            // (The button is already disabled, but we check here too, to be safe.)
            if (_game.IsGameOver)
            {
                ErrorMessage = "The game is already over. Start a new game.";
                return;
            }

            // Use the Model's rules to check the guess. If it is wrong, show its error message.
            string errorMessage;
            if (!BullsAndCowsGame.IsValidGuess(guess, _game.CodeLength, out errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            // The guess is valid: remove an old error and let the Model score it.
            ErrorMessage = "";
            GuessResult result = _game.SubmitGuess(guess);

            Bulls = result.Bulls;
            Cows = result.Cows;
            Attempts = _game.AttemptCount;

            if (result.IsWinningGuess)
            {
                ShowWin();
            }
        }

        // New Game button: a new secret number with the level chosen in the Picker.
        public void NewGame()
        {
            StartNewGame();
        }

        // =================================================================
        // Helper methods (used only inside this class)
        // =================================================================

        // Starts a game with the chosen level and clears the screen.
        // The best score is NOT reset - it is kept for the whole session.
        private void StartNewGame()
        {

            CodeLength = GetCodeLength();
            _game.StartNewGame(CodeLength);

            WelcomeMessage = "Welcome! Guess the " + CodeLength + " digit number";
            Attempts = 0;
            Bulls = 0;
            Cows = 0;
            GuessText = "";
            ErrorMessage = "";
            WinMessage = "";
            IsGameOver = false;
        }

        // Turns the chosen Picker item into a code length.
        // Item 0 (Easy) -> 4, item 1 (Medium) -> 5, item 2 (Hard) -> 6.
        private int GetCodeLength()
        {
            int index = _difficulties.IndexOf(_selectedDifficulty);
            if (index == -1)
            {
                return 4;   // nothing chosen: use Easy
            }
            return index + 4;
        }

        // Builds the win message and updates the best score (same rules as Program.cs).
        private void ShowWin()
        {
            int attempts = _game.AttemptCount;
            string message = "Correct! You guessed the secret number in " + attempts + " attempts.";

            // A new best score? "New record achieved!" only if there was an old record to beat.
            if (_bestScore == 0 || attempts < _bestScore)
            {
                if (_bestScore != 0)
                {
                    message = message + "\nNew record achieved!";
                }
                _bestScore = attempts;
                BestScoreText = _bestScore.ToString();
            }

            message = message + "\nYour current best score is " + _bestScore + " attempts.";

            WinMessage = message;
            IsGameOver = _game.IsGameOver;   // true now: Submit is disabled and the win message appears
        }
    }

    // =====================================================================
    // Command classes.
    // A Button's Command must be an object that implements ICommand.
    // Each class below calls one method of the ViewModel.
    // =====================================================================

    public class NumberCommand : ICommand
    {
        private GameViewModel _viewModel;

        public NumberCommand(GameViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.GetNewNumber();
        }
    }


    // Used by SubmitGuessCommand: calls SubmitGuess().
    public class GuessCommand : ICommand
    {
        private GameViewModel _viewModel;

        public GuessCommand(GameViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        // Required by ICommand. It tells the button to ask CanExecute again.
        public event EventHandler? CanExecuteChanged;

        // We never need it (IsEnabled is bound to CanGuess instead),
        // but this method uses the event so the compiler does not warn about it.
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        // Can the command run now? Always yes.
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        // What happens when the button is clicked.
        public void Execute(object? parameter)
        {
            _viewModel.SubmitGuess();
        }
    }

    // Used by NewGameCommand: calls NewGame().
    public class RestartCommand : ICommand
    {
        private GameViewModel _viewModel;

        public RestartCommand(GameViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        // Required by ICommand (see GuessCommand above).
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        // The New Game button always works.
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.NewGame();
        }
    }
}
