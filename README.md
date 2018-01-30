## Why does SmashELO exist?
The request came to me from a friend, who lamented Super Smash Brother's lack of skill tracking like other games. With a game as competitive as Super Smash Brothers, the lack of skill rating makes it difficult to compare someone's skill with someone else. The lack of skill rating also hinders new player's ability to visualize their skill improvements. Lastly, it makes it harder to brag.

![Greetings!](http://i.imgur.com/paSxNK4.png)

## What it does
SmashELO is a essentially a database front end with special features built in. You can add players and matches manually, or you can load several at a time from a file. SmashELO also allows you to input JSON data from Challonge.com and will automatically add all participants and matches!

After that, SmashELO can calculate every player's Elo, using a simple, but effective algorithm based on the classic Elo rating system. 

## How I built it
SmashELO was made using C# in Monodevelop on Ubuntu. I chose C# because if its LINQ functionality makes handling data much easier.

![Load from Challonge!](http://i.imgur.com/VrnWYBT.gif)

## Challenges I ran into
The occasional bugs and glitches always present in development. However, one particular challenge was making sure I didn't get confused with all the levels of abstraction. I had to soft-restart the project once just because I got confused with my own code.

## Accomplishments that I'm proud of
I never worked with large amounts of data like this before, and I'm really glad how it turned out. SmashELO can handle hundreds of players and matches without seeming to break a sweat. I also never knew how Elo was calculated before this, so I am proud that the algorithm I implemented functions as intended.

![Sorting!](http://i.imgur.com/8F6Wm6v.gif)

## What I learned
I learned a lot about JSON, LINQ, and C# while making SmashELO. LINQ was something I really enjoyed learning about. 

## What's next for SmashELO
SmashELO is currently a CLI so a GUI is definitely one of the things I really want to implement. A GUI would make SmashELO more approachable and visually pleasing. Maybe in the future, SmashELO can become a true tournament manager!
