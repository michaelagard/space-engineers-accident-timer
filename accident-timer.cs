const String LcdDisplayName = "Harage - Text panel - Accident Timer";
const String SpeakerName = "Harage - Sound Block - Accident";

public Program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update10;
}

public int tick10 = 0;
public int second = 0;
public int Time;

public void Save()
{
  Storage = second.ToString();
	Echo("Saved");
}

public void Main(string action)
{
  var LcdDisplay = (IMyTextPanel) GridTerminalSystem.GetBlockWithName(LcdDisplayName);
  var Speaker = (IMySoundBlock) GridTerminalSystem.GetBlockWithName(SpeakerName);

  LcdDisplay.ContentType=ContentType.TEXT_AND_IMAGE;
  LcdDisplay.SetValue( "alignment", (Int64)2 );
  LcdDisplay.FontSize = 2.5f;

  string TimeUnit = "";

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

  if (second <= 60)
  {
    TimeUnit = " SECOND";
    Time = second;
  }
  else if (second > 60 && second < 3600)
  {
    TimeUnit = " MINUTE";
    Time = Convert.ToInt16(second / 60);
  }
  else if (second > 3600 && second < 86400)
  {
    TimeUnit = " HOUR";
    Time = Convert.ToInt16(second / 3600);
  }
    else if (second > 86400)
  {
    TimeUnit = " DAY";
    Time = Convert.ToInt16(second / 86400);
  }
  
  LcdDisplay.WriteText("\n"+Time+" "+TimeUnit+(Time == 1 ? "": "S")+"\n"+"ACCIDENT FREE");
  Echo("\n"+Time+" "+TimeUnit+(Time == 1 ? "": "S")+"\n"+"ACCIDENT FREE");
}