I downloaded the two CSV files and normalized the data using python. When mapping the data I used the date columns as the x-axis, hospital visits as y-axis, and Grocery store visits on the z-axis.
I colored them using the first two digit of the FIPS value so that places close in geographical location are colored the same. I generated a new object for
each line I wanted to draw and attached a line component to it. I then used the vertices I generated from the data and added those to the line render component. 

To utilized transformations, I created a matMult function to perform my multiplications. I used translation rotation and scaling on the data to get it in the viewport without moving the camera. 
I also separated out the different colors because it was a little hard to follow with so many lines.

My visualization does not look that great but it appears that larger grocery store visits correspond with some of the highest hospital visits. This could also be the 
result of an increased population for that segment of data. I was planning on controlling for that but ran into an issue that I did not have time to deal with before needing to submit the project.