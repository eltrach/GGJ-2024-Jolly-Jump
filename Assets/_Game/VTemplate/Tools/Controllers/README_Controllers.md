# Controllers

## Description

The **Controller** is a controller system that you can use. You can create as many controllers as you want. For now, two types of controllers are available, the **JoystickController** and the **SwerveController**, both of them also **supporting keyboard controls**.

The interfaces used are :

* **ITouchController** and its implementation **TouchControllerImpl**
* **IJoystickController** (inherits from ITouchController) and its implementation **JoystickControllerImpl** (inherits from TouchControllerImpl)
* **ISwerveController** (inherits from ITouchController) and its implementation **SwerveControllerImpl** (inherits from TouchControllerImpl)

## Usage

### IJoystickController

JoystickControllerImpl is a native C# class implementing IJoystickController, managed only via code. You can create a joystick using (you must give your score a persistent MonoBehaviour as the Update function runs as a Coroutine) :

```plaintext
IJoystickController joystick = new JoystickControllerImpl(mono);
```

You can also give it a radius, by default it's 10% of the Screen.width

IJoystickController also gives you access to the event **OnJoystickInputReceived** allowing you to use the result of the input like so :

```plaintext
joystick.OnJoystickInputReceived += Move;

void Move (Vector2 direction)
{
    transform.Translate(new Vector3(direction.x, 0, direction.y) * _speed * Time.deltaTime);
}
```

### ISwerveController

Same behaviour than IJoystickController :

```plaintext
ISwerveController swerve = new SwerveControllerImpl(mono);
```

ISwerveController also gives you access to the event **OnSwerveInputReceived** allowing you to use the result of the input like so :

```plaintext
swerve.OnSwerveInputReceived += Move;

void Move (float screenPercentageDelta)
{
    transform.Translate(new Vector3(screenPercentageDelta * (_maxX - _minX) * _sensitivity, 0, 0));

    if (transform.position.x < _minX)
        transform.Translate(Vector3.right * (_minX - transform.position.x));
    if (transform.position.x > _maxX)
        transform.Translate(Vector3.left * (transform.position.x - _maxX));
}
```

### ITouchController

Both controllers inherits from ITouchController, giving access to 3 events if you need :

* **OnTouchStart** raised on the first clic
* **OnTouching** raised as long as the player keeps his input
* **OnTouchEnd** raised when the player stops the input

## Advanced

These controllers are meant to be used as basic controllers, they are not supposed to replace your controller's code. If you want, you can create your own controllers using this architecture, check how the **JoystickControllerImpl** and the **SwerveControllerImpl** are made. Basically, you can inherit from **TouchControllerImpl** and **ITouchController** (or you can create another interface, like IJoystickController, that inherits from ITouchController, if you need more informations than just OnTouchStart, OnTouching and OnTouchEnd).