# Padel Info - Alexa Skill

## Demo URL

[https://www.amazon.co.uk/Marcos-Gimeno-Padel-info/dp/B07ZZ9CDZ6](https://www.amazon.co.uk/Marcos-Gimeno-Padel-info/dp/B07ZZ9CDZ6)

Otherwise you can install this skill on the Amazon Alexa mobile app by searching the skill 'Padel info'

## What is this?

Alexa skill that provides info about Padel rankings &amp; tournaments

After installing the Alexa Skill you can open the skill by saying:

"Alexa, open padel info"

Then you can ask something like:

- "who is the number one?"

- "when is the next World Padel Tour tournament?"

- "who are the top three ladies on the LTA padel Tour?"

Available rankings:

- World Padel Tour - (categories: male, female)

- LTA Padel Tour - formerly known as British Padel Tour - (categories: male, female, senior male, senior female, junior)

## Development

This skill was developed by using the Amazon Alexa Developer console. It uses AWS Lambda and Node.js

However, because the rankings and tournaments data could not be fetched fast enough to return a quick response, I developed a parser and cache to store and then provide that data. This was done using .NET Core 3 and python. 
