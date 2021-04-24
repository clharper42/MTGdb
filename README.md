# MTGdb
A visualtion of my other MTGdb using WPF

This program is a way to create and interact with your personal Magic The Gathering card collection.

First you scan cards using the TCGPlayer app as a way to speed up the process of entering cards into the database. You take the csv file that the app gives you and pass that in as an argument into the program.

Once the program has processed the cards it uses the information gathered to make a request to the [Scryfall API](https://scryfall.com/docs/api) to pull more information about the cards that the TCGPlayer file does not provide.

With the infromation gathered from Scryfall the program provides features such as searching, filtering and list creation.

Once the program has gathered information from Scryfall the program updates a csv file with the cards' information for which it will pull from and update when the program is ran again.

Set - Collector Number - Printing(Normal/Foil) is the unique identifier for a card 

Some cards may have issues being entered due to TCGPlayer and Scryfall having different ways of identifying cards. Fixing when I notice a discrepancy

Non-english cards do not currently work

<b><h3>To Use:</h3></b>
Make sure in the directory where the application is you have a folder named "Files" with Carddb.csv and TCGplayer.csv inside. The TCGplayer file comes from the TCGplayer app and is used to read in new cards into the database. The Carddb file is where the card information is stored once you quit out of the applicaiton. If you want to have multiple databases simply save the carddb file in a diffrent directory and overwrite the carddb file in the "Files" folder with the desired database. Any lists created and printed will be stored in the "Files" folder.

[Requires this "SDK 3.1.113" to run. May fix issues where application opens but nothing happens](https://dotnet.microsoft.com/download/dotnet/3.1)

[.NET Downloads](https://dotnet.microsoft.com/download)
