const String lcdDisplayName = "Harage - Text panel - Accident Timer";
const String SpeakerName = "Harage - Sound Block - Accident";

public Program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update10;
}
// Variable for counting every 10th tick
public int tick10 = 0;
// variable for counting every 60 ticks
public int second = 0;
// displayed time
public int time;
// unit of time (eg: seconds, minutes, hours)
public string timeUnit = "";

public void Save()
{
  Storage = second.ToString();
	Echo("Saved");
}

public void Main(string action)
{
  var lcdDisplay = (IMyTextPanel) GridTerminalSystem.GetBlockWithName(lcdDisplayName);
  var Speaker = (IMySoundBlock) GridTerminalSystem.GetBlockWithName(SpeakerName);
  
  var TimeKeyValuePair = CalculateTime(second);
  var SecondTime = TimeKeyValuePair.Key.ToString();
  var SecondTimeUnit = TimeKeyValuePair.Value;
  
  lcdDisplay.ContentType=ContentType.TEXT_AND_IMAGE;
  lcdDisplay.SetValue( "alignment", (Int64)2 );
  lcdDisplay.FontSize = 2.5f;

  switch (action)
  {
    case "ResetTime":
      Storage = "0";
      Speaker.SelectedSound = "SoundBlockAlert1";
      Speaker.Play();
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

  if (second == 5)
  {
    Speaker.Stop();
  }

  // Echo(SecondTime+" "+SecondTimeUnit);
  
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