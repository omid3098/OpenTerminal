# Unity OpenTerminal (Mobile support!)

Have you ever:
- Wanted to use cheat codes for your games?
- Enable GOD MODE while testing some features?
- Add some coins without leaving the game?
- Enable/Disable parts of your code at runtime without leaving playmode?

Now you can simply do [almost!] anything at runtime using OpenTerminal!


![openterminal_show](https://user-images.githubusercontent.com/6388730/27379276-e5f96a0a-568f-11e7-9df7-dd341c0c9491.gif)


## How to use: 
- Add [TerminalCommand("commandName", "commandDescription")] Attribute to public
    MonoBehaviour methods.
- Create an empty game object and add Terminal component to it.
- Run your game and press ` (the button usually on top of Tab button). On mobile long press with 4 fingers to show terminal.
- Type your commandName and it will be execute!

## Enable terminal in mobile:
![4-fingers-tap-gesture-icon](https://user-images.githubusercontent.com/6388730/28248214-f352bd1c-6a55-11e7-9bdf-bccced72bb9e.png)

## Code usage:
![usage](https://user-images.githubusercontent.com/6388730/27379156-71ef502a-568f-11e7-826c-527442951ee5.gif)


### Features:
- AutoComplete: press tab and see available commands

![openterminal_autocomplete_](https://user-images.githubusercontent.com/6388730/27424496-735677c0-574a-11e7-82a3-ce15522d0ac5.gif)

- Custom Terminal configs.

![image](https://user-images.githubusercontent.com/6388730/27377905-8dd0b4b8-568b-11e7-83f0-775d943773a9.png)

- Supports commands with simple parameters (int, string, float, double ... )

![openterminal_params](https://user-images.githubusercontent.com/6388730/27377435-34db691c-568a-11e7-9a29-89bea9755378.gif)
- Can show return type of a command in terminal

## Contact: 
  you can find me [@omid3098](https://twitter.com/omid3098) in twitter.

TODO:
- support vectors or other parameters which contain ",";
