# LaNoria Game Design doc

current doc: <https://docs.google.com/presentation/d/1rs6DF-Tlf7_JsVmTLqjSrzwOyMAkfLNhHRZNmjcenrM/edit#slide=id.g1b0c938e790_0_10>

## Overview

This games talks about the story of La Noria, a center of cultural innovation born in 2013, in Malaga Spain. The game explains the impact of the association in the territory of Malaga. La Noria has been involved in many different projects in the last ten years, the game tries to expose all of the projects dividing them into [4 categories](#Projects).

## Platform

The game will be available for mobile iOS and Android. The game will be displayed only in landscape mode.

## Gameplay

The game is about placing tiles into a map (so basically a puzzle). Each tile represents a project of La Noria and the map is representing the Malaga region. The map is made by multiple hexagonal pieces (around 100 pieces). The tiles are composed by 2, 3 or 4 hexagons, each hexagon has a different color (the color represents the project [category](#Projects)).

![mobile-noria](https://user-images.githubusercontent.com/45659694/210342035-7995898b-dea7-46d1-b9fc-e47b2d97d4bf.png)

Here a description of basics interactions (refer to the image above):

- The player can select one of the project cards at the bottom of the screen, each of this cards represent a different project of La Noria. Once a card is selected the details of the project and the associated tiles are displayed in the right side of the screen
- Once a card is selected the player can read the info about the project or drag the project (group of tiles) into the map
- The player can place the project around the map by dragging it. When the drag is over the project is released on the map (the tile is snapped in the nearest position). The player can rotate the tile by tapping on it. Here there are two situations:
  - The tile is in a valid position (this should be notified with the correct feedbacks), without any obstacles or other intersecting tiles. A button will be displayed around the tile, by hitting it the player will confirm the placement. 
  - The tile is not in a valid position (this should be notified with the correct feedbacks), the player should change the position and/or rotation. The player can also discard the tile just by clicking on another card.
- When the placement is over new cards will be given to the player.

The target of the game is to maximaze the score with combo of colors.


## Projects

La Noria's projects are divided into 4 categories, for each category is associated a different color:

- Environment and climate change (Medioambiente y cambio climático) - green
- Equality (Igualdad) - purple
- Tech for people (Tecnología para las personas) - yellow
- Depopulation (Desplobación) - red

Each project will be displayed as a playable card in game. The project description and images are inside the card.

## Map

The map consists of multiple hexagons. There are 8 different regions and one special region (Malaga city). The special has a different tile from the others.
![immagine](https://user-images.githubusercontent.com/45659694/210346090-bdbedde2-f875-42dd-8f5b-10b2e9c2c304.png)

## Rules

(!) - NOT DEFINITIVE

- (!) Once a tile is placed the player will receive new cards, the old ones are discarded
- Piece of maps covered with tiles of the same color increase the score
- If a region, [Map section for more details](#Map), presents all the colors within its borders the score is increased
