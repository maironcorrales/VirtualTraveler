/* eslint-disable  func-names */
/* eslint-disable  no-console */

const Alexa = require('ask-sdk');
var AWS = require('aws-sdk');
AWS.config.update({region: 'us-east-1'});
var alexaCookbook = require('./alexa-cookbook.js');
// Step 1: Require the alexaplusunity module and set it to alexaPlusUnityClass
var alexaPlusUnityClass = require('alexaplusunity');
var alexaPlusUnity = new alexaPlusUnityClass("pub-c-2e1c3538-77eb-4ca9-be43-2e65bf5a4329", "sub-c-80e3921e-84b1-11ea-9e86-0adc820ce981", true); //Third parameter enables verbose logging

const speechOutputs = {
  launch: {
    speak: {
      setup: [
        " Before we begin playing, we need to go through some setup. I have sent your player ID to your Alexa app. You will need to input this ID in the game when prompted."
      ],
      normal: [
        "Welcome to the Virtual traveler! if you are ready say continue to see the next options, and remember, that from now on you are able to use voice commands as well as the V.R controller. "  
      ],
    },
    reprompt: [
      " What shall we do?"
    ]
  },
  errors: {
    speak: [
      "Error!",
      "There was an issue!"
    ],
    reprompt: [
      " Please try again.",
      " Please try again later."
    ]
  },
};

const LaunchRequestHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'LaunchRequest';
  },
  async handle(handlerInput) {
    const attributesManager = handlerInput.attributesManager;
    const responseBuilder = handlerInput.responseBuilder;

    var attributes = await attributesManager.getPersistentAttributes() || {};
    attributes = await setAttributes(attributes);
    
    if(attributes == null) {
      return ErrorHandler.handle(handlerInput, "Error setting attributes... Check logs");
    }

    var reprompt = alexaCookbook.getRandomItem(speechOutputs.launch.reprompt);
    var speechText = alexaCookbook.getRandomItem(speechOutputs.launch.speak.normal);
    
    var response = responseBuilder
        .speak(speechText + reprompt)
        .reprompt(reprompt)
        .getResponse();

    // Step 2: Add SETUP_STATE check
    if(attributes.SETUP_STATE == "STARTED") {
      var launchSetUpResult = await launchSetUp(speechText, reprompt, handlerInput, attributes);
      attributes = launchSetUpResult.attributes;
      response = launchSetUpResult.response;
    }

    attributesManager.setPersistentAttributes(attributes);
    await attributesManager.savePersistentAttributes();
    return response;
  }
};

const InProgressFlipSwitchIntentHandler = {
  canHandle(handlerInput) {
    const request = handlerInput.requestEnvelope.request;
    return request.type === 'IntentRequest' &&
      request.intent.name === 'FlipSwitchIntent' &&
      request.dialogState !== 'COMPLETED';
  },
  handle(handlerInput) {
    const currentIntent = handlerInput.requestEnvelope.request.intent;
    return handlerInput.responseBuilder
      .addDelegateDirective(currentIntent)
      .getResponse();
  },
}

const CompletedFlipSwitchIntentHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'IntentRequest'
      && handlerInput.requestEnvelope.request.intent.name === 'FlipSwitchIntent';
  },
  async handle(handlerInput) {
    const state = handlerInput.requestEnvelope.request.intent.slots.State.value;

    const speechText = 'Light is now ' + state + '!';
    const reprompt = ' What\'s next?';

    var attributes = await handlerInput.attributesManager.getPersistentAttributes()

    // Step 3: Create the payload for turning on/off the light
    var payloadObj = {
      type: "State",
      message: state
    };

    // Step 4: Add alexaPlusUnity.publishEvent and send our payload
    var response = await alexaPlusUnity.publishMessage(payloadObj, attributes.PUBNUB_CHANNEL).then((data) => {
    return handlerInput.responseBuilder
        .speak(speechText + reprompt)
        .reprompt(reprompt)
        .getResponse();
    }).catch((err) => {
        return ErrorHandler.handle(handlerInput, err);
    });

    return response;
  }
};

