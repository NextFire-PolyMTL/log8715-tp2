## Base things
Entities
- Plants
- Preys
- Predators

### Common
- Disappear after some time
- Countdown can accelerate

### PLant
- Immobile
- Size decreases until 0
- Respawn at some random place when life 0
- Dying speed *2 each time a prey touches it

### Prey
- Moves at constant speed
- If preys touch each other, they activate a reproduction flag
- When die, if flag -> respawn at random
- Die speed/2 each time touche plant
- Die speed *2 each time predator touches it

### Predator
- Constant speed towards closest pret
- If predators touch each other, they activate a reproduction flag
- If flag -> respawn random
- die speed/2 each time touches prey

## Goals/TODO List
- IJob implementation for single thread w/ burst
- Job parallelism
- Algorithm upgrade
- 30FPS with
  - 3000 Pl
  - 3000 Prey
  - 2500 Pred
  - 12000 grid size

### Subgoals
- Have transform in the structures to deal with it via the job system


### <span style="color:red; font-weight:bold">Questions</span>
- How does the reproduce part doesn't enable a entity to reproduce with itself ??
  - Same question for the readonly accesses etc...
- Is the vector this really unavoidable, does putting getters/setters work if then we put `class.attribute` in the job handler (~~test first to see if it breaks before asking maybe~~) Tested, doesn't work, at least doesn't seem to pass reference.
- What are the limits of modification? Like change prefabs, components, ... as long as the config still works.
- `TransformAccessArray` -> Seems ok?? But having a `NativeArray<Vector3>` = way better imo, can we setup some kind of reference

### Include in report
- Job handling is the same for all lifetime changers because they are really similar.
- I use empty array aka `NativeArrays` of size 0 for things that aren't needed since
  - I can't send a null pointer because a NativeArray can't be a nullpointer.
  - The foreach loop could have an unexpected behaviour insted of just not looping at all since the array is empty.
- Usage of the converting methods to arrays, the `NativeArray`s of `Vector3`  because the `Transform`s don't work. Explaination for the parameter aray for class attriutes.
