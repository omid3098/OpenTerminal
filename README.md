# Unity OpenTerminal (Mobile support!)

## Features

- Add cheat codes to your game.
- Enable GOD MODE while testing your code.
- Shows return type of a command in terminal
- Custom Terminal configs.
- AutoComplete: press tab and see available commands. thanks to [@kenculate](https://github.com/kenculate)
- NEW: Report your game log to an email address(useful for testers)

Now you can simply do [almost!] anything at runtime using OpenTerminal!

![open-terminal](https://user-images.githubusercontent.com/6388730/84697557-9be01b00-af63-11ea-9971-e3e4dd3922c3.gif)

## How to use

- Add [TerminalCommand("commandName", "commandDescription")] Attribute to public
  MonoBehaviour methods.
- Create an empty game object and add Terminal component to it.
- Run your game and press ` (the button usually on top of Tab button). On mobile long press with 4 fingers to show terminal.
- Type your commandName and it will be execute!

## Enable terminal in mobile

![4-fingers-tap-gesture-icon](https://user-images.githubusercontent.com/6388730/28248214-f352bd1c-6a55-11e7-9bdf-bccced72bb9e.png)

## Code usage

Simple usage:

```csharp
    [TerminalCommand("rotate-cube", "rotates the cube")]
    public void RotateTheCube()
    {
        stop = false;
    }
```

With parameters:

```csharp
    [TerminalCommand("move-cube", "move-cube(x,y,z) Moves the cube")]
    public void Move(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }
```

## Change theme

Edit config file to change fonts and colors

![image](https://user-images.githubusercontent.com/6388730/27377905-8dd0b4b8-568b-11e7-83f0-775d943773a9.png)

## Limitations

- Does not support vectors or other parameters which contain "," as method input.

## License

[MIT License](LICENSE)

## Contacts

Telegram: [@omid3098](https://t.me/omid3098)  
Twitter: [@omid3098](https://twitter.com/omid3098)  
E-Mail: [info@omid-saadat.com](mailto:info@omid-saadat.com)