const InProgressChangeColorIntentHandler = {
  canHandle(handlerInput) {
    const request = handlerInput.requestEnvelope.request;
    return request.type === 'IntentRequest' &&
      request.intent.name === 'ChangeColorIntent' &&
      request.dialogState !== 'COMPLETED';
  },
  handle(handlerInput) {
    const currentIntent = handlerInput.requestEnvelope.request.intent;
    return handlerInput.responseBuilder
      .addDelegateDirective(currentIntent)
      .getResponse();
  },
}

const CompletedChangeColorIntentHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'IntentRequest'
      && handlerInput.requestEnvelope.request.intent.name === 'ChangeColorIntent';
  },
  async handle(handlerInput) {
    const color = handlerInput.requestEnvelope.request.intent.slots.Color.value;

    const speechText = 'Light is now ' + color + '!';
    const reprompt = ' What\'s next?';

    var attributes = await handlerInput.attributesManager.getPersistentAttributes();

    // Step 5: Create the payload for changing the light color
    var payloadObj = {
      type: "Color",
      message: color
    };

    // Step 6: Add alexaPlusUnity.publishEvent and send our payload
    var response = await alexaPlusUnity.publishMessage(payloadObj, attributes.PUBNUB_CHANNEL).then((data) => {
      return handlerInput.responseBuilder
          .speak(speechText + reprompt)
          .reprompt(reprompt)
          .getResponse();
    }).catch((err) => {
        return ErrorHandler.handle(handlerInput, err);
    });
    
    return response;
  },
};

const GetColorIntentHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'IntentRequest'
      && handlerInput.requestEnvelope.request.intent.name === 'GetColorIntent';
  },
  async handle(handlerInput) {
    var attributes = await handlerInput.attributesManager.getPersistentAttributes();
    
    const speechText = 'The light is currently ' + attributes.color + '!';
    const reprompt = ' What\'s next?';
    
    return handlerInput.responseBuilder
        .speak(speechText + reprompt)
        .reprompt(reprompt)
        .getResponse();
  }
}

const InProgressRequestInfoIntentHandler={
  canHandle(handlerInput) {
    const request = handlerInput.requestEnvelope.request;
    return request.type === 'IntentRequest' &&
      request.intent.name === 'RequestInfoIntentHandler' &&
      request.dialogState !== 'COMPLETED';
  },
  handle(handlerInput) {
    const currentIntent = handlerInput.requestEnvelope.request.intent;
    return handlerInput.responseBuilder
      .addDelegateDirective(currentIntent)
      .getResponse();
  },
}
const CompleteRequestInfoIntentHandler ={
  canHandle(handlerInput){
    return handlerInput.requestEnvelope.request.type === "IntentRequest"
      && handlerInput.requestEnvelope.request.intent.name === "RequestInfoIntent";
  },
  async handle(handlerInput){
    var attributes = await handlerInput.attributesManager.getPersistentAttributes();
    var speechText = '';
    var reprompt = ' Anything else?';

    var payloadObj = {
      type: "SceneInformation",
      message: "info"
    };
    var response = await alexaPlusUnity.publishMessageAndListenToResponse(payloadObj, attributes.PUBNUB_CHANNEL, 4000).then((data) => {
      speechText = data.message.SceneInfo + " ";
   
      return handlerInput.responseBuilder
         .speak(speechText + reprompt)
         .reprompt(reprompt)
         .getResponse();
    }).catch((err) => {
        return ErrorHandler.handle(handlerInput, err);
    });
    
    return response;

  },

}

const ContinueIntentHandler ={
    canHandle(handlerInput){
      return handlerInput.requestEnvelope.request.type === "IntentRequest"
        && handlerInput.requestEnvelope.request.intent.name === "ContinueIntent";
    },
    async handle(handlerInput){
      var attributes = await handlerInput.attributesManager.getPersistentAttributes();
      const speechText = "Continuing to next options. ";
      const reprompt = "What\'s next?";
      var payloadObj = {
        type: "Continue",
        message: "continue"
      };
      var response = await alexaPlusUnity.publishMessage(payloadObj, attributes.PUBNUB_CHANNEL).then((data) => {
        return handlerInput.responseBuilder
            .speak(speechText + reprompt)
            .reprompt(reprompt)
            .getResponse();
      }).catch((err) => {
          return ErrorHandler.handle(handlerInput, err);
      });
      return response;
    },
};

