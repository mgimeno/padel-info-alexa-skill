// This sample demonstrates handling intents from an Alexa skill using the Alexa Skills Kit SDK (v2).
// Please visit https://alexa.design/cookbook for additional examples on implementing slots, dialog management,
// session persistence, api calls, and more.
const Alexa = require('ask-sdk-core');
const http = require('http');

const apiUrl = "http://padel-info.marcosgimeno.com/api/";

const TranslationEnum = Object.freeze({ "WELCOME_MESSAGE": 1, "ERROR_MESSAGE": 2, "GOODBYE_MESSAGE": 3, "HELP_MESSAGE": 4 });

const translations =
    [
        {
            id: TranslationEnum.WELCOME_MESSAGE, values: ["Welcome to Padel info. I hold information about player rankings and tournament dates. I currently support these rankings: World Padel Tour and L T A Padel. Ask me something or ask me for help.",
                "Bienvenido a Pádel info. Tengo información sobre clasificaciones de jugadores y fechas de torneos. Actualmente tengo datos de estos rankings: World Padel Tour and L T A Padel. Pregúntame algo o pideme ayuda"]
        },
        { id: TranslationEnum.ERROR_MESSAGE, values: ["Sorry, there was a problem. Please try again in a few minutes", "Lo siento, ha habido un problema. Prueba de nuevo dentro de unos minutos"] },
        { id: TranslationEnum.GOODBYE_MESSAGE, values: ["Goodbye!", "Adiós!"] },
        {
            id: TranslationEnum.HELP_MESSAGE, values: ["These are some examples of questions that you can ask me. Who is the number one?. Who are the top three ladies on the World Padel Tour. When is the next L T A Padel tournament. I currently support these rankings, World Padel Tour and L T A Padel. What do you want to know?",
                "Estos son algunos ejemplos de preguntas que puedes hacerme. Quién es el número uno? Quienes son las tres mejores chicas del World Padel Tour? Cuándo es el próximo torneo del L T A Padel tour?. Actualmente tengo datos de estos rankings: World Padel Tour and L T A Padel. Qué quieres saber?"]
        }
    ];


const getTranslation = function (languageId, translationId) {
    const translation = translations.find(t => t.id === translationId);
    return translation.values[languageId];
}

const getUrlContent = function (url) {
    return new Promise((resolve, reject) => {
        const request = http.get(url, response => {
            response.setEncoding('utf8');

            let returnData = '';
            if (response.statusCode < 200 || response.statusCode >= 300) {
                return reject(new Error(`${response.statusCode}: ${response.req.getHeader('host')} ${response.req.path}`));
            }

            response.on('data', chunk => {
                returnData += chunk;
            });

            response.on('end', () => {
                resolve(returnData);
            });

            response.on('error', error => {
                reject(error);
            });
        });
        request.end();
    });
}

const getRankingCategoryId = function (languageId, rankingCategory) {

    rankingCategory = rankingCategory.toLowerCase();

    let result = 0;

    if (languageId == 0) {

        switch (rankingCategory) {
            case "male":
            case "boys":
            case "boy":
            case "men's":
            case "men":
            case "males":
                result = 0;
                break;
            case "female":
            case "girls":
            case "girl":
            case "ladies":
            case "lady":
            case "females":
                result = 1;
                break;
            case "senior male":
            case "senior men":
            case "senior males":
            case "senior men's":
                result = 2;
                break;
            case "senior female":
            case "senior ladies":
            case "senior lady":
            case "senior females":
                result = 3;
                break;
            case "junior":
            case "juniors":
                result = 4;
                break;
        }

    }
    else {

        switch (rankingCategory) {
            case "masculino":
            case "masculinos":
            case "chico":
            case "chicos":
            case "hombre":
            case "hombres":
                result = 0;
                break;
            case "femenino":
            case "femeninos":
            case "chica":
            case "chicas":
            case "mujer":
            case "mujeres":
                result = 1;
                break;
            case "masculino senior":
            case "chico senior":
            case "chicos senior":
            case "hombre senior":
            case "hombres senior":
                result = 2;
                break;
            case "femenino senior":
            case "chica senior":
            case "chicas senior":
            case "mujer senior":
            case "mujeres senior":
                result = 3;
                break;
            case "juvenil":
            case "juveniles":
            case "niño":
            case "niña":
            case "niños":
            case "niñas":
            case "joven":
            case "jovenes":
                result = 4;
                break;
        }

    }

    return result;

}

