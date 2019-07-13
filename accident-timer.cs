/*
Accident Timer (V1.1, 2019-07-13)

This script will count the number of in game seconds and return the time that has passed in
seconds, minutes, hours, and days. This information will be displayed on a configurable LCD
panel and echo in the console of the programmable block. An optional sound block can be
configured to play when the ResetTime argument is passed.

This script accepts a 7 arguments, that are self explainatory, which can be passed to a button
press linked to the programming block this script is running on.
- ResetTime
- AddSecond
- AddMinute
- AddHour
- RemoveSecond
- RemoveMinute
- RemoveHour
*/ 

// [Configuration]

const String LCDDisplayName = "Harage - Text panel - Accident Timer";
const float LCDFontSize = 2.5f; // Include the "f" at the end of the font size
string LCDAlignment = "Center"; // "Left", "Center", "Right"

const bool UseSpeakers = true;
const String SpeakerName = "Harage - Sound Block - Accident";
const int TimeToPlaySound = 1;

// [End Of Configuration]

public Program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update10;
}
// Variable for counting every 10th tick
public int tick10 = 0;
// Variable for counting every 60 ticks
public int second = 0;
// Displayed time
public int time;
// Unit of time (eg: seconds, minutes, hours)
public string timeUnit = "";

public void Save()
{
  Storage = second.ToString();
	Echo("Saved");
}

public void Main(string action)
{
  var lcdDisplay = (IMyTextPanel) GridTerminalSystem.GetBlockWithName(LCDDisplayName);
  var Speaker = (IMySoundBlock) GridTerminalSystem.GetBlockWithName(SpeakerName);
  
  var TimeKeyValuePair = CalculateTime(second);
  var SecondTime = TimeKeyValuePair.Key.ToString();
  var SecondTimeUnit = TimeKeyValuePair.Value;

  // Configure LCD screen
  lcdDisplay.ContentType=ContentType.TEXT_AND_IMAGE;
  lcdDisplay.FontSize = LCDFontSize;
  switch (LCDAlignment)
  {
    case "Left":
      lcdDisplay.SetValue( "alignment", (Int64)0 );
      break;
    case "Right":
      lcdDisplay.SetValue( "alignment", (Int64)1 );
      break;
    case "Center":
      lcdDisplay.SetValue( "alignment", (Int64)2 );
      break;
  }


  switch (action)
  {
    case "ResetTime":
      Storage = "0";
      if (UseSpeakers)
      {
        Speaker.SelectedSound = "SoundBlockAlert1";
        Speaker.Play();
      }
      break;
    case "AddSecond":
      Storage = (Int32.Parse(Storage) + 1).ToString();
      break;
    case "AddMinute":
      Storage = (Int32.Parse(Storage) + 60).ToString();
      break;
    case "AddHour":
      Storage = (Int32.Parse(Storage) + 3600).ToString();
      break;
    case "RemoveSecond":
      Storage = (Int32.Parse(Storage) - 1).ToString();
      break;
    case "RemoveMinute":
      Storage = (Int32.Parse(Storage) - 60).ToString();
      break;
    case "RemoveHour":
      Storage = (Int32.Parse(Storage) - 3600).ToString();
      break;
  }

  if (Storage != "")
  {
    second = Int32.Parse(Storage);
  }

  tick10++;

  if (tick10 >= 6)
  {
    tick10 = 0;
    second++;
    Storage = second.ToString();
  }
  if (UseSpeakers)
  {
    if (second == TimeToPlaySound)
    {
      Speaker.Stop();
    }
  }
  lcdDisplay.WriteText("\n"+SecondTime+" "+SecondTimeUnit+(time == 1 ? "": "S")+"\n"+"ACCIDENT FREE");
  Echo("\n"+SecondTime+" "+SecondTimeUnit+(time == 1 ? "": "S")+"\n"+"ACCIDENT FREE");
}

public KeyValuePair<int, string> CalculateTime(int timeInSeconds)
{
  if (timeInSeconds <= 59)
  {
    timeUnit = "SECOND";
    time = timeInSeconds;
    return new KeyValuePair<int, string>(time, timeUnit);
  }
  else if (timeInSeconds >= 60 && timeInSeconds <= 3599)
  {
    timeUnit = "MINUTE";
    time = Convert.ToInt16(timeInSeconds / 60);
    return new KeyValuePair<int, string>(time, timeUnit);
  }
  else if (timeInSeconds >= 3600 && timeInSeconds <= 86399)
  {
    timeUnit = "HOUR";
    time = Convert.ToInt16(timeInSeconds / 3600);
    return new KeyValuePair<int, string>(time, timeUnit);
  }
    else if (timeInSeconds >= 86400)
  {
    timeUnit = "DAY";
    time = Convert.ToInt16(timeInSeconds / 86400);
    return new KeyValuePair<int, string>(time, timeUnit);
  }
  timeUnit = "SECOND";
  time = timeInSeconds;
  return new KeyValuePair<int, string>(time, timeUnit);
}