const GoToSceneIntentHandler ={
  canHandle(handlerInput){
    return handlerInput.requestEnvelope.request.type === "IntentRequest"
      && handlerInput.requestEnvelope.request.intent.name === "GoToSceneIntent";
  },

  async handle(handlerInput){
    var attributes = await handlerInput.attributesManager.getPersistentAttributes();
      const speechText = "Lets go! ";
      const reprompt = "Let me know if you need something while you do the tour!";
      var payloadObj = {
        type: "GoToLocation",
        message: "location"
      };
      var response = await alexaPlusUnity.publishMessage(payloadObj, attributes.PUBNUB_CHANNEL).then((data) => {
        return handlerInput.responseBuilder
            .speak(speechText + reprompt)
            .reprompt(reprompt)
            .getResponse();
      }).catch((err) => {
          return ErrorHandler.handle(handlerInput, err);
      });
      return response;
  },
};

const InProgressGetObjectInDirectionIntentHandler = {
  canHandle(handlerInput) {
    const request = handlerInput.requestEnvelope.request;
    return request.type === 'IntentRequest' &&
      request.intent.name === 'GetObjectInDirectionIntent' &&
      request.dialogState !== 'COMPLETED';
  },
  handle(handlerInput) {
    const currentIntent = handlerInput.requestEnvelope.request.intent;
    return handlerInput.responseBuilder
      .addDelegateDirective(currentIntent)
      .getResponse();
  },
}

const CompletedGetObjectInDirectionIntentHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'IntentRequest'
      && handlerInput.requestEnvelope.request.intent.name === 'GetObjectInDirectionIntent';
  },
  async handle(handlerInput) {
    const direction = handlerInput.requestEnvelope.request.intent.slots.Direction.value;
    const direction_id = handlerInput.requestEnvelope.request.intent.slots.Direction.resolutions.resolutionsPerAuthority[0].values[0].value.id;
    var attributes = await handlerInput.attributesManager.getPersistentAttributes();
    var speechText = '';
    var reprompt = ' What\'s next?';

    // Step 7: Create the payload for getting object in a direction
    var payloadObj = {
      type: "GetObject",
      message: direction_id
    };

    // Step 8: Add alexaPlusUnity.publishMessageAndListenToResponse and send our payload
    var response = await alexaPlusUnity.publishMessageAndListenToResponse(payloadObj, attributes.PUBNUB_CHANNEL, 4000).then((data) => {
      speechText = 'Currently, ' + data.message.object + ' is ' + direction + ' of you!';
   
      return handlerInput.responseBuilder
         .speak(speechText + reprompt)
         .reprompt(reprompt)
         .getResponse();
    }).catch((err) => {
        return ErrorHandler.handle(handlerInput, err);
    });
    
    return response;
  }
}

const HelpIntentHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'IntentRequest'
      && handlerInput.requestEnvelope.request.intent.name === 'AMAZON.HelpIntent';
  },
  handle(handlerInput) {
    const speechText = 'You can say turn on to turn on the light!';

    return handlerInput.responseBuilder
      .speak(speechText)
      .reprompt(speechText)
      .withSimpleCard('Alexa Plus Unity Test', speechText)
      .getResponse();
  },
};

const CancelAndStopIntentHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'IntentRequest'
      && (handlerInput.requestEnvelope.request.intent.name === 'AMAZON.CancelIntent'
        || handlerInput.requestEnvelope.request.intent.name === 'AMAZON.StopIntent');
  },
  handle(handlerInput) {
    const speechText = 'Goodbye!';

    return handlerInput.responseBuilder
      .speak(speechText)
      .withSimpleCard('Alexa Plus Unity Test', speechText)
      .getResponse();
  },
};

