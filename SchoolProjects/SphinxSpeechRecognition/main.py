
from Sphinx_SpeechRecognition import *


if __name__ == "__main__":
	myKeyWords = ("one", "two", "three", "four", "shut down")
	defaultKeyWord = "apply"
	
	# Deafult keyWord is used to make sure, that command is not pronounced by accident.
	# e.g. 	"apply shut down" is valid command.
	#		"shut down" is NOT a valid command.

	speechRecogniser = SpeechRecogniser()
	
	# Default keyWord and its confidence, e.g. apply
	speechRecogniser.SetDefaultKeyWord(defaultKeyWord, 0.85)
	# Array of keywords and its confidence
	speechRecogniser.SetKeyWords(myKeyWords, 0.75)
	# Full Commands -> Commands with key word
	speechRecogniser.CreateFullCommands()
	# Duration for adjusting the recogniser - computing Threshold
	speechRecogniser.SetDurationForAdjustingRecogniser(3)
	# If allowed, threshold ich editing in runtime
	speechRecogniser.AllowDynamicEnergyThreshold(True)
	# Pause Threshold
	speechRecogniser.SetPauseThreshold(0.5)
	# Dynamic energy adjustment damping
	speechRecogniser.SetDynamicEnergyAdjustmentDamping(0.13)
	# Dynamic energy ratio
	speechRecogniser.SetDynamicEnergyRatio(1.5)
	# Phrase threshold
	speechRecogniser.SetPhraseThreshold(0.15)
	# Non speaking duration
	speechRecogniser.SetNonSpeakingDuration(0.5)
	
	# Initialize speech recognition engine
	speechRecogniser.InitializeRecogniser()
	
	while True:
		speechRecogniser.Recognise()
		output = speechRecogniser.GetResult()
		
		if output:
			print(output)