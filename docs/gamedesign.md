# LaNoria Game Design doc

[current google doc](https://docs.google.com/document/d/1SPc61o-s_OPw4w89VVaIKAdP1lstlgJqu2hLmYhbrAM/edit#)

## Overview

This game tells the story of La Noria ( https://www.malaga.es/lanoria/ ), a center of cultural innovation born in 2013, in Malaga, Spain. The game explains the impact of the association in the territory of Malaga. La Noria has been involved in many different projects in the last ten years, the game tries to expose many of the projects dividing them into 4 categories.

## Platform

The game will be available for mobile iOS and Android. The game will be displayed only in landscape mode. Could be published on the web.

## Features

- The game is in Spanish only.
- Should be playable with just 1 finger interaction (to make it easy a PC/web port)
- The target player is a non gamer, so let’s make it easy to read and play.
- Let’s take in considerations accessibility (colors mostly and readability)
- The game will be free and open source

## Home

In home we have just 4 buttons:

### About La Noria
An introduction to what it is and what it does, text and images.
Then a list/book of all the available projects-cards in the game (title + photo + details)

### Play
To play a game. You can play as many times as you want to improve your high-score.

### Options

- toggle sfx
- toggle music / volume
- credits

## Gameplay

The game is about selecting and placing projects into the map of Malaga province. The map is made of multiple cells (hexagonal pieces). The projects are composed of 2, 3 or 4 cells with different colors (the color represents a category).

![mobile-noria](https://user-images.githubusercontent.com/45659694/210342035-7995898b-dea7-46d1-b9fc-e47b2d97d4bf.png)

Here a description of basics interactions (refer to the image above):

- The player can select one of the project cards at the bottom of the screen. Once a project is selected its details and the associated tiles are displayed in the right side of the screen
- We can read the info about the project or drag the project into the map.
- The player can place the project around the map by dragging it and releasing it wherever on the map (the tile is snapped in the nearest position). The player can rotate the project by tapping on it. Here there are two situations:
  - The project is in a valid position (this should be notified with the correct feedback), without any obstacles or other intersecting tiles. A button will be displayed to confirm the placement.
  - The tile is not in a valid position (this should be notified with the correct feedback), the player should change the position and/or rotation. The player can also discard the tile just by clicking on another project card.
- When the placement is done, new project cards will be given to the player.

Repeat until no more projects can be put in the map.

The goal of the game is to maximize the score.


## Score

The score is calculated by adding points every time a project is placed & confirmed… and at the end of the game when some bonuses are computed.

### Placing a project (Basic Points)
Any project placed on the map gives:

- 2 tiles project: 4 points
- 3 tiles project: 8 points
- 4 tiles project: 12 points

These points are added to the score as soon as a project is confirmed on the map.

### Color matching (Synergy Bonus)
Adjacent cells of the same color give 1 bonus point for each cell adjacent to the project cells.  
NOTE: only the cells adjacent to the placed project give bonus points, large groups of cells of the same colors doe not give extra bonus.  
These points are added to the score as soon as a project is confirmed on the map.

### All colors in an area (Transversality Bonus)
Placing all 4 colors in a subregion (Comarca) gives an extra bonus (i.e.. 20 points to be sure it’s always a priority over the Synergy bonus).  
These points could be added to the score as soon as a project is confirmed on the map or at the end of the game.

### End of game review (Final Score computation)
At the end of the game (when no more projects can be placed on the map), the game automatically review the full board and it may give some additional bonuses/maluses:

- A malus for each empty cell left on the map (i.e. -3 points)
- A major bonus if the map has been fully covered (i.e. 50 points)

The game will display the Final Score and it may also display a summary of the different voices, like in the example below:

- Cells/Hexagons: 56
- Synergy bonus: 105
- Transversality bonus: 120 (6x20)
- Empty cells: -21 (-3x7)
- FINAL SCORE: 260

## Projects

La Noria's projects are divided into 4 categories:

1. Environment and climate change (Medioambiente y cambio climático) - 00C087
2. Equality (Igualdad) - purple - 8123FF
3. Tech for people (Tecnología para las personas) - FF8905
4. Depopulation (Despoblación) - FF40C5

Each project will be displayed as a playable card in game.  
The project description and cover image are accessible from the card.  
The Projects database live in a Google Sheet where the content editor can define:

- Id
- Number of tiles or Tiles-combo id (a list of the 10 possible configurations)
- Color sequence (1,2,3,4)
- Title
- Description
- Image (?)

## Map

The map consists of multiple hexagons. There are 8 different regions and one special region (Malaga city). The special has a different tile from the others.
![immagine](https://user-images.githubusercontent.com/45659694/210346090-bdbedde2-f875-42dd-8f5b-10b2e9c2c304.png)

## Rules
NOT DEFINITIVE
Once a tile is placed the player will receive new cards, the old ones are discarded

## Analytics
Just basic analytics and a final game score result.