const getRankingTypeId = function (rankingType) {

    rankingType = rankingType.toLowerCase();

    let result = 0;

    switch (rankingType) {
        case "world padel tour":
        case "w. p. t.":
        case "w p t":
            result =  0;
            break;
        case "l. t. a. padel":
        case "british padel":
        case "padel u. k.":
        case "british padel tour":
        case "l t a padel tour":
        case "l t a padel":
        case "l. t. a":
        case "l. t. a. padel tour":
            result =  1;
            break;
    }

    return result;

}

const getLanguageId = function (locale) {

    locale = locale.toLowerCase();

    let result = 0;

    switch (locale) {
        case "en-au":
        case "en-ca":
        case "en-gb":
        case "en-in":
        case "en-us":
            result =  0;
            break;
        case "es-es":
        case "es-mx":
        case "es-us":
            result =  1;
            break;
    }

    return result;

}

const LaunchRequestHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'LaunchRequest';
    },
    handle(handlerInput) {

        const locale = handlerInput.requestEnvelope.request.locale;
        const languageId = getLanguageId(locale);
        const speakOutput = getTranslation(languageId, TranslationEnum.WELCOME_MESSAGE);

        return handlerInput.responseBuilder
            .speak(speakOutput)
            .reprompt(speakOutput)
            .getResponse();
    }
};

const GetPlayersAtPositionIntentHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'IntentRequest'
            && Alexa.getIntentName(handlerInput.requestEnvelope) === 'GetPlayersAtPositionIntent';
    },
    async handle(handlerInput) {

        const slots = handlerInput.requestEnvelope.request.intent.slots;

        const locale = handlerInput.requestEnvelope.request.locale;

        const languageId = getLanguageId(locale);
        let rankingTypeId = 0;
        let rankingCategoryId = 0;
        let position = 1;

        if (slots && slots.rankingType && slots.rankingType.value) {
            const rankingType = slots.rankingType.value;
            rankingTypeId = getRankingTypeId(rankingType);
        }

        if (slots && slots.rankingCategory && slots.rankingCategory.value) {
            const rankingCategory = slots.rankingCategory.value;
            rankingCategoryId = getRankingCategoryId(languageId, rankingCategory);
        }
        else {
            if (slots && slots.spanishSingularGender && slots.spanishSingularGender.value && (slots.spanishSingularGender.value === "la")) {
                rankingCategoryId = 1;
            }
        }

        if (slots && slots.position && slots.position.value) {
            position = slots.position.value;
        }

        const url = `${apiUrl}GetPlayersAtPosition?languageId=${languageId}&rankingTypeId=${rankingTypeId}&rankingCategoryId=${rankingCategoryId}&position=${position}`;

        try {
            const response = await getUrlContent(url);

            const speakOutput = response;

            handlerInput.responseBuilder
                .speak(speakOutput);

        } catch (error) {
            console.log(error);

            const speakOutput = getTranslation(languageId, TranslationEnum.ERROR_MESSAGE);

            handlerInput.responseBuilder
                .speak(speakOutput);
        }

        return handlerInput.responseBuilder
            .getResponse();

    }
};

const GetPlayersAtPositionsIntentHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'IntentRequest'
            && Alexa.getIntentName(handlerInput.requestEnvelope) === 'GetPlayersAtPositionsIntent';
    },
    async handle(handlerInput) {

        const slots = handlerInput.requestEnvelope.request.intent.slots;

        const locale = handlerInput.requestEnvelope.request.locale;

        const languageId = getLanguageId(locale);
        let rankingTypeId = 0;
        let rankingCategoryId = 0;
        let numberOfTopPositions = 1;

        if (slots && slots.rankingType && slots.rankingType.value) {
            const rankingType = slots.rankingType.value;
            rankingTypeId = getRankingTypeId(rankingType);
        }

        if (slots && slots.rankingCategory && slots.rankingCategory.value) {
            const rankingCategory = slots.rankingCategory.value;
            rankingCategoryId = getRankingCategoryId(languageId, rankingCategory);
        }
        else {
            if (slots && slots.spanishPluralGender && slots.spanishPluralGender.value && (slots.spanishPluralGender.value === "las")) {
                rankingCategoryId = 1;
            }
        }

        if (slots && slots.position && slots.position.value) {
            numberOfTopPositions = slots.position.value;
        }

        const url = `${apiUrl}GetPlayersAtPositions?languageId=${languageId}&rankingTypeId=${rankingTypeId}&rankingCategoryId=${rankingCategoryId}&numberOfTopPositions=${numberOfTopPositions}`;

        try {
            const response = await getUrlContent(url);

            const speakOutput = response;

            handlerInput.responseBuilder
                .speak(speakOutput);

        } catch (error) {
            console.log(error);

            const speakOutput = getTranslation(languageId, TranslationEnum.ERROR_MESSAGE);

            handlerInput.responseBuilder
                .speak(speakOutput);
        }

        return handlerInput.responseBuilder
            .getResponse();

    }
};

const GetTournamentsIntentHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'IntentRequest'
            && Alexa.getIntentName(handlerInput.requestEnvelope) === 'GetTournamentsIntent';
    },
    async handle(handlerInput) {

        const slots = handlerInput.requestEnvelope.request.intent.slots;

        const locale = handlerInput.requestEnvelope.request.locale;

        const languageId = getLanguageId(locale);
        let rankingTypeId = 0;

        if (slots && slots.rankingType && slots.rankingType.value) {
            const rankingType = slots.rankingType.value;
            rankingTypeId = getRankingTypeId(rankingType);
        }

        const url = `${apiUrl}GetTournaments?languageId=${languageId}&rankingTypeId=${rankingTypeId}`;

        try {
            const response = await getUrlContent(url);

            const speakOutput = response;

            handlerInput.responseBuilder
                .speak(speakOutput);

        } catch (error) {
            console.log(error);

            const speakOutput = getTranslation(languageId, TranslationEnum.ERROR_MESSAGE);

            handlerInput.responseBuilder
                .speak(speakOutput);
        }

        return handlerInput.responseBuilder
            .getResponse();

    }
};

const HelpIntentHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'IntentRequest'
            && Alexa.getIntentName(handlerInput.requestEnvelope) === 'AMAZON.HelpIntent';
    },
    handle(handlerInput) {

        const locale = handlerInput.requestEnvelope.request.locale;

        const languageId = getLanguageId(locale);

        const speakOutput = getTranslation(languageId, TranslationEnum.HELP_MESSAGE);

        return handlerInput.responseBuilder
            .speak(speakOutput)
            .reprompt(speakOutput)
            .getResponse();
    }
};

const CancelAndStopIntentHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'IntentRequest'
            && (Alexa.getIntentName(handlerInput.requestEnvelope) === 'AMAZON.CancelIntent'
                || Alexa.getIntentName(handlerInput.requestEnvelope) === 'AMAZON.StopIntent');
    },
    handle(handlerInput) {

        const locale = handlerInput.requestEnvelope.request.locale;

        const languageId = getLanguageId(locale);

        const speakOutput = getTranslation(languageId, TranslationEnum.GOODBYE_MESSAGE);

        return handlerInput.responseBuilder
            .speak(speakOutput)
            .getResponse();
    }
};

const SessionEndedRequestHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'SessionEndedRequest';
    },
    handle(handlerInput) {
        // Any cleanup logic goes here.
        return handlerInput.responseBuilder.getResponse();
    }
};

// The intent reflector is used for interaction model testing and debugging.
// It will simply repeat the intent the user said. You can create custom handlers
// for your intents by defining them above, then also adding them to the request
// handler chain below.
const IntentReflectorHandler = {
    canHandle(handlerInput) {
        return Alexa.getRequestType(handlerInput.requestEnvelope) === 'IntentRequest';
    },
    handle(handlerInput) {
        const intentName = Alexa.getIntentName(handlerInput.requestEnvelope);
        const speakOutput = `You just triggered ${intentName}`;

        return handlerInput.responseBuilder
            .speak(speakOutput)
            //.reprompt('add a reprompt if you want to keep the session open for the user to respond')
            .getResponse();
    }
};

// Generic error handling to capture any syntax or routing errors. If you receive an error
// stating the request handler chain is not found, you have not implemented a handler for
// the intent being invoked or included it in the skill builder below.
const ErrorHandler = {
    canHandle() {
        return true;
    },
    handle(handlerInput, error) {

        const locale = handlerInput.requestEnvelope.request.locale;

        const languageId = getLanguageId(locale);

        console.log(`~~~~ Error handled: ${error.stack}`);

        const speakOutput = getTranslation(languageId, TranslationEnum.ERROR_MESSAGE);

        return handlerInput.responseBuilder
            .speak(speakOutput)
            .reprompt(speakOutput)
            .getResponse();
    }
};

// The SkillBuilder acts as the entry point for your skill, routing all request and response
// payloads to the handlers above. Make sure any new handlers or interceptors you've
// defined are included below. The order matters - they're processed top to bottom.
exports.handler = Alexa.SkillBuilders.custom()
    .addRequestHandlers(
        LaunchRequestHandler,
        GetPlayersAtPositionIntentHandler,
        GetPlayersAtPositionsIntentHandler,
        GetTournamentsIntentHandler,
        HelpIntentHandler,
        CancelAndStopIntentHandler,
        SessionEndedRequestHandler,
        IntentReflectorHandler, // make sure IntentReflectorHandler is last so it doesn't override your custom intent handlers
    )
    .addErrorHandlers(
        ErrorHandler,
    )
    .lambda();
