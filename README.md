# Accident Timer (V1.1, 2019-07-13)

This script will count the number of in game seconds and return the time that has passed in seconds, minutes, hours, and days. This information will be displayed on a configurable LCD panel and echo in the console of the programmable block. An optional sound block can be configured to play when the ResetTime argument is passed.

This script accepts a 7 arguments which can be passed to a button press linked to the programming block this script is running on.
- ResetTime - Resets the time to 0 and plays the speaker block if configured.
- AddSecond - Adds one second to the timer.
- AddMinute - Adds one minute to the timer.
- AddHour - Adds one hour to the timer.
- RemoveSecond - Removes one second to the timer.
- RemoveMinute - Removes one minute to the timer.
- RemoveHour - Removes one hour to the timer.
