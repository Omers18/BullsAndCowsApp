# Bulls and Cows

A C# console implementation of the Bulls and Cows game.

The program generates a secret number with unique digits and allows the
player to submit guesses. Each valid guess receives Bulls and Cows feedback.

The player can choose between three difficulty levels:
- Easy - 4 digits
- Medium - 5 digits
- Hard - 6 digits

A difficulty level is selected at the beginning of the game and can be
selected again whenever a new game is started.

Supported commands:
- n / new - start a new game and choose a new difficulty
- q / quit / exit - quit the program

## Bonus Features

The following optional bonus features were added:

- Player name - the player enters their name and it is included in the
  victory message.
- Best score - the program remembers the lowest number of attempts needed
  to win during the current session.
- Difficulty levels - the player can play with a 4, 5, or 6 digit secret
  number.

## Changes from the specification

The following changes were made in order to support the optional bonus
features:

- `CodeLength`, originally defined as a constant with the value 4, was
  changed to a property with a private setter so that each game can use a
  length of 4, 5, or 6 digits.

- The `BullsAndCowsGame` constructor was changed from
  `BullsAndCowsGame()` to `BullsAndCowsGame(int codeLength)` so that the
  selected code length can be provided when the game is created.

- `StartNewGame()` was changed to `StartNewGame(int codeLength)` so that
  the player can select a new difficulty whenever a new game is started.

- `IsValidGuess(string guess, out string errorMessage)` was changed to
  `IsValidGuess(string guess, int codeLength, out string errorMessage)`
  so that guess validation can work with all three code lengths while the
  method remains static.

- `GuessResult` now receives the code length in its constructor and stores
  it internally. `IsWinningGuess` therefore checks whether the number of
  Bulls equals the selected code length instead of always checking for
  exactly 4 Bulls.

- The program currently rejects guesses that start with 0.

## Known Bugs

No known bugs at the time of submission.