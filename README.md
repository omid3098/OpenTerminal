# Unity OpenTerminal
Terminal Command line for Unity3D


![openterminal_show](https://user-images.githubusercontent.com/6388730/27379276-e5f96a0a-568f-11e7-9df7-dd341c0c9491.gif)


## How to use: 
- Add [Command("commandName", "commandDescription")] Attribute to your methods.
- Create an empty game object and add Terminal component to it.
- Run your game and press ` (the button usually on top of Tab button).
- Type your commandName and it will be execute!

![usage](https://user-images.githubusercontent.com/6388730/27379156-71ef502a-568f-11e7-826c-527442951ee5.gif)


### Features:
- AutoComplete: press tab and see available commands

![openterminal_autocomplete_](https://user-images.githubusercontent.com/6388730/27424496-735677c0-574a-11e7-82a3-ce15522d0ac5.gif)

- Custom Terminal configs.

![image](https://user-images.githubusercontent.com/6388730/27377905-8dd0b4b8-568b-11e7-83f0-775d943773a9.png)

- Supports commands with simple parameters (int, string, float, double ... )

![openterminal_params](https://user-images.githubusercontent.com/6388730/27377435-34db691c-568a-11e7-9a29-89bea9755378.gif)
- Can show return type of a command in terminal

TODO:
- support vectors or other parameters which contain ",";
