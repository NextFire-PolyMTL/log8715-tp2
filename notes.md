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
