
import speech_recognition as speechRec

class SpeechRecogniser:
    allowDynamicEnergyThreshold = False
    deafultKeyWord = ""
    defaultWordConfidence = 0
    durationOfAdjustingRecogniser = 0
    dynamicEnergyAdjustmentDamping = 0
    dynamicEnergyRatio = 0
    fullCommands = ""
    keyWords = ""
    keyWordsConfidence = 0
    keyWordsForResolving = ""
    nonSpeakingDuration = 0
    output = ""   
    pauseThreshold = 0   
    phraseThreshold = 0    
    recogniser = None
    source = None



    def InitializeRecogniser(self):
        global allowDynamicEnergyThreshold
        global deafultKeyWord
        global defaultWordConfidence
        global durationOfAdjustingRecogniser
        global dynamicEnergyAdjustmentDamping
        global dynamicEnergyRatio
        global fullCommands
        global keyWords
        global keyWordsConfidence
        global keyWordsForResolving
        global nonSpeakingDuration
        global output
        global pauseThreshold  
        global phraseThreshold        
        global recogniser
        global source
        

        recogniser = speechRec.Recognizer()

        with speechRec.Microphone() as source:
            recogniser.adjust_for_ambient_noise(source, duration = durationOfAdjustingRecogniser)
            recogniser.pause_threshold = pauseThreshold # seconds of non-speaking audio before a phrase is considered complete
            recogniser.dynamic_energy_threshold = allowDynamicEnergyThreshold

            recogniser.dynamic_energy_adjustment_damping = dynamicEnergyAdjustmentDamping #0.13
            recogniser.dynamic_energy_ratio = dynamicEnergyRatio #1.5

            # seconds after an internal operation (e.g., an API request) 
            # starts before it times out, or ``None`` for no timeout
            recogniser.operation_timeout = None

            # minimum seconds of speaking audio before we consider the speaking 
            # audio a phrase - values below this are ignored (for filtering out clicks and pops)
            recogniser.phrase_threshold = phraseThreshold #0.15

            # seconds of non-speaking audio to keep on both sides of the recording
            recogniser.non_speaking_duration = nonSpeakingDuration #0.5

    def Recognise(self):
        global defaultWordConfidence
        global keyWordsConfidence
        global deafultKeyWord
        global keyWords
        global output
        global recogniser
        global source

        with speechRec.Microphone() as source:
            audio = recogniser.listen(source)

            # recognize speech using Sphinx
            try:
                result = recogniser.recognize_sphinx(audio,language='en-US', keyword_entries = keyWords)
                # output = self.FindLongestRecognisedPhrase(result)
                output = self.FindFirstRecognisedPhrase(result)

            except speechRec.UnknownValueError:
                output = "Sphinx could not understand audio"
            except speechRec.RequestError as e:
                output = "Sphinx error; {0}".format(e)

    

    def CreateFullCommands(self):
        global fullCommands
        global deafultKeyWord
        global keyWordsForResolving
        global keyWordsConfidence

        fullCommands = [""] * len(keyWordsForResolving)
        
        for i in range(0, len(keyWordsForResolving)):
            fullCommands[i] = str(deafultKeyWord + ' ' + keyWordsForResolving[i])
            keyWords[i] = (fullCommands[i], keyWordsConfidence)




    def SetDefaultKeyWord(self, word, wordConfidence):
        global defaultWordConfidence
        global deafultKeyWord

        # set default keyWord, e.g. apply
        if isinstance(word, str) == True:
            deafultKeyWord = word
        else:
            raise Exception("Wrong type of variable. Set string.")

        # set confidence for default keyWord, e.g. 0.85
        if isinstance(wordConfidence, (int, float)):
            if wordConfidence >= 0 and wordConfidence <= 1:
                defaultWordConfidence = wordConfidence
            else:
                raise Exception("Set value of confidence between 0 and 1")
        else:
            raise Exception("Wrong type of variable. Set double.")





    def SetDurationForAdjustingRecogniser(self, duration):
        global durationOfAdjustingRecogniser

        # sets the value in seconds
        if isinstance(duration, int):
            if duration >= 0 and duration <= 30:
                durationOfAdjustingRecogniser = duration
            else:
                raise Exception("Set value of confidence between 0 and 30")
        else:
            raise Exception("Wrong type of variable. Set integer.")





    def AllowDynamicEnergyThreshold(self, value):
        global allowDynamicEnergyThreshold

        if isinstance(value, bool):
            allowDynamicEnergyThreshold = value
        else:
            raise Exception("Wrong type of variable. Set bool.")





    def SetPauseThreshold(self, value):
        global pauseThreshold

        if isinstance(value, (int, float)):
            if value >= 0.5 and value <= 30:
                pauseThreshold = value
            else:
                raise Exception("Set value of confidence between 0.5 and 30")
        else:
            raise Exception("Wrong type of variable. Set double.")





    def SetKeyWords(self, keyWord, confidence):
        global keyWords
        global keyWordsConfidence
        global keyWordsForResolving

        if all(isinstance(item, str) for item in keyWord) == False:
            raise Exception("Wrong type of variable. Set string.")

        # set confidence for default keyWord, e.g. 0.85
        if isinstance(confidence, (int, float)):
            if confidence >= 0 and confidence <= 1:
                keyWordsConfidence = confidence
            else:
                raise Exception("Set value of confidence between 0 and 1")
        else:
            raise Exception("Wrong type of variable. Set double.")

        keyWords = [None] * len(keyWord)
        keyWordsForResolving = [None] * len(keyWord)

        for i in range(0, len(keyWord)):
            keyWords[i] = (keyWord[i], keyWordsConfidence)
            keyWordsForResolving[i] = keyWord[i]




    def SetDynamicEnergyAdjustmentDamping(self, damping):
        global dynamicEnergyAdjustmentDamping

        if isinstance(damping, (int, float)):
            if damping >= 0 and damping <= 1:
                dynamicEnergyAdjustmentDamping = damping
            else:
                raise Exception("Set value of confidence between 0 and 1")
        else:
            raise Exception("Wrong type of variable. Set double.")




    def SetDynamicEnergyRatio(self, ratio):
        global dynamicEnergyRatio

        if isinstance(ratio, (int, float)):
            if ratio >= 1 and ratio <= 20:
                dynamicEnergyRatio = ratio
            else:
                raise Exception("Set value of confidence between 1 and 20")
        else:
            raise Exception("Wrong type of variable. Set double.")




    def SetPhraseThreshold(self, threshold):
        global phraseThreshold

        if isinstance(threshold, (int, float)):
            if threshold >= 0 and threshold <= 1:
                phraseThreshold = threshold
            else:
                raise Exception("Set value of confidence between 0 and 1")
        else:
            raise Exception("Wrong type of variable. Set double.")




    def SetNonSpeakingDuration(self, duration):
        global nonSpeakingDuration

        if isinstance(duration, (int, float)):
            if duration >= 0.5 and duration <= 30:
                nonSpeakingDuration = duration
            else:
                raise Exception("Set value of confidence between 0.5 and 30")
        else:
            raise Exception("Wrong type of variable. Set double.")





    def FindFirstRecognisedPhrase(self, recognisedString):
        global keyWordsForResolving
        global deafultKeyWord

        indexes = [254] * len(keyWordsForResolving)

        for i in range(0, len(keyWordsForResolving)):
            if keyWordsForResolving[i] in recognisedString:
                indexes[i] = recognisedString.index(keyWordsForResolving[i])

        bestResultIndex = min(indexes)
        iIndex = indexes.index(bestResultIndex)

        ans = keyWordsForResolving[iIndex].replace(deafultKeyWord + ' ', '')
        return ans



    def FindLongestRecognisedPhrase(self, recognisedString):
        global keyWordsForResolving
        global deafultKeyWord

        numberOfSpacesInKeyWords = [0] * len(keyWordsForResolving)

        for i in range(0, len(keyWordsForResolving)):
            if keyWordsForResolving[i] in recognisedString:
                numberOfSpacesInKeyWords[i] = keyWordsForResolving[i].count(' ')

        maxNumberOfSpaces = max(numberOfSpacesInKeyWords)

        if maxNumberOfSpaces == 0:
            return self.FindFirstRecognisedPhrase(recognisedString)
        else:
            bestResultIndex = numberOfSpacesInKeyWords.index(maxNumberOfSpaces)
            ans = keyWordsForResolving[bestResultIndex].replace(deafultKeyWord + ' ', '')
            return ans



    def GetResult(self):
        global output
        return str(output)



def GetSpeechRecogniserInstance():
    mySpeechRecogniser = SpeechRecogniser()
    return mySpeechRecogniser