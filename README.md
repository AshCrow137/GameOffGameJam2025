# GameOffGameJam2025
November GameOff game jame; by Stars team


Unity version: 6000.0.61f1 without any additional modules (like WEBGL or android\IOS support)

Try to avoid using MonoBehavior, if possible. 

In MonoBehaviors, if possible, try to avoid using Start and Awake methods, instead create Initialize() method and put the code from Start and Awake() in it. Then, if the object represent on scene, create a variable (using [SerializeField]) in BootManager, put the object from scene into BootManager and call Initialize() method from the variable. In that case we can control initialisation order and avoid some issues in future.
<img width="453" height="279" alt="image" src="https://github.com/user-attachments/assets/3b7fa0a9-11d3-431c-a082-c9af8d52f556" />


All public Enum must be placed in EnumLibrary file 

<img width="273" height="322" alt="image" src="https://github.com/user-attachments/assets/7099c638-64cb-4d17-a5b0-ad75055c78b4" />



If you use UnityEvents, global events must be placed in static class GlobalEventManager. You can create any events and methods that invoke these events in GlobalEventManager. 
<img width="844" height="247" alt="image" src="https://github.com/user-attachments/assets/695ec407-787b-4ce6-b1f8-3b04fbcd2649" />

<img width="592" height="319" alt="image" src="https://github.com/user-attachments/assets/74a3599a-a3b2-4a02-96b6-d489c5b337b5" />



All random variables must be seeded, using custom randomness class 