const SessionEndedRequestHandler = {
  canHandle(handlerInput) {
    return handlerInput.requestEnvelope.request.type === 'SessionEndedRequest';
  },
  handle(handlerInput) {
    console.log(`Session ended with reason: ${handlerInput.requestEnvelope.request.reason}`);

    return handlerInput.responseBuilder.getResponse();
  },
};

const ErrorHandler = {
  canHandle() {
    return true;
  },
  handle(handlerInput, error) {
    console.log(`Error handled: ${error.message}`);

    var errorReprompt = alexaCookbook.getRandomItem(speechOutputs.errors.reprompt);
    var errorSpeech = alexaCookbook.getRandomItem(speechOutputs.errors.speak) + errorReprompt;
    return handlerInput.responseBuilder
      .speak(errorSpeech)
      .reprompt(errorReprompt)
      .getResponse();
  },
};

const skillBuilder = Alexa.SkillBuilders.standard();

exports.handler = skillBuilder
  .addRequestHandlers(
    LaunchRequestHandler,
    InProgressFlipSwitchIntentHandler,
    CompletedFlipSwitchIntentHandler,
    InProgressChangeColorIntentHandler,
    CompletedChangeColorIntentHandler,
    GoToSceneIntentHandler,
    GetColorIntentHandler,
    InProgressGetObjectInDirectionIntentHandler,
    CompletedGetObjectInDirectionIntentHandler,
    InProgressRequestInfoIntentHandler,
    CompleteRequestInfoIntentHandler,
    HelpIntentHandler,
    ContinueIntentHandler,
    CancelAndStopIntentHandler,
    SessionEndedRequestHandler
  )
  .addErrorHandlers(ErrorHandler)
  .withTableName('AlexaPlusUnityTest')
  .withAutoCreateTable(true)
  .lambda();

  async function launchSetUp(speechText, reprompt, handlerInput, attributes) {
    const responseBuilder = handlerInput.responseBuilder;
  
    speechText += alexaCookbook.getRandomItem(speechOutputs.launch.speak.setup) + reprompt;
  
    // Step 9: Create a Pubnub and send it to user for them to input in the game
    var response = await alexaPlusUnity.addChannelToGroup(attributes.PUBNUB_CHANNEL, "virtualtraveler").then(async (data) => {
      var responseToReturn = responseBuilder
          .speak(speechText)
          .reprompt(reprompt)
          .withSimpleCard('Games SDK for Alexa', "Here is your Player ID: " + attributes.PUBNUB_CHANNEL)
          .getResponse();
      
      var userId = handlerInput.requestEnvelope.session.user.userId;
      return await sendUserId(userId, attributes, handlerInput, responseToReturn);
      }).catch((err) => {
          return ErrorHandler.handle(handlerInput, err);
      });
  
    var result = {
      response: response,
      attributes: attributes
    }
    return result;
  }
  
  async function sendUserId(userId, attributes, handlerInput, response) {
  
    // Step 10: Create a payload that has the user's alexa id
    var payloadObj = {
      type: "AlexaUserId",
      message: userId
    };
  
    // Step 11: Add alexaPlusUnity.publishEvent and send our payload
    
    return await alexaPlusUnity.publishMessage(payloadObj, attributes.PUBNUB_CHANNEL).then((data) => {
      return response;
    }).catch((err) => {
        return ErrorHandler.handle(handlerInput, err);
    });
  }
  
  async function setAttributes(attributes) {
    if (Object.keys(attributes).length === 0) {
      // Step 12: Initialize the attributes
      attributes.SETUP_STATE = "STARTED";
      var newChannel = await alexaPlusUnity.uniqueQueueGenerator("AlexaPlusUnityTest");

      if(newChannel != null) {
          attributes.PUBNUB_CHANNEL = newChannel;
          console.log(newChannel);
      } else {
          return null;
      }
      
      //Add more attributes here that need to be initalized at skill start
    }
    return attributes;
  }
