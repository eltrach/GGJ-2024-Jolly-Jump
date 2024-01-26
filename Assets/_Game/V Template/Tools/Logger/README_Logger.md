# Logger

## Description

The **Logger** is a simple Log system class based on the Debug.Log class from Unity. Its purpose is to allow log filtering so that you can enable / disable certain types of logs.\
The **Logger** is a static class that you can use the same way as the Debug.Log class.

## Usage

### Logger

To use the Logger, simply call :

```plaintext
Logger.Log(object message, LogLevel logLevel);
Logger.Log(object message, LogLevel logLevel, LoggerSettings settings);
```

All logs will be **prefixed** by _\[MWM\]\[Tag\]_ and **colored** if you give it a color (hexadecimal format) on the LoggerSettings. \
You can choose not to give it a tag nor a color, and it will be displayed as a regular Debug.Log only prefixed with \[MWM\].

### LoggerPrefs

To choose which tags you want to display, go inside the LoggerPrefs class and change the LogTagsToDisplay list :

* Leave it null or empty and it will display all logs

  ```plaintext
  public static List<string> LogTagsToDisplay = new List<string>();
  ```
* Give it all tags you want to display and it will only display these logs

  ```plaintext
  public static List<string> LogTagsToDisplay = new List<string>()
  {
     "GameCoordinator",
     "DataManager"
  };
  ```
* Give it only string.Empty as tag and it will not display any logs (except if you tagged some "")

  ```plaintext
  public static List<string> LogTagsToDisplay = new List<string>()
  {
     "",
  };
  ```