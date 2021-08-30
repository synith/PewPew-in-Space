# OOP-theory-demo: PewPew in Space

This mini game is a demonstration of my knowledge regarding the four pillars of Object-Oriented Programming.


<ins><b>Abstraction</b></ins>:<br> 
Replacing blocks of code with methods in the update and trigger/collider methods to hide some of the calculations and conditional branches with abstract method names that explain their function but hide the messier looking code, therefore increasing readability and reusability.


<ins><b>Encapsulation</b></ins>:<br> 
Using get and set accessors to control access to and/or verify certain values. An example of this would be where you have a variable that can be read by other scripts but only written to within its own script. Another example would be where you only want to set a variable with positive numbers, but not negative numbers; you could use an if branch to only allow numbers equal or greater than zero to be assigned to the variable and return an error on anything else.


<ins><b>Inheritance</b></ins>:<br> 
Instead of writing out the same code for multiple similar objects, you can create a child class from a parent class, having the child class use the code from the parent class and then building on top of that. This way sibling classes can differentiate from each other yet still share much of the same code. This is done using the Class : Parent notation when declaring the class. The protected keyword (an example of encapsulation) allows you to share access to inherited scripts


<ins><b>Polymorphism</b></ins>:<br>
When using inheritance you can modify or override the parents code (using the virtual and override keywords) allowing you to use parts of the parent code but not all, or even change inherited code. You can also use polymorphism to overload a method with multiple different parameters, for example a method that applies a color to an object could be given a color parameter or individual R, G, B integers as parameters.
