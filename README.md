# TimeMySlides


**TimeMySlides** (or **TMS**) is a C# application that help you for your public presentations.

> The timer can be started/stopped by any remote presenter (the remote presenter must have a "Black Screen" function).

## What it does (or can do):

- **Display** colored elapsed time in ***BIG*** (usefull if you are far from the screen).
- **Prompt** subtitles at specified time.
- **Warn** you at a specified time before the next subtitle.
- **Offset** the starting time (for training purpose).
- **Map** the "Black Screen" function (B key) to start/stop the timer.

## Resources

>Binaries link to be added.

## Building

>Open sln file with visual studio 2015 (I use community edition).
Build solution and get binaries in the \TimeMySlides\bin\Debug folder.

##Use

###Edit TimeMySlides.xml

Set the Window start size (min size: 950x550):

```xml
  <ScreenXSize>1200</ScreenXSize>
  <ScreenYSize>600</ScreenYSize>
```

Set the starting time offset (usefull to start at an arbitrary time):

```xml
  <StartTime>00:00:18</StartTime>
```

Set the list of subtitles:

```xml
  <SubTitles>
    <SubTitle>
      <!--Time for displaying text-->
      <Time>00:00:00</Time>
      <!--Text to display (can be on multiple lines and depends on window size)-->
      <Text>First white time at time 00:00:00 
      Red warning at 00:00:10</Text>
      <!-- Time color = red|orange|yellow|blue|white(default) -->
      <Color>white</Color>

      <!-- Warning is optionnal  -->
      <!--Warn user before next subtitle-->
      <!--The following exemple set the text in red color 10s before the next subtitle-->
      <WarningTime>00:00:10</WarningTime>
      <WarningColor>red</WarningColor>
    </SubTitle>

    <SubTitle>
      <Time>00:00:20</Time>
      <Text>Yellow time at 00:00:20 
      Blue warning at 00:00:25</Text>
      <Color>yellow</Color>
    </SubTitle>
  </SubTitles>
```