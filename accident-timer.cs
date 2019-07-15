/*
Accident Timer (V1.2, 2019-07-15)

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

const String LcdDisplayName = "Harage - Text panel - Accident Timer";
const float LCDFontSize = 2.5f; // Include the "f" at the end of the font size
string LCDAlignment = "Center"; // "Left", "Center", "Right"

const bool UseSpeakers = false;
const String SpeakerName = "Harage - Sound Block - Accident";
const int TimeToPlaySound = 1;

// [End Of Configuration]

public Program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update10;
}
// Variable for counting every 10th tick
public int Tick10 = 0;
// Variable for counting every 60 ticks
public int Second = 0;
// Displayed time
public int Time;
// Unit of time (eg: seconds, minutes, hours)
public string TimeUnit = "";

public void Save()
{
  Storage = Second.ToString();
	Echo("Saved");
}

public void Main(string action)

{
  var lcdDisplay = (IMyTextPanel) GridTerminalSystem.GetBlockWithName(LcdDisplayName);


  var Speaker = (IMySoundBlock) GridTerminalSystem.GetBlockWithName(SpeakerName);
  
  var TimeKeyValuePair = CalculateTime(Second);
  var SecondTime = TimeKeyValuePair.Key.ToString();
  var SecondTimeUnit = TimeKeyValuePair.Value;

  // Error checking

  if ( lcdDisplay == null || lcdDisplay.CubeGrid.GetCubeBlock( lcdDisplay.Position) == null)
  {
    Echo("ERROR: No LCD display found with the name provided.");
    return;
  }

  // Formatting LCD screens
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
    Second = Int32.Parse(Storage);
  }

  Tick10++;

  if (Tick10 >= 6)
  {
    Tick10 = 0;
    Second++;
    Storage = Second.ToString();
  }
  if (UseSpeakers)
  {
    if ( Speaker == null || Speaker.CubeGrid.GetCubeBlock( Speaker.Position) == null)
    {
      Echo("ERROR: No speaker found with the name provided.");
      return;
    } 
    if (Second == TimeToPlaySound)
    {
      Speaker.Stop();
    }
  }
  lcdDisplay.WriteText("\n"+SecondTime+" "+SecondTimeUnit+(Time == 1 ? "": "S")+"\n"+"ACCIDENT FREE");
  Echo("\n"+SecondTime+" "+SecondTimeUnit+(Time == 1 ? "": "S")+"\n"+"ACCIDENT FREE");
}

public KeyValuePair<int, string> CalculateTime(int timeInSeconds)
{
  if (timeInSeconds <= 59)
  {
    TimeUnit = "SECOND";
    Time = timeInSeconds;
    return new KeyValuePair<int, string>(Time, TimeUnit);
  }
  else if (timeInSeconds >= 60 && timeInSeconds <= 3599)
  {
    TimeUnit = "MINUTE";
    Time = Convert.ToInt16(timeInSeconds / 60);
    return new KeyValuePair<int, string>(Time, TimeUnit);
  }
  else if (timeInSeconds >= 3600 && timeInSeconds <= 86399)
  {
    TimeUnit = "HOUR";
    Time = Convert.ToInt16(timeInSeconds / 3600);
    return new KeyValuePair<int, string>(Time, TimeUnit);
  }
    else if (timeInSeconds >= 86400)
  {
    TimeUnit = "DAY";
    Time = Convert.ToInt16(timeInSeconds / 86400);
    return new KeyValuePair<int, string>(Time, TimeUnit);
  }
  TimeUnit = "SECOND";
  Time = timeInSeconds;
  return new KeyValuePair<int, string>(Time, TimeUnit);
}