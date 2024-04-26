## Project Name: Two-Player Board Games Framework

### Description:
This project involves the design and implementation of a modular, extensible framework for two-player board games. It supports a variety of games, including SOS and Connect Four, with capabilities for both human vs. human and human vs. computer gameplay. The framework includes an abstract class for general board game mechanisms and an interface for the game UI, facilitating easy integration and scalability of new games.

### Technologies Used:
Java
Object-Oriented Programming Principles
Design Patterns (Strategy, Memento, Singleton)

### Key Features:
Extensible architecture allowing for the addition of new board games without modifying existing code.
Support for multiple game modes, including human vs. computer, leveraging a simple AI.
GameState management for features like undo and redo, although these are partially implemented.
A console interface that provides a user-friendly way to interact with different games and settings.

### Challenges Faced:
Implementing a robust system for game state management to support undo and redo functionalities.
Developing a user-friendly console interface that accommodates various types of games and player interactions.
Ensuring the framework’s extensibility while maintaining a clean and manageable codebase.

### Outcome/Results:
Successfully implemented two games: SOS and Connect Four, with the foundational setup to add more games.
Established a flexible game management system that can handle game lifecycles, including starting, playing, and ending games, as well as managing player turns.
Laid the groundwork for advanced features like game state saving and loading, which are planned for future iterations.
