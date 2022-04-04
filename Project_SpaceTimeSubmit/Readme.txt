WASD and mouse movement for flying around. This was code from Github 
>> https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996 <<

Keys 1 and 2 switches between two different visualizations (who won vs the proportion of each party).

Voter Data from >> https://dataverse.harvard.edu/dataset.xhtml?persistentId=doi:10.7910/DVN/42MVDX << U.S. President 1976–2016

I trimmed the data using python (check the data folder) keeping the year, state, party, votes,
and total votes. I created a trimmed file with that subset and used that for my visualization.

I organized the data into a dictionary containing data for each state for each year.
I stored the votes for democrats and republicans across the state and made cube primitives
with colors representing the proportion of votes. The cube primitives were placed on top of
their respective states moving farther and farther away as the year increases.
