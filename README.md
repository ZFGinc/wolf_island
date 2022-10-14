# Wolf Island 
### (Van Tassel D. Style, development, efficiency, debugging and testing programs. - M .: Mir, 1981)

*** 

# Formulation of the problem:
### A 20 x 20 wolf island is inhabited by wild rabbits, wolves and she-wolves. There are several representatives of each species.

### Rabbits are rather stupid: at any given time, they have the same 1/9 chance of moving to one of the eight neighboring squares (except for areas bounded by the coastline) or simply sitting still. Each rabbit with a probability of 0.2 turns into two rabbits.

### Each she-wolf moves randomly until one of the eight neighboring squares contains a rabbit she is hunting. If the she-wolf and the rabbit are in the same square, the she-wolf eats the rabbit and scores one point. Otherwise, she loses 0.1 points. Wolves and she-wolves with zero points die.

### The wolf behaves like a she-wolf until all the rabbits in the neighboring squares disappear; then, if the she-wolf is in one of the eight nearby squares, the wolf chases after her. If a wolf and a she-wolf are in the same square, and there is no rabbit to eat, they produce offspring of a random sex.

### At the initial moment of time, all wolves and she-wolves have 1 point.

*** 

## Write a program that implements this ecological model and observe the change in the population over a certain period of time.
